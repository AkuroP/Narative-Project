using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Team06
{
    public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;

    [Header("Text")] 
    [SerializeField] private Text textName;
    [SerializeField] private Text textBox;

    [Header("Image")] 
    [SerializeField] private Image leftImage;
    [SerializeField] private GameObject leftImageGO;
    [Space]
    [SerializeField] private Image rightImage;
    [SerializeField] private GameObject rightImageGO;
    
    [Header("Buttons Choice")] 
    [SerializeField] private Button button01;
    [SerializeField] private TextMeshProUGUI buttonTxt01;
    [Space]
     
    [SerializeField] private Button button02;
    [SerializeField] private TextMeshProUGUI buttonTxt02;
    [Space]
     
    [SerializeField] private Button button03;
    [SerializeField] private TextMeshProUGUI buttonTxt03;
    [Space]
    
    
    [Header("Continue")]
    [SerializeField] private Button buttonContinue;

    [Header("disable interactable")]
    [SerializeField] private Color textDisableColor;
    [SerializeField] private Color buttonDisableColor;

    [Header("interactable")]
    [SerializeField] private Color textInteractableColor;
    [Space]
    
    [Header("List")] 
    private List<Button> buttons = new List<Button>();
    private List<TextMeshProUGUI> buttonsTxt = new List<TextMeshProUGUI>();
    
     private void Awake()
        {
            ShowDialogueUI(false);

            buttons.Add(button01);
            buttons.Add(button02);
            buttons.Add(button03);

            buttonsTxt.Add(buttonTxt01);
            buttonsTxt.Add(buttonTxt02);
            buttonsTxt.Add(buttonTxt03);
        }

        public void ShowDialogueUI(bool show)
        {
            dialogueUI.SetActive(show);
        }

        public void SetText(string text)
        {
            textBox.text = text;
        }

        public void SetName(string text)
        {
            textName.text = text;
        }

        public void SetImage(Sprite leftImage, Sprite rightImage)
        {
            if (leftImage != null)
                this.leftImage.sprite = leftImage;

            if (rightImage != null)
                this.rightImage.sprite = rightImage;
        }

        public void HideButtons()
        {
            buttons.ForEach(button => button.gameObject.SetActive(false));
            buttonContinue.gameObject.SetActive(false);
        }

        public void SetButtons(List<DialogueButtonContainer> dialogueButtonContainers)
        {
            HideButtons();
        
            for (int i = 0; i < dialogueButtonContainers.Count; i++)
            {
                buttons[i].onClick = new Button.ButtonClickedEvent();
                buttons[i].interactable = true;
                buttonsTxt[i].color = textInteractableColor;
        
                if (dialogueButtonContainers[i].conditionCheck || dialogueButtonContainers[i].choiceState == ChoicesStateType.GrayOut)
                {
                    buttonsTxt[i].text = $"{i + 1}: " + dialogueButtonContainers[i].text;
                    buttons[i].gameObject.SetActive(true);
        
                    if (!dialogueButtonContainers[i].conditionCheck)
                    {
                        buttons[i].interactable = false;
                        buttonsTxt[i].color = textDisableColor;
                        var colors = buttons[i].colors;
                        colors.disabledColor = buttonDisableColor;
                        buttons[i].colors = colors;
                    }
                    else
                    {
                        buttons[i].onClick.AddListener(dialogueButtonContainers[i].unityAction);
                    }
                }
            }
        }

        public void SetContinue(UnityAction unityAction)
        {
            print("do it");
            buttonContinue.onClick = new Button.ButtonClickedEvent();
            buttonContinue.onClick.AddListener(unityAction);
            buttonContinue.gameObject.SetActive(true);
        }
    }
}
