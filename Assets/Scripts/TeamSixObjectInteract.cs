using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSixObjectInteract : MonoBehaviour
{
    public GameObject interactPopup;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            interactPopup.SetActive(true);
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
    }
}
