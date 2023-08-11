using System.Collections;
using System.Collections.Generic;
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
}
