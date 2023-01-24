using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventData : BaseData
{
    public List<EventDataStringModifier> stringModifiers = new List<EventDataStringModifier>();
    public List<ContainerDialogueEventSo> dialogueEventSos = new List<ContainerDialogueEventSo>();
}
