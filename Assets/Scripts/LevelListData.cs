namespace Picker3d
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "LevelListData", menuName = "Picker3d/LevelList")]
    public class LevelListData : ScriptableObject
    {
        [field: SerializeField] public LevelEntity[] Levels {get; private set;}
        LevelEntity _previousRandomLevel = null;

        public LevelEntity GetRandomLevel()
        {
            int rnd = 0;
            do
            {
                rnd = Random.Range(0, Levels.Length - 1);
                
            }while(_previousRandomLevel == Levels[rnd]);
            
            _previousRandomLevel = Levels[rnd];
            return Levels[rnd];
        }
    }
}