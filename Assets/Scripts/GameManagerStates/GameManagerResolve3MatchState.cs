using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.GameManagerStates.GameManagerTasks;

namespace Assets.Scripts.GameManagerStates
{
    public class GameManagerResolve3MatchState : GameManagerState
    {
        private MatchedTileList _matchedTileResultList;
        private TileBoardManager _tileBoardManager;
        private List<Tile> _actionTileList;

        public GameManagerResolve3MatchState() : base()
        {
            _tileBoardManager = GameManager.TileBoardManager;
            
            GetAllMatchedTileFromTileBoard();
            AddTask(new GameManagerPlayAllMatchedTilePopActionTask(this, _matchedTileResultList));
            AddTask(new GameManagerPopAllMatchedTileTask(this, _tileBoardManager));
            AddTask(new GameManagerFillTileBoardTask(this, _tileBoardManager));
        }

        private void GetAllMatchedTileFromTileBoard()
        {
            _tileBoardManager.FindAllMatchedTile();
            _matchedTileResultList = _tileBoardManager.GetMatchedTileResultList();
        }
        
        public override void OnFinishAllTask()
        {
            // in resolve 3match state, find all 3 match tiles and pop them.
            // and insert new tiles into tileboard.
            // after that, change state to IdleState.

            GameManager.ChangeState(new GameManagerIdleState());
        }
    }
}
