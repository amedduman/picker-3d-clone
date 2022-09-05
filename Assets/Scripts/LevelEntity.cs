namespace Picker3d
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Sirenix.OdinInspector;

    public class LevelEntity : MonoBehaviour
    {
        [SerializeField] LevelKeyGenerator _keyGenerator;
        public Transform LevelEnd;
        [ReadOnly] public int Key = -1;


        [Button]
        void GenerateKey()
        {
            Key = _keyGenerator.GenerateUniqueKey(this);
        }

        void OnValidate()
        {
            if(Key == -1)
            {
                GenerateKey();
            }            
        }

    }


}