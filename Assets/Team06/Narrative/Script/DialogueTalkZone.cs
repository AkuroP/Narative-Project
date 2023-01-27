using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Team06;

namespace Team06
{
    public class DialogueTalkZone : MonoBehaviour
    {
        [SerializeField] private GameObject speechBubble;
        [SerializeField] private KeyCode talkKey = KeyCode.E;
        [SerializeField] private Text keyInputText;
        private DialogueTalk _dialogueTalk;

        private void Awake()
        {
            _dialogueTalk = GetComponent<DialogueTalk>();
            speechBubble.SetActive(false);
            keyInputText.text = talkKey.ToString();
        }

        void Update()
        {
            if (Input.GetKeyDown(talkKey) && speechBubble.activeSelf)
            {
                // TODO: Start Dialogue.
                _dialogueTalk.StartDialogue();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                speechBubble.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                speechBubble.SetActive(false);
            }
        }
        
        
    }
}
