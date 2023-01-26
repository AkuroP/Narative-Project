using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Team06
{
    [System.Serializable]
   public class ChoiceData : BaseData
   {
#if UNITY_EDITOR
      public TextField TextField { get; set; }
      public ObjectField ObjectField { get; set; }
#endif

      public ContainerChoiceStateType choiceStateTypes = new ContainerChoiceStateType();
      public List<LanguageGeneric<string>> text = new List<LanguageGeneric<string>>();
      public List<LanguageGeneric<AudioClip>> audioClips = new List<LanguageGeneric<AudioClip>>();
      public List<EventDataStringCondition> stringConditions = new List<EventDataStringCondition>();
   }
}
