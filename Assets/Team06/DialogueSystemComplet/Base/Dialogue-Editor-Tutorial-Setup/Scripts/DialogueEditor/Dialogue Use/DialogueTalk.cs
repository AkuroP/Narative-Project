using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Team06
{
    public class DialogueTalk : DialogueGetData
    {
        [SerializeField] private bool autoDialogue;
        [SerializeField] private float timeBetweenSentence = 3f;

        
        private DialogueController _dialogueController;
        private AudioSource _audioSource;

        private DialogueData _currentDialogueNodeData;
        private DialogueData _lastDialogueNodeData;

        private List<DialogueDataBaseContainer> _baseContainers;
        private int _currentIndex = 0;

        public TimelineSwitcher timelineSwitcher;
        public Team06.TeamSixPlayer player;

        private void Awake()
        {
            _dialogueController = FindObjectOfType<DialogueController>();
            _audioSource = GetComponent<AudioSource>();
        }

        public void StartDialogue()
        {
            _dialogueController.ShowDialogueUI(true);
            timelineSwitcher.canSwitch = false;
            player.PlayerStopMoving();
            player.CanMove = false;
            CheckNodeType(GetNextNode(dialogueContainer.startDatas[0]));
        }

        private void CheckNodeType(BaseData baseNodeData)
        {
            switch (baseNodeData)
            {
                case StartData nodeData:
                    RunNode(nodeData);
                    break;
                case DialogueData nodeData:
                    RunNode(nodeData);
                    break;
                case EventData nodeData:
                    RunNode(nodeData);
                    break;
                case EndData nodeData:
                    RunNode(nodeData);
                    break;
                case BranchData nodeData:
                    RunNode(nodeData);
                    break;
                default:
                    break;
            }
        }

        //Run Start Node
        private void RunNode(StartData nodeData)
        {
            CheckNodeType(GetNextNode(dialogueContainer.startDatas[0]));
        }

        //Run Branch Node
        private void RunNode(BranchData nodeData)
        {
            bool checkBranch = true;
            foreach (EventDataStringCondition item in nodeData.stringConditions)
            {
                if (!GameEvents.Instance.DialogueConditionEvents(item.stringEventText.value,
                    item.stringEventConditionType.value, item.number.value))
                {
                    checkBranch = false;
                    break;
                }
            }

            string nextNoce = (checkBranch ? nodeData.trueGuidNode : nodeData.falseGuidNode);
            CheckNodeType(GetNodeByGuid(nextNoce));
        }

        //Run Event Node
        private void RunNode(EventData nodeData)
        {
            foreach (ContainerDialogueEventSo item in nodeData.dialogueEventSos)
            {
                if (item.dialogueEventSo != null)
                {
                    item.dialogueEventSo.RunEvent();
                }
            }

            foreach (EventDataStringModifier item in nodeData.stringModifiers)
            {
                GameEvents.Instance.DialogueModifierEvents(item.stringEventText.value,
                    item.stringEventModifierType.value, item.number.value);
            }

            CheckNodeType(GetNextNode(nodeData));
        }

        //Run End Node
        private void RunNode(EndData nodeData)
        {
            switch (nodeData.endNodeType.value)
            {
                case EndNodeType.End:
                    _dialogueController.ShowDialogueUI(false);
                    break;
                case EndNodeType.Repeat:
                    CheckNodeType(GetNodeByGuid(_currentDialogueNodeData.nodeGuid));
                    break;
                case EndNodeType.ReturnToStart:
                    CheckNodeType(GetNextNode(dialogueContainer.startDatas[0]));
                    break;
                default:
                    break;
            }
        }

        //Run Dialogue Node
        private void RunNode(DialogueData nodeData)
        {
            _currentDialogueNodeData = nodeData;

            _baseContainers = new List<DialogueDataBaseContainer>();
            _baseContainers.AddRange(nodeData.dialogueDataImages);
            _baseContainers.AddRange(nodeData.dialogueDataNames);
            _baseContainers.AddRange(nodeData.dialogueDataTexts);

            _currentIndex = 0;

            _baseContainers.Sort(delegate(DialogueDataBaseContainer x, DialogueDataBaseContainer y)
            {
                return x.id.value.CompareTo(y.id.value);
            });

            DialogueToDo();
        }

        private void DialogueToDo()
        {
            _dialogueController.HideButtons();

            for (int i = _currentIndex; i < _baseContainers.Count; i++)
            {
                _currentIndex = i + 1;
                if (_baseContainers[i] is DialogueDataName)
                {
                    DialogueDataName tmp = _baseContainers[i] as DialogueDataName;
                    _dialogueController.SetName(tmp?.characterName.value);
                }

                if (_baseContainers[i] is DialogueDataImages)
                {
                    DialogueDataImages tmp = _baseContainers[i] as DialogueDataImages;
                    _dialogueController.SetImage(tmp?.spriteLeft.value, tmp?.spriteRight.value);
                }

                if (_baseContainers[i] is DialogueDataText)
                {
                    DialogueDataText tmp = _baseContainers[i] as DialogueDataText;
                    print("");
                    _dialogueController.SetText(tmp.text
                        .Find(text => text.languageType == LanguageController.Instance.LanguageType)
                        .languageGenericType);
                    PlayAudio(tmp?.audioClips
                        .Find(text => text.languageType == LanguageController.Instance.LanguageType)
                        .languageGenericType);
                    Buttons();
                    break;
                }
            }
        }

        private void PlayAudio(AudioClip audioClip)
        {
            _audioSource.Stop();
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }

        private void Buttons()
        {
            if (_currentIndex == _baseContainers.Count && _currentDialogueNodeData.dialogueDataPorts.Count == 0)
            {
                if (autoDialogue)
                {
                    StartCoroutine(EndDialogue());
                }
                else
                {
                    UnityAction unityAction = null;
                    unityAction += () => CheckNodeType(GetNextNode(_currentDialogueNodeData));
                    _dialogueController.SetContinue(unityAction);
                }
                
                print("1");
                
            }
            else if (_currentIndex == _baseContainers.Count)
            {
                List<DialogueButtonContainer> dialogueButtonContainers = new List<DialogueButtonContainer>();

                foreach (DialogueDataPort port in _currentDialogueNodeData.dialogueDataPorts)
                {
                    ChoiceCheck(port.inputGuid, dialogueButtonContainers);
                }
                print("2");
                _dialogueController.SetButtons(dialogueButtonContainers);
            }
            else
            {
                print("3");
                if (autoDialogue)
                {
                    StartCoroutine(NextDialogue());
                }
                else
                {
                    UnityAction unityAction = null;
                    unityAction += () => DialogueToDo();
                    _dialogueController.SetContinue(unityAction);
                }
                
            }
        }

        private void ChoiceCheck(string guidID, List<DialogueButtonContainer> dialogueButtonContainers)
        {
            BaseData asd = GetNodeByGuid(guidID);
            ChoiceData choiceNode = GetNodeByGuid(guidID) as ChoiceData;
            DialogueButtonContainer dialogueButtonContainer = new DialogueButtonContainer();

            bool checkBranch = true;
            foreach (EventDataStringCondition item in choiceNode.stringConditions)
            {
                if (!GameEvents.Instance.DialogueConditionEvents(item.stringEventText.value,
                    item.stringEventConditionType.value, item.number.value))
                {
                    checkBranch = false;
                    break;
                }
            }

            UnityAction unityAction = null;
            unityAction += () => CheckNodeType(GetNextNode(choiceNode));

            dialogueButtonContainer.choiceState = choiceNode.choiceStateTypes.value;
            dialogueButtonContainer.text = choiceNode.text
                .Find(text => text.languageType == LanguageController.Instance.LanguageType).languageGenericType;
            dialogueButtonContainer.unityAction = unityAction;
            dialogueButtonContainer.conditionCheck = checkBranch;

            dialogueButtonContainers.Add(dialogueButtonContainer);
        }

        IEnumerator NextDialogue()
        {
            yield return new WaitForSeconds(timeBetweenSentence);
            DialogueToDo();
        }
        IEnumerator EndDialogue()
        {
            yield return new WaitForSeconds(timeBetweenSentence);
            timelineSwitcher.canSwitch = true;
            player.CanMove = true;
            CheckNodeType(GetNextNode(_currentDialogueNodeData));
        }
    }
}

