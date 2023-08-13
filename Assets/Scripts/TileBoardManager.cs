using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

using TilePair = System.ValueTuple<Tile, Tile>;

/*
Board의 역할
1. 게임에 사용될 자료구조를 관리한다.
*/

public class MatchedTiles : List<Tile>
{
    public MatchedTiles() { }

    // clone constructor
    public MatchedTiles(MatchedTiles otherMatchedTiles) : base(otherMatchedTiles) { }
}

public class MatchedTilesList : List<MatchedTiles> { }

public class TileBoardManager : MonoBehaviour
{
    private TilePair _tilePair;
    private TileBoard _tileBoard;

    private MatchedTilesList _matchedTilesList;
    private MatchedTiles _matchedTiles;

    public void Initialize(TileBoard tileBoard)
    {
        _tileBoard = tileBoard;
        _matchedTilesList = new();
        _matchedTiles = new();
    }

    public void SwapTwoTiles(TilePair tilePair)
    {
        _tilePair = tilePair;

        (int srcX, int srcY) = GetTileIndexes(_tilePair.Item1);
        (int destX, int destY) = GetTileIndexes(_tilePair.Item2);

        Debug.Assert(srcX != -1 && srcY != -1 && destX != -1 && destY != -1);

        if (AreTwoTilesAdjacent(srcX, srcY, destX, destY))
        {
            SwapTileInTileBoard(srcX, srcY, destX, destY);
            // PlayTileSwapEffect(src, dest);
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

    public void FindAll3MatchTiles()
    {
        _matchedTilesList.Clear();

        for (int i = 0; i < _tileBoard.Count; i++)
        {
            for (int j = 0; j < _tileBoard[i].Count; j++)
            {
                Find3MatchFromTile(j, i);
            }
        }

        // if (Is3MatchExist())
        // {
        //     PopAll3Match();
        // }
        // else
        // {
        //     ResetTileBoard();
        // }
    }

    private void Find3MatchFromTile(int x, int y)
    {
        // find 3 match in each direction
        (int, int)[] directions = { (-1, 0), (1, 0), (0, -1), (0, 1) };
        foreach ((int, int) direction in directions)
        {
            _matchedTiles.Clear();
            _matchedTiles.Add(_tileBoard[y][x]);
            Find3MatchFromTileWithDirection(x, y, direction);
        }
    }

    private void Find3MatchFromTileWithDirection(int x, int y, (int, int) direction)
    {
        int dirX = direction.Item1,
            dirY = direction.Item2;

        if (
            IsIndexOutOfRange(x + dirX, y + dirY)
            || IsNotEqualTwoTilesColor(_tileBoard[y][x], _tileBoard[y + dirY][x + dirX])
        )
        {
            if (IsMatchedTilesCountEqualOrGreaterThan3() && IsNotDuplicatedMatchedTiles())
            {
                _matchedTilesList.Add(new MatchedTiles(_matchedTiles));
            }
            return;
        }
        else
        {
            _matchedTiles.Add(_tileBoard[y + dirY][x + dirX]);
            Find3MatchFromTileWithDirection(x + dirX, y + dirY, direction);
        }
    }

    private bool IsIndexOutOfRange(int x, int y)
    {
        return x < 0 || y < 0 || x >= _tileBoard[0].Count || y >= _tileBoard.Count;
    }

    private bool IsNotEqualTwoTilesColor(Tile tileA, Tile tileB)
    {
        return tileA.Color != tileB.Color;
    }

    private bool IsMatchedTilesCountEqualOrGreaterThan3()
    {
        return _matchedTiles.Count >= 3;
    }

    private bool IsNotDuplicatedMatchedTiles()
    // TODO
    // 두 matchedTiles가 같은 원소를 포함하고 있는지 확인하려면 별도의 정보가 필요하다.
    // 단순히 원소 비교만 해서는 불가능하다.
    // 해시값 비교 또는 삽입할 때 정렬이 되도록 하는 방법이 있겠다.
    {
        foreach (MatchedTiles matchedTiles in _matchedTilesList)
        {
            if (matchedTiles.Count != _matchedTiles.Count)
                continue;

            bool areTwoMatchedTilesEqual = true;
            for (int i = 0; i < _matchedTiles.Count; i++)
            {
                if (_matchedTiles[i] != matchedTiles[i])
                {
                    areTwoMatchedTilesEqual = false;
                    break;
                }
            }
            if (areTwoMatchedTilesEqual)
            {
                return true;
            }
        }
        return true;
    }

    public void PopAllMatchedTiles()
    {
        foreach (MatchedTiles matchedTiles in _matchedTilesList)
        {
            string colors = "";
            matchedTiles.ForEach(
                (tile) =>
                {
                    colors = $"{colors} {tile.Color}";
                }
            );
            Debug.Log(colors);
        }
    }

    public MatchedTilesList GetMatchedTilesList()
    {
        return _matchedTilesList;
    }

    public void ResetTileBoard() { }
}
