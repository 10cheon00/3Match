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
        (int destX, int destY) = GetTileIndexes(dest);

        Debug.Assert(srcX != -1 && srcY != -1 && destX != -1 && destY != -1);

        if(AreTwoTilesAdjacent(srcX, srcY, destX, destY))
        {
            SwapTileInTileBoard(srcX, srcY, destX, destY);
            PlayTileSwapEffect(src, dest);
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

    private void PlayTileSwapEffect(Tile src, Tile dest)
    {
        Vector3 midPoint = Vector3.Lerp(src.transform.position, dest.transform.position, 0.5f);
        src.Start180DegreeRotation(midPoint);
        dest.Start180DegreeRotation(midPoint);
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
