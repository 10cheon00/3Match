using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates.GameManagerTasks
{
    public class GameManagerSelectTilePairTask : GameManagerTask
    {
        private TileSwapHandler _tileSwapHandler;

        public GameManagerSelectTilePairTask(GameManagerState gameManagerState, TileSwapHandler tileSwapHandler)
            : base(gameManagerState) 
        {
            _tileSwapHandler = tileSwapHandler;
        }

        public override void RunTask()
        {
            if (_tileSwapHandler.IsPlayerSelectedTwoTiles())
            {
                FinishTask();
            }
        }
    }
}
