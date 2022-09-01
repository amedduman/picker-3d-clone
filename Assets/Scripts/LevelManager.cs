namespace Picker3d
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Sirenix.OdinInspector;

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] LevelListData _levelListSo;
        [SerializeField][Range(2, 15)] int _levelCountToLoadAtSameTime = 3;
        [SerializeField] bool _isDebug;
        [ShowIf(nameof(_isDebug))] [SerializeField] int _levelToLoadInDebugMode = 1; 

        int _activeLevelIndex;
        int _passedLevelCount;
        string _levelIndexKey = "levelIndex";
        string _passedLevelCountKey = "passedLevelCount";
        List<LevelEntity> _loadedLevels = new List<LevelEntity>();
        int unloadedLevelDestroyDelay = 3;

        void OnEnable()
        {
            GameManager.Instance.OnTryAgainButtonPressed += Restart;
            GameManager.Instance.OnNextLevelButtonPressed += LevelPassed;
        }

        void OnValidate()
        { 
            _levelToLoadInDebugMode = Mathf.Clamp(_levelToLoadInDebugMode, 1, _levelListSo.Levels.Count);
        }

        void Start()
        {
#if !UNITY_EDITOR
            _isDebug = false;
#endif
            if (_isDebug)
            {
                _activeLevelIndex = _levelToLoadInDebugMode - 1;
                _passedLevelCount = _activeLevelIndex;
                PlayerPrefs.SetInt(_levelIndexKey, _activeLevelIndex);
                PlayerPrefs.SetInt(_passedLevelCountKey, _passedLevelCount);
            }
            else
            {
                _activeLevelIndex = PlayerPrefs.GetInt(_levelIndexKey, 0);
                _passedLevelCount = PlayerPrefs.GetInt(_passedLevelCountKey, 0);
            }

            int levelIndex = _activeLevelIndex;
            LevelEntity previousLevel = null;
            LevelEntity levelPrefabToLoad = null;

            for (int i = 0; i < _levelCountToLoadAtSameTime; i++)
            { 
                levelPrefabToLoad = _levelListSo.Levels[levelIndex % _levelListSo.Levels.Count];
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

        [Button]
        void ResetLevelData()
        {
            PlayerPrefs.SetInt(_levelIndexKey, 0);
            PlayerPrefs.SetInt(_passedLevelCountKey, 0);
            Debug.Log("level data reset");
        }

        void Restart()
        {
            SceneManager.LoadScene(0);
        }

        void LevelPassed()
        {
            _passedLevelCount++;

            // unload passed level
            LevelEntity unloadedLevel = _loadedLevels[0];
            _loadedLevels.RemoveAt(0);
            Destroy(unloadedLevel.gameObject, unloadedLevelDestroyDelay);

            // get next level
            LevelEntity levelPrefabToLoad = null;
            if (_passedLevelCount <= _levelListSo.Levels.Count - 1) 
            {
                levelPrefabToLoad = _levelListSo.Levels[_activeLevelIndex];
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

            for (int i = 0; i < _levelListSo.Levels.Count; i++)
            {
                if(_loadedLevels[0].Key ==  _levelListSo.Levels[i].Key)
                {
                    _activeLevelIndex = i;
                }
                
            }

            PlayerPrefs.SetInt(_levelIndexKey, _activeLevelIndex);
            PlayerPrefs.SetInt(_passedLevelCountKey, _passedLevelCount);

        }
    }

}