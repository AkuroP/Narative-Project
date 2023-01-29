using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team06
{


    public class LevelLoad : MonoBehaviour
    {
        public string name;
        public void OnTriggerEnter2D(Collider2D other)
        {
            SceneManager.LoadScene(name);
        }
    }
}
