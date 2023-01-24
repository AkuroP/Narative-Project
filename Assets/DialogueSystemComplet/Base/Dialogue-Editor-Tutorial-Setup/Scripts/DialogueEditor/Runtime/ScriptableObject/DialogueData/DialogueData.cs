using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class DialogueData : BaseData
{
    public List<DialogueDataBaseContainer> dialogueBaseContainers { get; set; } = new List<DialogueDataBaseContainer>();
    public List<DialogueDataName> dialogueDataNames = new List<DialogueDataName>();
    public List<DialogueDataText> dialogueDataTexts = new List<DialogueDataText>();
    public List<DialogueDataImages> dialogueDataImages = new List<DialogueDataImages>();
    public List<DialogueDataPort> dialogueDataPorts = new List<DialogueDataPort>();
}

[System.Serializable]
public class DialogueDataBaseContainer
{
    public ContainerInt id = new ContainerInt();
}

[System.Serializable]
public class DialogueDataName : DialogueDataBaseContainer
{
    public ContainerString characterName = new ContainerString();
}

[System.Serializable]
public class DialogueDataText : DialogueDataBaseContainer
{
#if UNITY_EDITOR
    public TextField TextField { get; set; }
    public ObjectField ObjectField { get; set; }
#endif
    public ContainerString guidID = new ContainerString();
    public List<LanguageGeneric<string>> text = new List<LanguageGeneric<string>>();
    public List<LanguageGeneric<AudioClip>> audioClips = new List<LanguageGeneric<AudioClip>>();
}

[System.Serializable]
public class DialogueDataImages : DialogueDataBaseContainer
{
    public ContainerSprite spriteLeft = new ContainerSprite();
    public ContainerSprite spriteRight = new ContainerSprite();
}

[System.Serializable]
public class DialogueDataPort
{
    public string portGuid;
    public string inputGuid;
    public string outputGuid;
}

