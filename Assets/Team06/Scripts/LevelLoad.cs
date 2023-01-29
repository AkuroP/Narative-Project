using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team06
{


    public class LevelLoad : MonoBehaviour
    {
        [HideInInspector] public bool isEnding;


        void Update()
        {
            if (isEnding)
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }
}
