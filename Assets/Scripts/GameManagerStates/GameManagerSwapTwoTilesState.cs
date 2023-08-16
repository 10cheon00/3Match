using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates
{
    public class GameManagerSwapTwoTilesState : GameManagerState
    {
        private TilePair _tilePair;

        public GameManagerSwapTwoTilesState(TilePair tilePair) : base()
        {
            // tilePair에 아무 값도 들어있지 않음
            _tilePair = tilePair;
            SwapTwoTilesAndPlayEffect();
        }

        private void SwapTwoTilesAndPlayEffect()
        {
            Debug.Log($"{_tilePair.tileA.Color} {_tilePair.tileB.Color}");
            GameManager.TileBoardManager.SwapTwoTiles(_tilePair);

            Vector3 midPoint = Vector3.Lerp(
                _tilePair.tileA.transform.position,
                _tilePair.tileB.transform.position,
                0.5f
            );
            _tilePair.tileA.PlayRotationEffect(midPoint);
            _tilePair.tileB.PlayRotationEffect(midPoint);
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
            return _tilePair.tileA.IsReadyToPlayTileEffect() && _tilePair.tileB.IsReadyToPlayTileEffect();
        }
    }
}
