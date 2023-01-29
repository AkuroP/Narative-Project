using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Team06
{
    public class GameEvent_Exemple : GameEvents
    {
        private bool thought;
        private bool talkedAngelier;
        private bool talkedToClemence;
        public LevelLoad levelLoad;
        public Image textPanel;
        public Sprite modifyTextPanel;

        public bool Thought { get => thought; set => thought = value; }

        public override bool DialogueConditionEvents(string stringEvent,
            StringEventConditionType stringEventConditionType, float value = 0)
        {
            switch (stringEvent.ToLower())
            {
                case "money":
                    return useStringEventCondition.ConditionFloatCheck(FindObjectOfType<Player>().Money, value,
                        stringEventConditionType);
                case "health":
                    return useStringEventCondition.ConditionFloatCheck(FindObjectOfType<Player>().Health, value,
                        stringEventConditionType);
                case "didaskforfood":
                    return useStringEventCondition.ConditionBoolCheck(FindObjectOfType<Player>().DidWeTalk,
                        stringEventConditionType);
                case "thought":
                    return useStringEventCondition.ConditionBoolCheck(Thought, stringEventConditionType);
                case "TalkedToAngelier":
                    return useStringEventCondition.ConditionBoolCheck(talkedAngelier, stringEventConditionType);
                case "TalkedToClemence":
                    return useStringEventCondition.ConditionBoolCheck(talkedToClemence, stringEventConditionType);
                case "Ending":
                    return useStringEventCondition.ConditionBoolCheck(levelLoad.isEnding, stringEventConditionType);
                default:
                    Debug.LogWarning("No String Event was found");
                    return false;
            }
        }

        public override void DialogueModifierEvents(string stringEvent, StringEventModifierType stringEventModifierType, float value = 0)
        {
            switch (stringEvent.ToLower())
            {
                case "money":
                    FindObjectOfType<Player>()
                        .ModifyMoney((int) useStringEventModifier.ModifierFloatCheck(value, stringEventModifierType));
                    break;
                case "health":
                    FindObjectOfType<Player>()
                        .ModifyHealth((int) useStringEventModifier.ModifierFloatCheck(value, stringEventModifierType));
                    break;
                case "didaskforfood":
                    FindObjectOfType<Player>().DidWeTalk =
                        (stringEventModifierType == StringEventModifierType.SetTrue ? true : false);
                    FindObjectOfType<Player>().DidWeTalk =
                        useStringEventModifier.ModifierBoolCheck(stringEventModifierType);
                    break;
                case "thought":
                    Thought = (stringEventModifierType == StringEventModifierType.SetTrue ? true : false);
                    Thought = useStringEventModifier.ModifierBoolCheck(stringEventModifierType);
                    break;
                case "TalkedToAngelier":
                    talkedAngelier = (stringEventModifierType == StringEventModifierType.SetTrue ? true : false);
                    talkedAngelier = useStringEventModifier.ModifierBoolCheck(stringEventModifierType);
                    break;
                case "TalkedToClemence":
                    talkedToClemence = (stringEventModifierType == StringEventModifierType.SetTrue ? true : false);
                    talkedToClemence = useStringEventModifier.ModifierBoolCheck(stringEventModifierType);
                    break;
                case "Ending":
                    levelLoad.isEnding = (stringEventModifierType == StringEventModifierType.SetTrue ? true : false);
                    levelLoad.isEnding = useStringEventModifier.ModifierBoolCheck(stringEventModifierType);
                    break;
                default:
                    break;
            }
        }

        public void Update()
        {
            if (thought)
            {
                textPanel.sprite = modifyTextPanel;
            }
            else
            {
                var tmpSprite = textPanel.sprite;
            }
        }
    }
}
