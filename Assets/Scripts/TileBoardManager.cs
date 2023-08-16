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

        (int srcX, int srcY) = GetTileIndexes(_tilePair.tileA);
        (int destX, int destY) = GetTileIndexes(_tilePair.tileB);

        Debug.Assert(srcX != -1 && srcY != -1 && destX != -1 && destY != -1);

        if (AreTwoTilesAdjacent(srcX, srcY, destX, destY))
        {
            SwapTileInTileBoard(srcX, srcY, destX, destY);
        }
    }

    private (int, int) GetTileIndexes(Tile tile)
    {
        for (int i = 0; i < _tileBoard.Count; i++)
        {
            for (int j = 0; j < _tileBoard[i].Count; j++)
            {
                if (_tileBoard[i][j] == tile)
                {
                    return (j, i);
                }
            }
        }
        return (-1, -1);
    }

    private bool AreTwoTilesAdjacent(int srcX, int srcY, int destX, int destY)
    {
        int xDiff = Mathf.Abs(srcX - destX);
        int yDiff = Mathf.Abs(srcY - destY);
        return xDiff + yDiff == 1;
    }

    private void SwapTileInTileBoard(int srcX, int srcY, int destX, int destY)
    {
        (_tileBoard[destY][destX], _tileBoard[srcY][srcX]) = (
            _tileBoard[srcY][srcX],
            _tileBoard[destY][destX]
        );
    }
#endregion SwapTwoTiles

#region PopAllMatchedTiles
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

            if (IsFoundMatchedTilesCountGreaterThan2() && IsNotDuplicatedFoundMatchedTiles())
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

    private bool IsNotDuplicatedFoundMatchedTiles()
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

    public void PopAllMatchedTiles() { }

    public MatchedTileList GetMatchedTileResultList()
    {
        return _matchedTileResultList;
    }
#endregion PopAll3MatchedTiles

    public void ResetTileBoard() { }
}
