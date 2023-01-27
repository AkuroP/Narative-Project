using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

namespace Team06
{
    public class DialogueContainerValues
    {
    }

    [System.Serializable]
    public class ContainerDialogueEventSo
    {
        public DialogueEventSO dialogueEventSo;
    }

    [System.Serializable]
    public class LanguageGeneric<T>
    {
        public LanguageType languageType;
        public T languageGenericType;
    }

//Values-------------------
    [System.Serializable]
    public class ContainerString
    {
        public string value;
    }

    [System.Serializable]
    public class ContainerInt
    {
        public int value;
    }

    [System.Serializable]
    public class ContainerFloat
    {
        public float value;
    }

    [System.Serializable]
    public class ContainerSprite
    {
        public Sprite value;
    }

//Enum-------------------
    [System.Serializable]
    public class ContainerChoiceStateType
    {
#if UNITY_EDITOR
        public EnumField EnumField;
#endif
        public ChoicesStateType value = ChoicesStateType.Hide;
    }

    [System.Serializable]
    public class ContainerEndNodeType
    {
#if UNITY_EDITOR
        public EnumField EnumField;
#endif
        public EndNodeType value = EndNodeType.End;
    }

    [System.Serializable]
    public class ContainerLanguageType
    {
#if UNITY_EDITOR
        public EnumField EnumField;
#endif
        public LanguageType value = LanguageType.French;
    }

    [System.Serializable]
    public class ContainerStringEventModifierType
    {
#if UNITY_EDITOR
        public EnumField EnumField;
#endif
        public StringEventModifierType value = StringEventModifierType.SetTrue;
    }

    [System.Serializable]
    public class ContainerStringEventConditionType
    {
#if UNITY_EDITOR
        public EnumField EnumField;
#endif
        public StringEventConditionType value = StringEventConditionType.True;
    }

    [System.Serializable]
    public class EventDataStringModifier
    {
        public ContainerString stringEventText = new ContainerString();
        public ContainerFloat number = new ContainerFloat();

        public ContainerStringEventModifierType stringEventModifierType = new ContainerStringEventModifierType();
    }

    [System.Serializable]
    public class EventDataStringCondition
    {
        public ContainerString stringEventText = new ContainerString();
        public ContainerFloat number = new ContainerFloat();

        public ContainerStringEventConditionType stringEventConditionType = new ContainerStringEventConditionType();
    }
}


