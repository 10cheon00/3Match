using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

using TilePair = System.ValueTuple<Tile, Tile>;

public abstract class GameManagerState
{
    private static GameManager _gameManager;
    protected GameManager GameManager
    {
        get { return _gameManager; }
    }

    public abstract void Handle();

    public void ChangeState(GameManagerState gameManagerState)
    {
        _gameManager.ChangeState(gameManagerState);
    }

    public void SetGameManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
}

public class GameManagerIdleState : GameManagerState
{
    public override void Handle()
    {
        // in idle state, just wait until player select tiles
        // when player selected, change state to SwappingState.
        TileSwapHandler tileSwapHandler = GameManager.TileSwapHandler;
        if (tileSwapHandler.IsPlayerSelectedTwoTiles())
        {

            TilePair tiles = tileSwapHandler.GetSelectedTiles();
            tileSwapHandler.Reset();

            ChangeState(new GameManagerSwapTwoTilesState(tiles));
        }
    }
}

public class GameManagerSwapTwoTilesState : GameManagerState
{
    private TilePair _tiles;
    private bool _isSwappingFinished;

    public GameManagerSwapTwoTilesState(TilePair tiles)
    {
        _tiles = tiles;
        _isSwappingFinished = false;

        // swap two tile in tileboard.
        // play swapping tile effect.
        SwapTwoTilesAndPlayEffect();
    }

    private void SwapTwoTilesAndPlayEffect()
    {
        TileBoardManager tileBoardManager = GameManager.TileBoardManager;

        tileBoardManager.SwapTwoTiles(_tiles);

        Vector3 midPoint = Vector3.Lerp(
            _tiles.Item1.transform.position,
            _tiles.Item2.transform.position,
            0.5f
        );
        _tiles.Item1.Start180DegreeRotation(midPoint);
        _tiles.Item2.Start180DegreeRotation(midPoint);
    }

    public override void Handle()
    {
        // in swapping state, swap two tiles and play swapping effect.
        // after end of swapping effect, change state to Resolve3Match.

        UpdateState();
        if (_isSwappingFinished)
        {
            ChangeState(new GameManagerResolve3MatchState());
        }
    }

    private void UpdateState()
    {
        _isSwappingFinished =
            _tiles.Item1.IsSwappingEffectFinished && _tiles.Item2.IsSwappingEffectFinished;
    }
}

public class GameManagerResolve3MatchState : GameManagerState
{
    public override void Handle()
    {
        // in resolve 3match state, find all 3 match tiles and pop them.
        // and insert new tiles into tileboard.
        // after that, change state to IdleState

        GameManager.ChangeState(new GameManagerIdleState());
    }
}
