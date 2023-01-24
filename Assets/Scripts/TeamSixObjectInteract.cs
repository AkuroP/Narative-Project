using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSixObjectInteract : MonoBehaviour
{
    public GameObject interactPopup;
    public SpriteRenderer renderer;
    
    public Sprite switchSprite;
    public Sprite baseSprite;
    public Sprite actualSprite;


    private void Start()
    {
        actualSprite = baseSprite;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            interactPopup.SetActive(true);

            renderer.sprite = baseSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            interactPopup.SetActive(false);
        }
    }


    public void Interact()
    {
        Debug.Log(("INTERACTION"));

        //switch sprite (placeholder)
        SwitchSprite();

    }

    private void SwitchSprite()
    {
        SpriteRenderer playerSr = interactPopup.GetComponent<SpriteRenderer>();
        renderer.sprite = playerSr.sprite;
        playerSr.sprite = switchSprite;
    }
}
