using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Assets.Scripts.GameManagerStates;

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

    private GameManagerState _gameManagerState;

    private void Start()
    {
        _tileBoardManager.Initialize();

        GameManagerState.SetGameManager(this);
        ChangeState(new GameManagerIdleState());
    }

    void Update()
    {
        _gameManagerState.Run();
    }

    public void ChangeState(GameManagerState state)
    {
        _gameManagerState = state;
    }
}
