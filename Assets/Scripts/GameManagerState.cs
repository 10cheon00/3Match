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

    public GameManagerIdleState()
    {
        _tileSwapHandler = GameManager.TileSwapHandler;
    }

    public override void Handle()
    {
        // in idle state, just wait until player select tiles
        // when player selected, change state to SwappingState.
        if (_tileSwapHandler.IsPlayerSelectedTwoTiles())
        {
            TilePair tiles = _tileSwapHandler.GetSelectedTiles();
            _tileSwapHandler.Reset();

            ChangeState(new GameManagerSwapTwoTilesState(tiles));
        }
    }
}

public class GameManagerSwapTwoTilesState : GameManagerState
{
    private TilePair _tiles;
    private bool _isSwappingEffectFinished;

    public GameManagerSwapTwoTilesState(TilePair tiles)
    {
        _tiles = tiles;
        _isSwappingEffectFinished = false;

        // swap two tile in tileboard.
        // play swapping tile effect.
        SwapTwoTilesAndPlayEffect();
    }

    private void SwapTwoTilesAndPlayEffect()
    {
        GameManager.TileBoardManager.SwapTwoTiles(_tiles);

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
        if (_isSwappingEffectFinished)
        {
            ChangeState(new GameManagerResolve3MatchState());
        }
    }

    private void UpdateState()
    {
        _isSwappingEffectFinished =
            _tiles.Item1.IsSwappingEffectFinished && _tiles.Item2.IsSwappingEffectFinished;
    }
}

public class GameManagerResolve3MatchState : GameManagerState
{
    private MatchedTilesList _matchedTilesList;
    private TileBoardManager _tileBoardManager;

    public GameManagerResolve3MatchState()
    {
        _tileBoardManager = GameManager.TileBoardManager;
    }

    public override void Handle()
    {
        // in resolve 3match state, find all 3 match tiles and pop them.
        // and insert new tiles into tileboard.
        // after that, change state to IdleState.

        FindAllMatchedTiles();
        PopAllMatchedTilesAndPlayEffect();
        InsertNewTiles();

        GameManager.ChangeState(new GameManagerIdleState());
    }

    private void FindAllMatchedTiles()
    {
        _tileBoardManager.FindAll3MatchTiles();
        _matchedTilesList = _tileBoardManager.GetMatchedTilesList();
    }

    private void PopAllMatchedTilesAndPlayEffect()
    {
        _tileBoardManager.PopAllMatchedTiles();
    }

    private void InsertNewTiles() { }
}
