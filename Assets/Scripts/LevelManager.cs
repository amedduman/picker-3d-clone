namespace Picker3d
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] LevelListData _levelListSo;
        [SerializeField] [Range(3, 15)] int _levelCountToLoadAtSameTime = 3;
        [SerializeField] bool _isDebug;
        [SerializeField] [Range(0, 100)] int _activeLevelIndex; 

        string _levelIndexKey = "levelIndex";

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
            if(!_isDebug) _activeLevelIndex = PlayerPrefs.GetInt(_levelIndexKey, 0);

            int levelIndex = _activeLevelIndex;
            LevelEntity previousLevel = null;
            LevelEntity levelPrefabToLoad = null;

            for (int i = 0; i < _levelCountToLoadAtSameTime; i++)
            {
                levelPrefabToLoad = _levelListSo.Levels[levelIndex % _levelListSo.Levels.Length];
                Vector3 levelSpawnPoint = i == 0 ? Vector3.zero : previousLevel.LevelEnd.position;
                LevelEntity instantiatedLevel =  Instantiate(levelPrefabToLoad, levelSpawnPoint, Quaternion.identity);

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

        }
    }

}