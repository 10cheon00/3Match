using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates
{
    public class GameManagerSwapTwoTilesState : GameManagerState
    {
        private TilePair _tiles;
        private bool _isSwappingEffectFinished;

        public GameManagerSwapTwoTilesState(TilePair tiles)
        {
            _tiles = tiles;
            _isSwappingEffectFinished = false;

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
            _tiles.tileA.Start180DegreeRotation(midPoint);
            _tiles.tileB.Start180DegreeRotation(midPoint);
        }

        public override void Handle()
        {
            // in swapping state, swap two tiles and play swapping effect.
            // after end of swapping effect, change state to Resolve3Match.

            UpdateState();
            if (_isSwappingEffectFinished)
            {
                ChangeState(new GameManagerResolve3MatchState());
            }
        }

        private void UpdateState()
        {
            _isSwappingEffectFinished =
                _tiles.tileA.IsSwappingEffectFinished && _tiles.tileB.IsSwappingEffectFinished;
        }
    }
}
