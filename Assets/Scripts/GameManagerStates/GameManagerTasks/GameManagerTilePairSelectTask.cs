using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates.GameManagerTasks
{
    public class GameManagerTilePairSelectTask : GameManagerTask
    {
        private TileSwapHandler _tileSwapHandler;

        public GameManagerTilePairSelectTask(GameManagerState gameManagerState, TileSwapHandler tileSwapHandler)
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
