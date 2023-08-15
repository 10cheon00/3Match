using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Assets.Scripts.GameManagerStates
{
    public abstract class GameManagerState
    {
        private static GameManager _gameManager;
        protected GameManager GameManager
        {
            get { return _gameManager; }
        }

        public GameManagerState()
        {
            Start();
        }

        protected virtual void Start() { }

        public abstract void Handle();

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
