namespace Picker3d
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Sirenix.OdinInspector;

    public class LevelEntity : MonoBehaviour
    {
        public GameObject Road;

        [SerializeField] LevelKeyGenerator _keyGenerator;
        public Transform LevelEnd;
        public Transform LevelEndPoolObject;
        [ReadOnly] public int Key = -1;


        [Button]
        public void GenerateKey()
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