namespace Picker3d
{
    using System.Linq;
    using System.Collections;
    using UnityEngine;
    using Sirenix.OdinInspector;
    using UnityEditor;

    public class LevelEditorManager : MonoBehaviour
    {
        [SerializeField] LevelEditorModes _mode = LevelEditorModes.Generate;

        [ValueDropdown(nameof(GetAllLevels), IsUniqueList = true)]
        [HideIf(nameof(_isGenerating))]
        [SerializeField] GameObject _levelPrefab;

        bool _isGenerating;
        GameObject _loadedLevel;

        private static IEnumerable GetAllLevels()
        {
            var root = "Assets/Levels/";

            return UnityEditor.AssetDatabase.GetAllAssetPaths()
                .Where(x => x.StartsWith(root))
                .Select(x => x.Substring(root.Length))
                .Select(x => new ValueDropdownItem(x, UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(root + x)));
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
            // remove existing levels
            var levels = FindObjectsOfType<LevelEntity>(); 
            if (levels.Length > 0)
            {
                for (int i = levels.Length - 1; i >= 0; i--)
                {
                    DestroyImmediate(levels[i].gameObject); 
                }
            }

            _loadedLevel = PrefabUtility.InstantiatePrefab(_levelPrefab) as GameObject;
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