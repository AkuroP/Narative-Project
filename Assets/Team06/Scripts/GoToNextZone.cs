using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team06
{
    public class GoToNextZone : MonoBehaviour
    {
        public Cinemachine.CinemachineConfiner confiner;
        public Animation transition;
        public Collider2D nextConfiner;
        public Transform tpPoint;

        public TeamSixPlayer player;

        public Vector3 newSize = new Vector3(.3f, .3f, .3f);
        public float newFOV = 25f;

        public Transform newPos;
        
        [Header("Ending")]
        public bool goEnding;
        public GameObject introTalk;
        public GameObject endTalk;
        

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.CompareTag("Player"))
            {
                if (goEnding)
                {
                    introTalk.SetActive(false);
                    endTalk.SetActive(true);
                }
                if(transition.isPlaying)transition.Stop();
                transition.gameObject.SetActive(true);
                transition.Play();
                StartCoroutine(ToNextPoint());

               
            }
        }
        

        private IEnumerator ToNextPoint()
        {
            if(player.moveCoroutine != null)StopCoroutine(player.moveCoroutine);
            player.PlayerStopMoving();
            player.CanMove = false;
            yield return new WaitForSeconds(.45f);
            confiner.m_BoundingShape2D = nextConfiner;
            player.transform.localScale = newSize;
            Cinemachine.CinemachineVirtualCamera vcam = player.confiner.GetComponent<Cinemachine.CinemachineVirtualCamera>();
            vcam.m_Lens.FieldOfView = newFOV * 2;
            if(newPos != null)vcam.transform.position = newPos.position;
            player.CanMove = true;
            
            player.transform.position = tpPoint.position;
            yield return new WaitForSeconds(.45f);
            player.StopAllCoroutines();
        }

    }

}
