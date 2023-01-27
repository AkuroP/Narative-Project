using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Team06
{
    public class DialogueTalkingZoneAuto2D : MonoBehaviour
    {
        public DialogueTalk dialogueTalk;
        private Collider2D thisCollider2D;
        
        

        private void Awake()
        {
            thisCollider2D = GetComponent<Collider2D>();
            
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                
                    dialogueTalk.StartDialogue();
                    thisCollider2D.enabled = false;


            }
        }
        
    }
}
