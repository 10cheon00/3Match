using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates.GameManagerTasks
{
    public class GameManagerInsertNewTilesTask : GameManagerTask
    {
        public GameManagerInsertNewTilesTask(
            GameManagerState gameManagerState,
            MatchedTileList matchedTileList
        ) : base(gameManagerState) 
        {
            
        }

        public override void RunTask() { }
    }
}
