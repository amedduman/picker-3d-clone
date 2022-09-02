namespace Picker3d
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject TapToPlayScreen;
        [SerializeField] GameObject LevelPassedScreen;
        [SerializeField] GameObject LevelFailedScreen;
        [SerializeField] GameObject _inGameScreen;
        [SerializeField] TextMeshProUGUI _levelCount;

        void OnEnable()
        {
            GameManager.Instance.OnGameStart += HandleGameStart;
            GameManager.Instance.OnLevelPassed += HandleLevelPassed;
            GameManager.Instance.OnLevelFailed += HandleLevelFailed;
            GameManager.Instance.OnPassedLevelCountChanged += ChangeLevelCount;

            LevelFailedScreen.GetComponentInChildren<Button>().onClick.AddListener(TryAgainButtonAction);
            LevelPassedScreen.GetComponentInChildren<Button>().onClick.AddListener(NextLevelButtonAction);
        }

        void OnDisable()
        {
            GameManager.Instance.OnGameStart -= HandleGameStart;
            GameManager.Instance.OnLevelPassed -= HandleLevelPassed;
            GameManager.Instance.OnLevelFailed -= HandleLevelFailed;
            GameManager.Instance.OnPassedLevelCountChanged -= ChangeLevelCount;

            LevelFailedScreen.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            LevelPassedScreen.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        }

        void HandleGameStart()
        {
            TapToPlayScreen.SetActive(false);
            _inGameScreen.SetActive(true);
        }

        void HandleLevelPassed()
        {
            LevelPassedScreen.SetActive(true);
        }

        void HandleLevelFailed()
        {
            LevelFailedScreen.SetActive(true);
        }

        void TryAgainButtonAction()
        {
            GameManager.Instance.TryAgainButtonAction();
        }

        void NextLevelButtonAction()
        {
            LevelPassedScreen.gameObject.SetActive(false);
            GameManager.Instance.NextLevelButtonAction();
        }

        void ChangeLevelCount(int level)
        {
            _levelCount.text = level.ToString();
        }
    }

}