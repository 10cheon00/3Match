using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
Board의 역할
1. 게임에 사용될 자료구조를 관리한다.
*/

public class MatchedTileList : List<Tile> { }

public class TileBoardManager : MonoBehaviour
{
    private TilePair _tilePair;
    private TileBoard _tileBoard;
    public TileBoard TileBoard
    {
        get { return _tileBoard; }
    }

    private MatchedTileList _matchedTileResultList;
    private List<(Tile, Coord)> _foundMatchedTiles;
    private List<List<bool>> _isMatchedTile;
    private Coord[] _directionList;

#region TileBoardFactory
    [SerializeField]
    private Coord _tileBoardSize;
    public Coord TileBoardSize
    {
        get { return _tileBoardSize; }
    }
    [SerializeField]
    private TileBoardFactory _tileBoardFactory;
    // TODO
    // this is required just for calculate position by coord in FillTileBoardTask class.
    // FillTileBoardTask class doesn't need factory, so this should be refactored.
    public TileBoardFactory TileBoardFactory
    {
        get { return _tileBoardFactory; }
    }
#endregion TileBoardFactory

    [SerializeField]
    private Tile _nullTile;

    [SerializeField]
    private Text text;

    public void Initialize()
    {
        _tileBoard = _tileBoardFactory.CreateTileBoard(_tileBoardSize);
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

        _nullTile = GetComponentInChildren<Tile>();
    }

#region DEBUG
    private void Update()
    {
        string str = "";
        for (int i = 0; i < _tileBoard.Count; i++)
        {
            for (int j = 0; j < _tileBoard[i].Count; j++)
            {
                if (IsNullTileCoord(new(j, i)))
                {
                    str += "- ";
                }
                else
                {
                    str += $"{(int)_tileBoard[i][j].Color} ";
                }
            }
            str += "\n";
        }
        text.text = str;
    }
#endregion DEBUG

#region SwapTwoTiles
    public void SwapTwoTiles(TilePair tilePair)
    {
        _tilePair = tilePair;

        Coord srcCoord = GetTileCoord(_tilePair.tileA);
        Coord destCoord = GetTileCoord(_tilePair.tileB);

        Debug.Assert(
            srcCoord.x != -1 && srcCoord.y != -1 && destCoord.x != -1 && destCoord.y != -1
        );

        if (AreTwoTilesAdjacent(srcCoord, destCoord))
        {
            SwapTileInTileBoard(srcCoord, destCoord);
        }
    }

    public Coord GetTileCoord(Tile tile)
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
        _matchedTileResultList.Clear();
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
        foreach (Tile tile in _matchedTileResultList)
        {
            Coord coord = GetTileCoord(tile);
            if (IsOutOfRangeCoordInTileBoard(coord) == false)
            {
                DestroyTileInCoord(coord);
            }
        }
    }

    private void DestroyTileInCoord(Coord coord)
    {
        Tile tile = _tileBoard[coord.y][coord.x];
        SetTileToNullTile(coord);
        GameObject.Destroy(tile.gameObject);
    }

    private void SetTileToNullTile(Coord coord)
    {
        // TODO
        // instead of setting null directly into tileboard,
        // set instance of other tile or singleton tile which means null.
        // this pattern will avoid null reference exception.
        _tileBoard[coord.y][coord.x] = _nullTile;
    }

#endregion PopAllMatchedTile

#region FillTileBoard

    public void MoveTileToBottom()
    {
        // swap tile to lowest bottom null tile.
        for (int y = _tileBoard[_tileBoard.Count - 1].Count - 1; y >= 0; y--)
        {
            for (int x = 0; x < _tileBoard[y].Count; x++)
            {
                Coord tileCoord = new(x, y);
                if (IsNullTileCoord(tileCoord))
                {
                    continue;
                }

                Coord lowestNullTileCoord = FindLowestNullTileCoordFromTileCoord(tileCoord);
                SwapTileInTileBoard(tileCoord, lowestNullTileCoord);
            }
        }
    }

    private Coord FindLowestNullTileCoordFromTileCoord(Coord tileCoord)
    {
        while (true)
        {
            tileCoord.y++;
            if (IsOutOfRangeCoordInTileBoard(tileCoord) || IsNotNullTileCoord(tileCoord))
            {
                tileCoord.y -= 1;
                break;
            }
        }

        return tileCoord;
    }

    private bool IsNotNullTileCoord(Coord coord)
    {
        return _tileBoard[coord.y][coord.x] != _nullTile;
    }

    private bool IsNullTileCoord(Coord coord)
    {
        return _tileBoard[coord.y][coord.x] == _nullTile;
    }

    public bool IsNullTile(Tile tile)
    {
        return tile == _nullTile;
    }

    public void CreateNewTilesOnNullTiles()
    {
        for (Coord coord = new(0, 0); coord.y < _tileBoard.Count; coord.y++)
        {
            for (coord.x = 0; coord.x < _tileBoard[coord.y].Count; coord.x++)
            {
                if (IsNullTileCoord(coord))
                {
                    GameObject newTileObject = _tileBoardFactory.CreateRandomTileObject();
                    Tile newTile = newTileObject.GetComponent<Tile>();
                    _tileBoard[coord.y][coord.x] = newTile;
                }
            }
        }
    }

    public TileBoard CreateTileBoardSnapshot()
    {
        TileBoard tileBoard = new();
        for (int i = 0; i < _tileBoard.Count; i++)
        {
            tileBoard.Add(new());
            for (int j = 0; j < _tileBoard[i].Count; j++)
            {
                tileBoard[i].Add(_tileBoard[i][j]);
            }
        }
        return tileBoard;
    }

#endregion FillTileBoard
}
