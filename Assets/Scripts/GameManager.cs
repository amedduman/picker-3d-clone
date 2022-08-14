namespace Picker3d
{
    using System;
    using UnityEngine;

    public class GameManager : Singleton<GameManager>
    {
        public event Action OnGameStart;
        public event Action OnLevelPassed;
        public event Action OnLevelFailed;
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
    }

}