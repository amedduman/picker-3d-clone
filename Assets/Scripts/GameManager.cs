namespace Picker3d
{
    using System;
    using UnityEngine;

    public class GameManager : Singleton<GameManager>
    {
        public event Action OnGameStart;
        public event Action OnLevelPassed;
        public event Action OnLevelFailed;
        public event Action OnTryAgainButtonPressed;
        public event Action OnNextLevelButtonPressed;
        public event Action OnNextLevelReady;
        public event Action<int> OnPassedLevelCountChanged;
        bool _hasGameStart;

        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(!_hasGameStart)
                {
                    _hasGameStart = true;
                    OnGameStart?.Invoke();
                }
            }
        }

        public void LevelPassed()
        {
            OnLevelPassed?.Invoke();
        }

        public void LevelFailed()
        {
            OnLevelFailed?.Invoke();
        }

        public void TryAgainButtonAction()
        {
            OnTryAgainButtonPressed?.Invoke();
        }

        public void NextLevelButtonAction()
        {
            OnNextLevelButtonPressed?.Invoke();
        }

        public void NextLevelReady()
        {
            OnNextLevelReady?.Invoke();
        }

        public void LevelCountChanged(int level)
        {
            OnPassedLevelCountChanged?.Invoke(level);
        }
    }

}