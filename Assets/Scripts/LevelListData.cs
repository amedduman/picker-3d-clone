namespace Picker3d
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "LevelListData", menuName = "Picker3d/LevelList")]
    public class LevelListData : ScriptableObject
    {
        [field: SerializeField] public LevelEntity[] Levels {get; private set;}
    }
}