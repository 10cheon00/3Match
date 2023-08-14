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
            InsertNewTilesAndPlayEffect();

            // GameManager.ChangeState(new GameManagerIdleState());
        }

        private void FindAllMatchedTiles()
        {
            _tileBoardManager.FindAll3MatchTiles();
        }

        private void PopAllMatchedTilesAndPlayEffect()
        {
            _matchedTilesList = _tileBoardManager.GetMatchedTilesList();
            foreach(MatchedTiles matchedTiles in _matchedTilesList)
            {
                foreach(Tile tile in matchedTiles)
                {
                    tile.PlayPopEffect();
                }
            }
            _tileBoardManager.PopAllMatchedTiles();

        }

        private void InsertNewTilesAndPlayEffect() { }
    }
}
