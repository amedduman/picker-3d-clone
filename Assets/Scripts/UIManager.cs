namespace Picker3d
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject TapToPlayScreen;

        void OnEnable()
        {
            GameManager.Instance.OnGameStart += HandleGameStart;
        }

        void OnDisable()
        {
            GameManager.Instance.OnGameStart -= HandleGameStart;
        }

        void HandleGameStart()
        {
            TapToPlayScreen.SetActive(false);
        }
    }

}