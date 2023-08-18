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

        public GameManagerResolve3MatchState() : base()
        {
            _tileBoardManager = GameManager.TileBoardManager;
            
            GetAllMatchedTileFromTileBoard();
            PopAllMatchedTile();
            AddTask(new GameManagerPlayAllMatchedTilePopActionTask(this, _matchedTileResultList));
            // InsertNewTilesAndPlayAction();
        }

        private void GetAllMatchedTileFromTileBoard()
        {
            _tileBoardManager.FindAllMatchedTile();
            _matchedTileResultList = _tileBoardManager.GetMatchedTileResultList();
        }

        private void PopAllMatchedTile()
        {
            _tileBoardManager.PopAllMatchedTiles();
        }

        private void InsertNewTilesAndPlayAction() 
        {
            foreach(Tile tile in _matchedTileResultList)
            {
                tile.PlayNewTileInsertAction();
            }
        }
        
        public override void OnFinishAllTask()
        {
            // in resolve 3match state, find all 3 match tiles and pop them.
            // and insert new tiles into tileboard.
            // after that, change state to IdleState.

            GameManager.ChangeState(new GameManagerIdleState());

            // TODO
            // if all pop tile effect ends, change state to idle.
            // if (IsNewTilesInserted())
            // {
            //     GameManager.ChangeState(new GameManagerIdleState());
            // }
        }

        private bool IsNewTilesInserted()
        {
            return false;
        }
    }
}
