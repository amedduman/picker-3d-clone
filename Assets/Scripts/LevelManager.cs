namespace Picker3d
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] LevelListData _levelListSo;
        [SerializeField][Range(2, 15)] int _levelCountToLoadAtSameTime = 3;
        [SerializeField] bool _isDebug;
        [SerializeField][Range(0, 100)] int _activeLevelIndex;

        string _levelIndexKey = "levelIndex";
        List<LevelEntity> _loadedLevels = new List<LevelEntity>();

        void OnEnable()
        {
            GameManager.Instance.OnTryAgainButtonPressed += Restart;
            GameManager.Instance.OnNextLevelButtonPressed += LevelPassed;
        }

        void Start()
        {
#if !UNITY_EDITOR
            _isDebug = false;
#endif
            if (!_isDebug) _activeLevelIndex = PlayerPrefs.GetInt(_levelIndexKey, 0);

            int levelIndex = _activeLevelIndex;
            LevelEntity previousLevel = null;
            LevelEntity levelPrefabToLoad = null;

            for (int i = 0; i < _levelCountToLoadAtSameTime; i++)
            {
                levelPrefabToLoad = _levelListSo.Levels[levelIndex % _levelListSo.Levels.Length];
                Vector3 levelSpawnPoint = i == 0 ? Vector3.zero : previousLevel.LevelEnd.position;
                LevelEntity instantiatedLevel = Instantiate(levelPrefabToLoad, levelSpawnPoint, Quaternion.identity);

                _loadedLevels.Add(instantiatedLevel);

                previousLevel = instantiatedLevel;
                levelIndex++;
            }
        }

        void OnDisable()
        {
            GameManager.Instance.OnTryAgainButtonPressed -= Restart;
            GameManager.Instance.OnNextLevelButtonPressed -= LevelPassed;
        }

        void Restart()
        {
            SceneManager.LoadScene(0);
        }

        void LevelPassed()
        {
            _activeLevelIndex++;
            if (_activeLevelIndex == 1) return;

            // unload passed level
            LevelEntity unloadedLevel = _loadedLevels[0];
            _loadedLevels.RemoveAt(0);
            Destroy(unloadedLevel.gameObject);

            // get next level
            int passedLevelIndex = _activeLevelIndex - 1;
            LevelEntity levelPrefabToLoad = null;
            if (passedLevelIndex < _levelListSo.Levels.Length - 1)
            {
                levelPrefabToLoad = _levelListSo.Levels[passedLevelIndex + 1];
            }
            // we run out levels, get a random level
            else
            {
                levelPrefabToLoad = _levelListSo.GetRandomLevel();
            }

            // load the level
            Vector3 levelSpawnPoint = _loadedLevels[_loadedLevels.Count - 1].LevelEnd.position;
            LevelEntity instantiatedLevel = Instantiate(levelPrefabToLoad, levelSpawnPoint, Quaternion.identity);
            _loadedLevels.Add(instantiatedLevel);
        }
    }

}