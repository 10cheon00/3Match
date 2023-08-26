using System;
using System.Collections.Generic;
using UnityEngine;

/*
Board의 역할
1. 게임에 사용될 자료구조를 관리한다.
*/

struct Coord
{
    public int x,
        y;

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Coord operator +(Coord a, Coord b) => new Coord(a.x + b.x, a.y + b.y);
}

public class MatchedTileList : List<Tile> { }

public class TileBoardManager : MonoBehaviour
{
    private TilePair _tilePair;
    private TileBoard _tileBoard;

    private MatchedTileList _matchedTileResultList;
    private List<(Tile, Coord)> _foundMatchedTiles;
    private List<List<bool>> _isMatchedTile;
    private Coord[] _directionList;

    public void Initialize(TileBoard tileBoard)
    {
        _tileBoard = tileBoard;
        _matchedTileResultList = new();
        _foundMatchedTiles = new();
        _isMatchedTile = new();
        _directionList = new Coord[]
        {
            new Coord(-1, 0),
            new Coord(0, -1),
            new Coord(1, 0),
            new Coord(0, 1)
        };
    }

#region SwapTwoTiles
    public void SwapTwoTiles(TilePair tilePair)
    {
        _tilePair = tilePair;

        Coord srcCoord = GetTileIndexes(_tilePair.tileA);
        Coord destCoord = GetTileIndexes(_tilePair.tileB);

        Debug.Assert(srcCoord.x != -1 && srcCoord.y != -1 && destCoord.x != -1 && destCoord.y != -1);

        if (AreTwoTilesAdjacent(srcCoord, destCoord))
        {
            SwapTileInTileBoard(srcCoord, destCoord);
        }
    }

    private Coord GetTileIndexes(Tile tile)
    {
        for (int i = 0; i < _tileBoard.Count; i++)
        {
            for (int j = 0; j < _tileBoard[i].Count; j++)
            {
                if (_tileBoard[i][j] == tile)
                {
                    return new(j, i);
                }
            }
        }
        return new(-1, -1);
    }

    private bool AreTwoTilesAdjacent(Coord coordA, Coord coordB)
    {
        int xDiff = Mathf.Abs(coordA.x - coordB.x);
        int yDiff = Mathf.Abs(coordA.y - coordB.y);
        return xDiff + yDiff == 1;
    }

    private void SwapTileInTileBoard(Coord coordA, Coord coordB)
    {
        (_tileBoard[coordB.y][coordB.x], _tileBoard[coordA.y][coordA.x]) = (
            _tileBoard[coordA.y][coordA.x],
            _tileBoard[coordB.y][coordB.x]
        );
    }
#endregion SwapTwoTiles

#region Resolve3Match
    public void FindAllMatchedTile()
    {
        InitializeMatchedTileBoard();

        for (int i = 0; i < _tileBoard.Count; i++)
        {
            for (int j = 0; j < _tileBoard[i].Count; j++)
            {
                FindMatchedTilesOnCoord(new(j, i));
            }
        }
    }

    private void InitializeMatchedTileBoard()
    {
        _isMatchedTile.Clear();
        for (int i = 0; i < _tileBoard.Count; i++)
        {
            _isMatchedTile.Add(new());
            for (int j = 0; j < _tileBoard[i].Count; j++)
            {
                _isMatchedTile[i].Add(false);
            }
        }
    }

    private void FindMatchedTilesOnCoord(Coord coord)
    {

        foreach (Coord direction in _directionList)
        {
            _foundMatchedTiles.Clear();
            _foundMatchedTiles.Add((_tileBoard[coord.y][coord.x], coord));
            FindMatchedTilesOnCoordWithDirection(coord, direction);

            if (IsFoundMatchedTilesCountGreaterThan2() && IsNotAlreadyFoundMatchedTile())
            {
                AddMatchedTilesToResultList();
            }
        }
    }

    private void FindMatchedTilesOnCoordWithDirection(Coord coord, Coord direction)
    {
        Coord nextCoord = coord + direction;
        if (IsOutOfRangeCoordInTileBoard(nextCoord))
        {
            return;
        }

        Tile currentTile = _tileBoard[coord.y][coord.x];
        Tile nextTile = _tileBoard[nextCoord.y][nextCoord.x];

        if (currentTile.Color == nextTile.Color)
        {
            _foundMatchedTiles.Add((nextTile, nextCoord));
            FindMatchedTilesOnCoordWithDirection(nextCoord, direction);
        }
    }

    private bool IsOutOfRangeCoordInTileBoard(Coord coord)
    {
        return coord.x < 0
            || coord.y < 0
            || coord.y >= _tileBoard.Count
            || coord.x >= _tileBoard[coord.y].Count;
    }

    private bool IsFoundMatchedTilesCountGreaterThan2()
    {
        return _foundMatchedTiles.Count > 2;
    }

    private bool IsNotAlreadyFoundMatchedTile()
    {
        foreach ((Tile tile, Coord coord) in _foundMatchedTiles)
        {
            if (_isMatchedTile[coord.y][coord.x] == false)
            {
                return true;
            }
        }
        return false;
    }

    private void AddMatchedTilesToResultList()
    {
        foreach ((Tile tile, Coord coord) in _foundMatchedTiles)
        {
            _matchedTileResultList.Add(tile);
            _isMatchedTile[coord.y][coord.x] = true;
        }
    }

    public MatchedTileList GetMatchedTileResultList()
    {
        return _matchedTileResultList;
    }
#endregion Resolve3Match

#region PopAllMatchedTile

    public void DestroyAllMatchedTile()
    {
        foreach(Tile tile in _matchedTileResultList)
        {
            Coord coord = GetTileIndexes(tile);
            _tileBoard[coord.y][coord.x] = null;
            GameObject.Destroy(tile.gameObject);
        }
        for (int i = 0; i < _tileBoard.Count; i++)
        {
            for (int j = 0; j < _tileBoard[i].Count; j++)
            {
                UnityEngine.Debug.Log($"{i} {j} {_tileBoard[i][j] == null}");
            }
        }
    }

#endregion PopAllMatchedTile

    public void ResetTileBoard() { }
}
