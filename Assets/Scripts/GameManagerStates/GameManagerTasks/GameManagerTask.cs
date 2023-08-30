using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates.GameManagerTasks
{
    public abstract class GameManagerTask 
    {
        private GameManagerState _gameManagerState;
        private bool _isInitialized;
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = true; }
        }

        public GameManagerTask(GameManagerState gameManagerState) 
        {
            _gameManagerState = gameManagerState;
            _isInitialized = false;
        }

        public virtual void InitializeTask() { }
        
        public abstract void RunTask();
        public virtual void FinishTask()
        {
            _gameManagerState.FinishTask();
        }
    }
}
