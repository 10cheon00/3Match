using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates
{
    public class GameManagerResolve3MatchState : GameManagerState
    {
        private MatchedTilesList _matchedTilesList;
        private TileBoardManager _tileBoardManager;

        public GameManagerResolve3MatchState()
        {
            _tileBoardManager = GameManager.TileBoardManager;
        }

        public override void Handle()
        {
            // in resolve 3match state, find all 3 match tiles and pop them.
            // and insert new tiles into tileboard.
            // after that, change state to IdleState.

            FindAllMatchedTiles();
            PopAllMatchedTilesAndPlayEffect();
            InsertNewTiles();

            GameManager.ChangeState(new GameManagerIdleState());
        }

        private void FindAllMatchedTiles()
        {
            _tileBoardManager.FindAll3MatchTiles();
            _matchedTilesList = _tileBoardManager.GetMatchedTilesList();
        }

        private void PopAllMatchedTilesAndPlayEffect()
        {
            _tileBoardManager.PopAllMatchedTiles();
        }

        private void InsertNewTiles() { }
    }
}
