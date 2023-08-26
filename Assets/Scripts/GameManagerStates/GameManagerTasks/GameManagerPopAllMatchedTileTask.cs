using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates.GameManagerTasks
{
    public class GameManagerPopAllMatchedTileTask : GameManagerTask
    {
        private MatchedTileList _matchedTileList;
        private TileBoardManager _tileBoardManager;

        public GameManagerPopAllMatchedTileTask(
            GameManagerState gameManagerState,
            TileBoardManager tileBoardManager,
            MatchedTileList matchedTileList
        ) : base(gameManagerState)
        {
            _matchedTileList = matchedTileList;
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
