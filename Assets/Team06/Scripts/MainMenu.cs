using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject volumeBtn;
    public AudioMixer audioMixer;

    private void Update()
    {
        if(volumeBtn != null)
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
