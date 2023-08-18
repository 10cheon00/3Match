using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.GameManagerStates.GameManagerTasks;

namespace Assets.Scripts.GameManagerStates
{
    public class GameManagerSwapTwoTilesState : GameManagerState
    {
        private TilePair _tilePair;

        public GameManagerSwapTwoTilesState(TilePair tilePair) : base()
        {
            _tilePair = tilePair;

            GameManager.TileBoardManager.SwapTwoTiles(_tilePair);
            AddTask(new GameManagerTilePairSwapActionTask(this, _tilePair));
            // SwapTwoTilesAndPlayEffect();
        }

        public override void OnFinishAllTask()
        {
            // in swapping state, swap two tiles and play swapping effect.
            // after end of swapping effect, change state to Resolve3Match.

            ChangeState(new GameManagerResolve3MatchState());
        }
    }
}
