using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team06
{
    public class DialogueTalkingZoneAuto2D : MonoBehaviour
    {
        public DialogueTalk dialogueTalk;
        private CircleCollider2D _circleCollider2D;

        private void Awake()
        {
            _circleCollider2D = GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                print("stp");
                dialogueTalk.StartDialogue();
                _circleCollider2D.enabled = false;

            }
        }
    }
}
