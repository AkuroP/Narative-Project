using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSixPlayer : MonoBehaviour
{
    [SerializeField]private float playerSpeed;
    [SerializeField]private bool playerIsMoving;
    private CharacterController playerController;

    private Coroutine moveCoroutine;
    
    // Start is called before the first frame update
    private void Start()
    {
        playerController = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    #region Movement
    //Start Moving player, direction : 1 = Right ; 2 = Left.
    public void PlayerStartMoving(int dir)
    {
        //reset movement if already moving
        if(playerIsMoving)StopCoroutine(moveCoroutine);

        //set settings for moving
        playerIsMoving = true;
        Vector2 PlayerDir = new Vector2(dir, 0f);
        moveCoroutine = StartCoroutine(Move(PlayerDir));
    }

    //Stop Moving player
    public void PlayerStopMoving()
    {
        //reset settings for moving
        playerIsMoving = false;
        StopCoroutine(moveCoroutine);
    }

    private IEnumerator Move(Vector2 dir)
    {
        
        while(playerIsMoving)
        {
            playerController.Move(dir * playerSpeed * Time.deltaTime);
            yield return null;   
        }
    }
    #endregion
}
