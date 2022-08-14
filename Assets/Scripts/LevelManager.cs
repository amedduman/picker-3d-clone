namespace Picker3d
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LevelManager : MonoBehaviour
    {
        void OnEnable()
        {
            GameManager.Instance.OnTryAgainButtonPressed += Restart;
        }

        void OnDisable()
        {
            GameManager.Instance.OnTryAgainButtonPressed -= Restart;
        }

        void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }

}