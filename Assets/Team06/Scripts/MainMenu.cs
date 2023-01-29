using System.Collections;
using System.Collections.Generic;
using Team06;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Team06
{

    public class MainMenu : MonoBehaviour
    {
        public GameObject volumeBtn;
        public AudioMixer audioMixer;
        private bool _optionMenuActived;
        public GameObject optionMenuGO;

        private void Update()
        {
            if (volumeBtn != null)
            {
                if (EventSystem.current.currentSelectedGameObject == GameObject.FindGameObjectWithTag("SliderVolume"))
                {
                    volumeBtn.SetActive(true);
                }
                else
                {
                    volumeBtn.SetActive(false);
                }
            }
        }

        public void StartSceneByIndex(int p_index)
        {
            SceneManager.LoadScene(p_index);
        }

        public void StartSceneByName(string p_name)
        {
            SceneManager.LoadScene(p_name);
        }

        public void OptionUI()
        {
            if (!_optionMenuActived)
            {
                optionMenuGO.SetActive(true);
                _optionMenuActived = true;
            }
            else
            {
                optionMenuGO.SetActive(false);
                _optionMenuActived = false;
            }
        }

        public void SetFrench()
        {
            LanguageController.Instance.LanguageType = LanguageType.French;
        }

        public void SetEnglish()
        {
            LanguageController.Instance.LanguageType = LanguageType.English;
        }

        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("volume", volume);
            Debug.Log(volume);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
