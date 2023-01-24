using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_Exemple : GameEvents
{
    public override bool DialogueConditionEvents(string stringEvent, StringEventConditionType stringEventConditionType, float value = 0)
    {
        switch (stringEvent.ToLower())
        {
            case "money":
                return useStringEventCondition.ConditionFloatCheck(FindObjectOfType<Player>().Money, value, stringEventConditionType);
            case "health":
                return useStringEventCondition.ConditionFloatCheck(FindObjectOfType<Player>().Health, value, stringEventConditionType);
            case "didaskforfood":
                return useStringEventCondition.ConditionBoolCheck(FindObjectOfType<Player>().DidWeTalk,stringEventConditionType);
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
                FindObjectOfType<Player>().ModifyMoney((int)useStringEventModifier.ModifierFloatCheck(value, stringEventModifierType));
                break;
            case "health":
                FindObjectOfType<Player>().ModifyHealth((int)useStringEventModifier.ModifierFloatCheck(value, stringEventModifierType));
                break;
            case "didaskforfood":
                FindObjectOfType<Player>().DidWeTalk = (stringEventModifierType == StringEventModifierType.SetTrue ? true : false);
                FindObjectOfType<Player>().DidWeTalk = useStringEventModifier.ModifierBoolCheck(stringEventModifierType);
                break;
            default:
                break;
        }
    }
}
