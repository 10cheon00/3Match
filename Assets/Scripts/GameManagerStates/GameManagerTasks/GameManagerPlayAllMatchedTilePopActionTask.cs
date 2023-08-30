using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates.GameManagerTasks
{
    public class GameManagerPlayAllMatchedTilePopActionTask : GameManagerTask
    {
        private MatchedTileList _matchedTileList;

        public GameManagerPlayAllMatchedTilePopActionTask(
            GameManagerState gameManagerState,
            MatchedTileList matchedTileList
        ) : base(gameManagerState)
        {
            _matchedTileList = matchedTileList;

            foreach(Tile tile in _matchedTileList)
            {
                if (tile.IsReadyToPlayTileAction())
                {
                    tile.PlayPopAction();
                }
            }
        }

        public override void RunTask()
        {
            bool isPopActionFinished = true;
            foreach(Tile tile in _matchedTileList)
            {
                if (tile.IsReadyToPlayTileAction() == false)
                {
                    isPopActionFinished = false;
                }
            }
            if(isPopActionFinished)
            {
                FinishTask();
            }
        }
    }
}
