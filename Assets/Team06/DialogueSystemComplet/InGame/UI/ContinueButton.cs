using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Team06
{
    public class ContinueButton : MonoBehaviour
    {
        [Header("Button")] [SerializeField] private Button continueButton;

        [Header("KeyCode")] [SerializeField] private KeyCode continueKey01 = KeyCode.Space;

        void Update()
        {
            if (Input.GetKeyDown(continueKey01) && continueButton.gameObject.activeSelf)
            {
                Continue();
            }
        }


        public void Continue()
        {
            continueButton.onClick.Invoke();
        }
    }
}
