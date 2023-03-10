using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team06
{
    public class TeamSixPlayer : MonoBehaviour
    {
        [SerializeField] private float playerSpeed;
        [SerializeField] private bool playerIsMoving;

        public bool PlayerIsMoving
        {
            get { return this.playerController; }
            set { playerIsMoving = value; }
        }


        private CharacterController playerController;

        public CharacterController PlayerController
        {
            get { return this.playerController; }
            set { playerController = value; }
        }

        public Coroutine moveCoroutine;

        [SerializeField] private bool canMove = true;

        public bool CanMove
        {
            get { return canMove; }
            set { this.canMove = value; }
        }

        public Cinemachine.CinemachineConfiner confiner;
        public Collider2D confinerColl;
        public Animator playerAnim;
        // Start is called before the first frame update
        private void Start()
        {
            playerController = this.GetComponent<CharacterController>();
            canMove = true;
            confiner.m_BoundingShape2D = confinerColl;
        }

        // Update is called once per frame
        private void Update()
        {

        }

        #region Movement

        //Start Moving player, direction : 1 = Right ; 2 = Left.
        public void PlayerStartMoving(int dir)
        {
            if (!canMove) return;
            //reset movement if already moving
            if (playerIsMoving) StopCoroutine(moveCoroutine);

            //set settings for moving
            playerIsMoving = true;
            playerAnim.SetBool("IsMoving", true);
            Vector2 PlayerDir = new Vector2(dir, 0f);
            FlipPlayer(dir);
            moveCoroutine = StartCoroutine(Move(PlayerDir));
        }

        public void FlipPlayer(float dir)
        {
            if (dir > 0) this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
            else if (dir < 0)
            {
                float scaleX = this.transform.localScale.x;
                if(scaleX > 0) scaleX = -scaleX;
                this.transform.localScale = new Vector3(scaleX, this.transform.localScale.y, this.transform.localScale.z);
            }

        }

        //Stop Moving player
        public void PlayerStopMoving()
        {
            if (!canMove) return;
            //reset settings for moving
            playerIsMoving = false;
            playerAnim.SetBool("IsMoving", false);

            if(moveCoroutine != null)StopCoroutine(moveCoroutine);
        }

        private IEnumerator Move(Vector2 dir)
        {

            while (playerIsMoving)
            {
                playerController.Move(dir * playerSpeed * Time.deltaTime);
                yield return null;
            }
        }

        public void SetMovement(bool movable)
        {
            this.canMove = movable;
        }

        #endregion
    }
}
