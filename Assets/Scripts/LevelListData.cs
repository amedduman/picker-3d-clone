namespace Picker3d
{
    using System.Collections.Generic;
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "LevelListData", menuName = "Picker3d/LevelList")]
    public class LevelListData : ScriptableObject
    {
        [field: SerializeField] public List<LevelEntity> Levels {get; private set;} = new List<LevelEntity>();
        LevelEntity _previousRandomLevel = null;

        public LevelEntity GetRandomLevel()
        {
            int rnd = 0;
            do
            {
                rnd = Random.Range(0, Levels.Count - 1);
                
            }while(_previousRandomLevel == Levels[rnd]);
            
            _previousRandomLevel = Levels[rnd];
            return Levels[rnd];
        }
    }
}