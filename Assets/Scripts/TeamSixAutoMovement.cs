using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team06
{
    public class TeamSixAutoMovement : MonoBehaviour
    {

        [System.Serializable]
        public struct MovingSettings
        {
            public float movingTime;
            public float movingSpeed;

            [Tooltip("x > 0 = Right | x < 0 = Left")]
            public Vector2 movingDir;

            [Tooltip("Le perso s'arrête en cinématique ?")]
            public bool pauseMoving;

            [Tooltip("temps d'arrêt")] public float pauseMovingTime;
        }

        [SerializeField] private MovingSettings[] movingSettings;

        [SerializeField] private int currentIndex;
        [SerializeField] private bool startMoving;
        [SerializeField] private TeamSixPlayer teamSixPlayer;

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            AutoMove();
        }

        private void AutoMove()
        {
            if (!startMoving) return;
            if (movingSettings[currentIndex].movingTime <= 0f)
            {
                if (movingSettings[currentIndex].pauseMoving)
                {
                    if (movingSettings[currentIndex].pauseMovingTime <= 0f)
                    {
                        ToNextIndex();
                    }
                    else
                    {
                        movingSettings[currentIndex].pauseMovingTime -= Time.deltaTime;
                    }
                }
                else ToNextIndex();
            }
            else
            {
                teamSixPlayer.PlayerController.Move(movingSettings[currentIndex].movingDir *
                                                    movingSettings[currentIndex].movingSpeed * Time.deltaTime);
                movingSettings[currentIndex].movingTime -= Time.deltaTime;
            }

        }

        private void ToNextIndex()
        {
            if (currentIndex >= movingSettings.Length - 1)
            {
                startMoving = false;
                teamSixPlayer.PlayerStopMoving();
                teamSixPlayer.CanMove = true;
                Destroy(this.gameObject);
            }
            else
            {
                movingSettings[currentIndex].movingDir = Vector2.zero;
                currentIndex += 1;
                teamSixPlayer.FlipPlayer(movingSettings[currentIndex].movingDir.x);
            }

            return;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                teamSixPlayer = collider.GetComponentInParent<TeamSixPlayer>();
                startMoving = true;
                teamSixPlayer.CanMove = false;
                this.GetComponent<Collider2D>().enabled = false;
                teamSixPlayer.FlipPlayer(movingSettings[currentIndex].movingDir.x);
                teamSixPlayer.PlayerIsMoving = false;
            }
        }
    }
}
