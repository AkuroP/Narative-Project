using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team06
{
    public class ObjectInteract : MonoBehaviour
    {
        public GameObject interactPopup;
        [SerializeField]private DialogueTalk dialogueTalk;


        private void Start()
        {
            this.GetComponent<DialogueTalk>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.CompareTag("Player"))
            {
                interactPopup.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                interactPopup.SetActive(false);
            }
        }


        public void Interact()
        {
            Debug.Log(("INTERACTION"));
            dialogueTalk.StartDialogue();
            this.gameObject.SetActive(false);
        }
    }
}
