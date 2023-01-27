using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team06
{
    public class LanguageController : MonoBehaviour
    {
        [SerializeField] private LanguageType languageType;

        public static LanguageController Instance { get; private set; }

        public LanguageType LanguageType
        {
            get => languageType;
            set => languageType = value;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
