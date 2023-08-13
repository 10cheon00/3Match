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
    private TileSwapHandler _tileSwapHandler;

    public GameManagerIdleState(TileSwapHandler tileSwapHandler)
    {
        _tileSwapHandler = tileSwapHandler;
    }

    public override void Handle()
    {
        // in idle state, just wait until player select tiles
        // when player selected, change state to SwappingState.

        if (_tileSwapHandler.IsPlayerSelectedTwoTiles())
        {
            TilePair tiles = _tileSwapHandler.GetSelectedTiles();
            _tileSwapHandler.Reset();

            ChangeState(new GameManagerSwappingState(tiles));
        }
    }
}

public class GameManagerSwappingState : GameManagerState
{
    private TilePair _tiles;
    private bool _isSwappingFinished;

    public GameManagerSwappingState(TilePair tiles)
    {
        _tiles = tiles;
        _isSwappingFinished = false;
        GameManager.SwapTwoTiles(_tiles);
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
        _isSwappingFinished = _tiles.Item1.IsSwappingEffectFinished && _tiles.Item2.IsSwappingEffectFinished;
    }
}

public class GameManagerResolve3MatchState : GameManagerState
{
    public override void Handle()
    {
        GameManager.Resolve3Match();
        // in resolve 3match state, find all 3 match tiles and pop them.
        // and insert new tiles into tileboard.
        // after that, change state to IdleState
    }
}
