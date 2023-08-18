using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.GameManagerStates.GameManagerTasks;

namespace Assets.Scripts.GameManagerStates
{
    public class GameManagerIdleState : GameManagerState
    {
        private TileSwapHandler _tileSwapHandler;

        public GameManagerIdleState() : base()
        {
            _tileSwapHandler = GameManager.TileSwapHandler;

            AddTask(new GameManagerTilePairSelectTask(this, _tileSwapHandler));
        }

        public override void OnFinishAllTask()
        {
            // in idle state, just wait until player select tiles
            // when player selected, change state to SwappingState.

            TilePair tilePair = _tileSwapHandler.GetSelectedTiles();
            _tileSwapHandler.Reset();
            ChangeState(new GameManagerSwapTwoTilesState(tilePair));

            // if (_tileSwapHandler.IsPlayerSelectedTwoTiles())
            // {
            //     TilePair tilePair = _tileSwapHandler.GetSelectedTiles();
            //     _tileSwapHandler.Reset();
            //     ChangeState(new GameManagerSwapTwoTilesState(tilePair));
            // }
        }
    }
}
