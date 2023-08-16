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
            FindAllMatchedTiles();
            PopAllMatchedTilesAndPlayEffect();
            InsertNewTilesAndPlayEffect();
        }

        public override void Handle()
        {
            // in resolve 3match state, find all 3 match tiles and pop them.
            // and insert new tiles into tileboard.
            // after that, change state to IdleState.

            // TODO
            // if all pop tile effect ends, change state to idle.
            // GameManager.ChangeState(new GameManagerIdleState());
        }

        private void FindAllMatchedTiles()
        {
            _tileBoardManager.FindAll3MatchTiles();
        }

        private void PopAllMatchedTilesAndPlayEffect()
        {
            // TODO
            // fix algorithm.
            // current algorithm find duplicated matched tiles.

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
