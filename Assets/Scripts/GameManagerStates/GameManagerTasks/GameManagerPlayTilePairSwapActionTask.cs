using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates.GameManagerTasks
{
    public class GameManagerPlayTilePairSwapActionTask : GameManagerTask
    {
        private TilePair _tilePair;

        public GameManagerPlayTilePairSwapActionTask(GameManagerState gameManagerState, TilePair tilePair)
            : base(gameManagerState) 
        {
            _tilePair = tilePair;
        }

        public override void InitializeTask()
        {
            Vector3 midPoint = Vector3.Lerp(
                _tilePair.tileA.transform.position,
                _tilePair.tileB.transform.position,
                0.5f
            );

            _tilePair.tileA.PlayRotationAction(midPoint);
            _tilePair.tileB.PlayRotationAction(midPoint);
        }

        public override void RunTask()
        {
            if (_tilePair.tileA.IsReadyToPlayTileAction() && _tilePair.tileB.IsReadyToPlayTileAction())
            {
                FinishTask();
            }
        }
    }
}
