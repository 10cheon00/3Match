using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates.GameManagerTasks
{
    public class GameManagerPopAllMatchedTileTask : GameManagerTask
    {
        private TileBoardManager _tileBoardManager;

        public GameManagerPopAllMatchedTileTask(
            GameManagerState gameManagerState,
            TileBoardManager tileBoardManager
        ) : base(gameManagerState)
        {
            _tileBoardManager = tileBoardManager;            
        }

        public override void RunTask() 
        {
            // destroy all matched tile.
            // and finish.
            _tileBoardManager.DestroyAllMatchedTile();
            FinishTask();
        }
    }
}
