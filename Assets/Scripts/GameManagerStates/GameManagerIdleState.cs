using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates
{
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
                TilePair tilePair = _tileSwapHandler.GetSelectedTiles();
                _tileSwapHandler.Reset();
                ChangeState(new GameManagerSwapTwoTilesState(tilePair));
            }
        }
    }
}
