namespace Picker3d
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameManager : Singleton<GameManager>
    {
        public event Action OnGameStart;
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
    }

}