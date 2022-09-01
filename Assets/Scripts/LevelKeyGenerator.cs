namespace Picker3d
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Sirenix.OdinInspector;

    [CreateAssetMenu(fileName = "LevelKeyGenerator", menuName = "Picker3d/LevelKeyGenerator")]
    public class LevelKeyGenerator : ScriptableObject
    {
        [ReadOnly] [SerializeField] List<int> keys = new List<int>();
        [ReadOnly] [SerializeField] List<KeyLevelData> KeyLevelPairs = new List<KeyLevelData>();

        public int GenerateUniqueKey(LevelEntity level)
        {
            int index = -1;
            for (int i = 0; i < KeyLevelPairs.Count; i++)
            {
                if(KeyLevelPairs[i].Level == level)
                {
                    index = i;
                }
                
            }

            if(index == -1)
            {
                KeyLevelPairs.Add(new KeyLevelData(level, 1));
                index = KeyLevelPairs.Count - 1;
            }

            do
            {
                KeyLevelPairs[index].LevelKey = Random.Range(1, 1000000);
            } while(keys.Contains(KeyLevelPairs[index].LevelKey));

            keys.Add(KeyLevelPairs[index].LevelKey);
            return KeyLevelPairs[index].LevelKey;
        }

        [System.Serializable]
        class KeyLevelData
        {
            public LevelEntity Level;
            public int LevelKey;

            public KeyLevelData(LevelEntity level, int key)
            {
                Level = level;
                LevelKey = key;
            }
        }
    }

}