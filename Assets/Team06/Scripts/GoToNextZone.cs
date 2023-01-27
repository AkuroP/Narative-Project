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
        

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.CompareTag("Player"))
            {
                if(transition.isPlaying)transition.Stop();
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
            player.transform.position = tpPoint.position;
            confiner.m_BoundingShape2D = nextConfiner;
            player.transform.localScale = newSize;
            player.confiner.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = newFOV / 2;
            player.CanMove = true;
            yield return new WaitForSeconds(.45f);
        }

    }

}
