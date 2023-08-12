using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
GameManager의 역할
1. 게임의 루프를 관리한다.

게임의 루프
while true:
    if 3-match not exist:
        reset

    if player input:
        while true:
            if 3-match exist:
                pop them
                insert new tile
            else:
                break
*/
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TileBoardFactory _tileBoardFactory;
    [SerializeField]
    private TileBoardManager _tileBoardManager;

    [SerializeField]
    private int xSize = 5;
    private int ySize = 5;

    void Start()
    {
        TileBoard tileBoard = _tileBoardFactory.CreateTileBoard(xSize, ySize);
        _tileBoardManager.Initialize(tileBoard);
    }

    // Update is called once per frame
    void Update() { }
}
