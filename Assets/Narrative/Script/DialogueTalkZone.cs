using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTalkZone : MonoBehaviour
{
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private KeyCode talkKey = KeyCode.E;
    [SerializeField] private Text keyInputText;

    private void Awake()
    {
        speechBubble.SetActive(false);
        keyInputText.text = talkKey.ToString();
    }

    void Update()
    {
        if(Input.GetKeyDown(talkKey) && speechBubble.activeSelf)
        {
            // TODO: Start Dialogue.
            Debug.Log("Start Dialouge");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            speechBubble.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            speechBubble.SetActive(false);
        }
    }
}
