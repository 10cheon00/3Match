using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates
{
    public class GameManagerSwapTwoTilesState : GameManagerState
    {
        private TilePair _tiles;

        public GameManagerSwapTwoTilesState(TilePair tiles)
        {
            _tiles = tiles;
        }

        protected override void Start()
        {
            // swap two tile in tileboard.
            // play swapping tile effect.
            SwapTwoTilesAndPlayEffect();
        }

        private void SwapTwoTilesAndPlayEffect()
        {
            GameManager.TileBoardManager.SwapTwoTiles(_tiles);

            Vector3 midPoint = Vector3.Lerp(
                _tiles.tileA.transform.position,
                _tiles.tileB.transform.position,
                0.5f
            );
            _tiles.tileA.PlayRotationEffect(midPoint);
            _tiles.tileB.PlayRotationEffect(midPoint);
        }

        public override void Handle()
        {
            // in swapping state, swap two tiles and play swapping effect.
            // after end of swapping effect, change state to Resolve3Match.

            if (IsSwappingEffectFinished())
            {
                ChangeState(new GameManagerResolve3MatchState());
            }
        }

        private bool IsSwappingEffectFinished()
        {
            return _tiles.tileA.IsReadyToPlayTileEffect() && _tiles.tileB.IsReadyToPlayTileEffect();
        }
    }
}
