using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates
{
    public class GameManagerResolve3MatchState : GameManagerState
    {
        private MatchedTileList _matchedTileResultList;
        private TileBoardManager _tileBoardManager;

        public GameManagerResolve3MatchState()
        {
            _tileBoardManager = GameManager.TileBoardManager;
            FindAllMatchedTile();
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

        private void FindAllMatchedTile()
        {
            _tileBoardManager.FindAllMatchedTile();
        }

        private void PopAllMatchedTilesAndPlayEffect()
        {
            // TODO
            // fix algorithm.
            // current algorithm find duplicated matched tiles.

            _matchedTileResultList = _tileBoardManager.GetMatchedTileResultList();
            foreach(Tile tile in _matchedTileResultList)
            {
                tile.PlayPopEffect();
            }
            _tileBoardManager.PopAllMatchedTiles();
        }

        private void InsertNewTilesAndPlayEffect() { }
    }
}
