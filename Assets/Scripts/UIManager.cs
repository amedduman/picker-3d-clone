namespace Picker3d
{
    using UnityEngine;

    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject TapToPlayScreen;
        [SerializeField] GameObject LevelPassedScreen;
        [SerializeField] GameObject LevelFailedScreen;

        void OnEnable()
        {
            GameManager.Instance.OnGameStart += HandleGameStart;
            GameManager.Instance.OnLevelPassed += HandleLevelPassed;
            GameManager.Instance.OnLevelFailed += HandleLevelFailed;
        }

        void OnDisable()
        {
            GameManager.Instance.OnGameStart -= HandleGameStart;
            GameManager.Instance.OnLevelPassed -= HandleLevelPassed;
            GameManager.Instance.OnLevelFailed -= HandleLevelFailed;
        }

        void HandleGameStart()
        {
            TapToPlayScreen.SetActive(false);
        }

        void HandleLevelPassed()
        {
            LevelPassedScreen.SetActive(true);
        }

        void HandleLevelFailed()
        {
            LevelFailedScreen.SetActive(true);
        }
    }

}