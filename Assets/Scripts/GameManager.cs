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
public class GameManager : MonoBehaviour //SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private TileBoardFactory _tileBoardFactory;

    [SerializeField]
    private TileBoardManager _tileBoardManager;

    [SerializeField]
    private TileSwapHandler _tileSwapHandler;

    [SerializeField]
    private int xSize = 5;
    private int ySize = 5;

    private GameManagerState _gameManagerState;

    // public override void InitializeSingletonOnAwake()
    private void Start()
    {
        TileBoard tileBoard = _tileBoardFactory.CreateTileBoard(xSize, ySize, _tileBoardManager);
        _tileBoardManager.Initialize(tileBoard);

        ChangeState(new GameManagerIdleState(_tileSwapHandler));
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

    public void SwapTwoTiles(TilePair tiles)
    {
        // swap two tile in tileboard.
        // play swapping tile effect.
        _tileBoardManager.SwapTwoTiles(tiles);
        Tile tileA = tiles.Item1;
        Tile tileB = tiles.Item2;

        Vector3 midPoint = Vector3.Lerp(tileA.transform.position, tileB.transform.position, 0.5f);
        tileA.Start180DegreeRotation(midPoint);
        tileB.Start180DegreeRotation(midPoint);
    }

    public void SwapTwoTilesCallback()
    {
        ChangeState(new GameManagerResolve3MatchState());
    }

    public void Resolve3Match() { }
}
