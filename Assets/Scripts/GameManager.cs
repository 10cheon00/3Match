using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using TilePair = System.ValueTuple<Tile, Tile>;

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
    public TileBoardManager TileBoardManager
    {
        get { return _tileBoardManager; }
    }

    [SerializeField]
    private TileSwapHandler _tileSwapHandler;
    public TileSwapHandler TileSwapHandler
    {
        get { return _tileSwapHandler; }
    }

    [SerializeField]
    private int xSize = 5;
    private int ySize = 5;

    private GameManagerState _gameManagerState;

    private void Start()
    {
        TileBoard tileBoard = _tileBoardFactory.CreateTileBoard(xSize, ySize, _tileBoardManager);
        _tileBoardManager.Initialize(tileBoard);

        ChangeState(new GameManagerIdleState());
        _gameManagerState.SetGameManager(this);
    }

    void Update()
    {
        _gameManagerState.Handle();
    }

    public void ChangeState(GameManagerState state)
    {
        _gameManagerState = state;
    }
}
