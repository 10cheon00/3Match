using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/*
Board의 역할
1. 게임에 사용될 자료구조를 관리한다.
*/

public class TileBoardManager : MonoBehaviour
{
    private TileBoard _tileBoard;

    public void Initialize(TileBoard tileBoard)
    {
        _tileBoard = tileBoard;
    }

    public void SwapTile(Tile src, Tile dest)
    {
        (int srcX, int srcY) = GetTileIndexes(src);
        (int destX, int destY) = GetTileIndexes(src);

        Debug.Assert(srcX != -1 && srcY != -1 && destX != -1 && destY != -1);

        SwapTileInTileBoard(srcX, srcY, destX, destY);
        SwapTileObjectPosition(src, dest);
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

    private void SwapTileInTileBoard(int srcX, int srcY, int destX, int destY)
    {
        (_tileBoard[destY][destX], _tileBoard[srcY][srcX]) = (
            _tileBoard[srcY][srcX],
            _tileBoard[destY][destX]
        );
    }

    private void SwapTileObjectPosition(Tile src, Tile dest)
    {
        (dest.transform.localPosition, src.transform.localPosition) = (
            src.transform.localPosition,
            dest.transform.localPosition
        );
    }

    private void Resolve3Match() { }
}
