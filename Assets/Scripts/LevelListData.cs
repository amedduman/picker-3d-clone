namespace Picker3d
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "LevelListData", menuName = "Picker3d/LevelList")]
    public class LevelListData : ScriptableObject
    {
        [field: SerializeField] public LevelEntity[] Levels {get; private set;}

        public LevelEntity GetRandomLevel()
        {
            int rnd = Random.Range(0, Levels.Length - 1);
            return Levels[rnd];
        }
    }
}