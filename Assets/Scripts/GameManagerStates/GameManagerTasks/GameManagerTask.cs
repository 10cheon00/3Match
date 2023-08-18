using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates.GameManagerTasks
{
    public abstract class GameManagerTask 
    {
        private GameManagerState _gameManagerState;
        public GameManagerTask(GameManagerState gameManagerState) 
        {
            _gameManagerState = gameManagerState;
        }
        
        public abstract void RunTask();
        public void FinishTask()
        {
            _gameManagerState.FinishTask();
        }
    }
}
