using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

using Assets.Scripts.GameManagerStates.GameManagerTasks;

namespace Assets.Scripts.GameManagerStates
{
    public abstract class GameManagerState
    {
        private static GameManager _gameManager;
        protected GameManager GameManager
        {
            get { return _gameManager; }
        }
        private Queue<GameManagerTask> _taskQueue;

        public GameManagerState() {
            _taskQueue = new();
        }

        public void Run() 
        {
            OnBeforeRunTask();
            if (IsTaskQueueNotEmpty())
            {
                if (IsNotCurrentTaskInitialized())
                {
                    InitializeCurrentTask();
                }
                RunCurrentTask();
            }
            else
            {
                OnFinishAllTask();
            }
            OnAfterRunTask();
        }

        public virtual void OnBeforeRunTask() { }

        private bool IsNotCurrentTaskInitialized()
        {
            return _taskQueue.Peek().IsInitialized == false;
        }

        private void InitializeCurrentTask()
        {
            GameManagerTask task = _taskQueue.Peek();
            task.InitializeTask();
            task.IsInitialized = true;
        }

        public virtual void RunCurrentTask()
        {
            GameManagerTask task = _taskQueue.Peek();
            task?.RunTask();
        }

        public abstract void OnFinishAllTask();

        public virtual void OnAfterRunTask() { }

        public void AddTask(GameManagerTask gameManagerTask)
        {
            _taskQueue.Enqueue(gameManagerTask);
        }

        private bool IsTaskQueueNotEmpty()
        {
            return _taskQueue.Count > 0;
        }

        public void FinishTask()
        {
            _taskQueue.Dequeue();
        }

        public void ChangeState(GameManagerState gameManagerState)
        {
            _gameManager.ChangeState(gameManagerState);
        }

        public static void SetGameManager(GameManager gameManager)
        {
            _gameManager = gameManager;
        }
    }
}
