namespace Picker3d
{
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Sirenix.OdinInspector;
    using UnityEditor;

    public class LevelEditorManager : MonoBehaviour
    {
        [InfoBox("There should be a Assets/Levels folder for this Level Editor function right.", InfoMessageType.Info)]
        [SerializeField] LevelEditorModes _mode = LevelEditorModes.Generate;

        [ShowIf(nameof(_isGenerating))] [FoldoutGroup("Required")] [SerializeField]
        LevelListData _levelList;
        [FoldoutGroup("Required")] [SerializeField]
        GameObject[] _collectableGroups;

        [ValueDropdown(nameof(GetAllLevels), IsUniqueList = true)]
        [HideIf(nameof(_isGenerating))] [SerializeField]
         GameObject _levelPrefab;

        [ValueDropdown(nameof(GetAllLevels), IsUniqueList = true)]
        [ShowIf(nameof(_isGenerating))] [SerializeField]
        GameObject _levelPrefabToGenerateFrom;

        [ValueDropdown(nameof(GetAllCollectableGroups), IsUniqueList = true)] [SerializeField]
        GameObject _selectedCollectableGroup;

        bool _isGenerating;
        GameObject _loadedLevel;
        GameObject _generatedLevel;

        IEnumerable GetAllLevels()
        {
            var root = "Assets/Levels/";

            return UnityEditor.AssetDatabase.GetAllAssetPaths()
                .Where(x => x.StartsWith(root))
                .Select(x => x.Substring(root.Length))
                .Select(x => new ValueDropdownItem(x, UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(root + x)));
        }

        IEnumerable GetAllCollectableGroups()
        {
            List<GameObject> collectableGroups = new List<GameObject>();

            var level = GetLevel();
            if(level == null) return null;

            var parent = level.CollectablesParent;

            for (int i = 0; i < parent.childCount; i++)
            {
                collectableGroups.Add(parent.GetChild(i).gameObject);
            }

            return collectableGroups;
        }

        enum LevelEditorModes
        {
            Generate,
            Iterate
        }

        void OnValidate()
        {
            _isGenerating = _mode == LevelEditorModes.Generate ? true : false;
        }

        [PropertySpace(10)]
        [HideIf(nameof(_isGenerating))]
        [Button]
        void LoadLevel()
        {
            RemoveExistingLevels();

            _loadedLevel = PrefabUtility.InstantiatePrefab(_levelPrefab) as GameObject;
        }

        [PropertySpace(10)]
        [ShowIf(nameof(_isGenerating))]
        [Button]
        void GenerateLevel()
        {
            RemoveExistingLevels();

            _generatedLevel = PrefabUtility.InstantiatePrefab(_levelPrefabToGenerateFrom) as GameObject;
        }

        [Button]
        void RemoveLevelWithoutSaving()
        {
            RemoveExistingLevels();
        }

        [PropertySpace(10)]
        [Button]
        void SetLevelLength(float length)
        {
            var level = GetLevel();
            if (level == null) return;
            if(length < 0) length = 10;
            var roadScale = level.Road.transform.localScale;
            level.Road.transform.localScale = new Vector3(roadScale.x, roadScale.y, length); 

            Vector3 lvlEndPos = level.LevelEndPoolObject.position;
            lvlEndPos.z = level.Road.transform.position.z + length;
            level.LevelEndPoolObject.position = lvlEndPos;
        }    

        [PropertySpace(5)]
        [Button]
        void SetRequiredCollectables(int count)
        {
            var level = GetLevel();
            if (level == null) return;
            if(count <= 0) count = 1;
            level.LevelEndPoolObject.GetComponentInChildren<LevelEndPool>().NecessaryCollectableCount = count; 
        }

        [PropertySpace(5)]
        [Button]
        void AddCollectable(int collectableIndex)
        {
            var level = GetLevel();
            if(level  == null) return;
            collectableIndex = Mathf.Clamp(collectableIndex, 0, _collectableGroups.Length - 1);
            var col = PrefabUtility.InstantiatePrefab(_collectableGroups[collectableIndex],level.CollectablesParent);
        } 

        [PropertySpace(5)]
        [Button]
        void ChangeSelectedCollectableGroupRotPos(Vector3 pos, Vector3 rot)
        {
            _selectedCollectableGroup.transform.position = pos;
            _selectedCollectableGroup.transform.rotation = Quaternion.Euler(rot);
        }

        [PropertySpace(5)]
        [Button]
        void RemoveSelectedCollectableGroup()
        {
            DestroyImmediate(_selectedCollectableGroup.gameObject);
            _selectedCollectableGroup = null;
        }

        LevelEntity GetLevel()
        {
            var level = _isGenerating == true ? _generatedLevel : _loadedLevel;
            return level.GetComponent<LevelEntity>();
        }

        [PropertySpace(10)]
        [ShowIf(nameof(_isGenerating))]
        [Button]
        void SaveNewLevel()
        {
            bool levelGenerated = false;

            string localPath = "Assets/Levels/" + _generatedLevel.name + ".prefab";

            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            PrefabUtility.SaveAsPrefabAsset(_generatedLevel, localPath, out levelGenerated);

            if(levelGenerated)
            {
                Debug.Log("level generated!");

                GameObject generatedLevelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(localPath);
                _levelList.Levels.Add(generatedLevelPrefab.GetComponent<LevelEntity>());
                generatedLevelPrefab.GetComponent<LevelEntity>().GenerateKey();
            } 
            else 
            {
                Debug.LogWarning("A problem occurred while trying to generate new level");
            }

            RemoveExistingLevels();
        }

        

        void RemoveExistingLevels()
        {
            var levels = FindObjectsOfType<LevelEntity>(); 
            if (levels.Length > 0)
            {
                for (int i = levels.Length - 1; i >= 0; i--)
                {
                    DestroyImmediate(levels[i].gameObject); 
                }
            }
        }

        [PropertySpace(10)]
        [HideIf(nameof(_isGenerating))]
        [Button]
        void ApplyChangesToPrefab()
        {
            PrefabUtility.ApplyPrefabInstance(_loadedLevel, InteractionMode.UserAction);
        }
    }
}