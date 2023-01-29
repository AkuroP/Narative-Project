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

        public float yPlusValue = 0.025f;
        

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.CompareTag("Player"))
            {
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
            player.transform.position = tpPoint.position;
            confiner.m_BoundingShape2D = nextConfiner;
            player.transform.localScale = newSize;
            Cinemachine.CinemachineVirtualCamera vcam = player.confiner.GetComponent<Cinemachine.CinemachineVirtualCamera>();
            vcam.m_Lens.FieldOfView = newFOV * 2;
            //vcam.GetCinemachineComponent<Cinemachine.CinemachineVirtualCamera>() .m_TrackedObjectOffset.y = yPlusValue; //Set(vcam.GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset.x, yPlusValue,vcam.GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset.z);
            player.CanMove = true;
            yield return new WaitForSeconds(.45f);
        }

    }

}
