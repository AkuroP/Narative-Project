using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueButtonContainer
{
        public UnityAction unityAction { get; set; }
        public string text { get; set; }
        public bool conditionCheck { get; set; }
        public ChoicesStateType choiceState { get; set; }
}
