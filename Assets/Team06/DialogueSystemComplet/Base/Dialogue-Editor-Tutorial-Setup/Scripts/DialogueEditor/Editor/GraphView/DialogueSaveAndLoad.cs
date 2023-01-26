using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace Team06
{
    public class DialogueSaveAndLoad
    {
        private List<Edge> edges => _graphView.edges.ToList();

        private List<BaseNode> nodes =>
            _graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();

        private DialogueGraphView _graphView;

        public DialogueSaveAndLoad(DialogueGraphView graphView)
        {
            this._graphView = graphView;
        }

        public void Save(DialogueContainerSO dialogueContainerSo)
        {
            SaveEdges(dialogueContainerSo);
            SaveNodes(dialogueContainerSo);

            EditorUtility.SetDirty(dialogueContainerSo);
            AssetDatabase.SaveAssets();
        }

        public void Load(DialogueContainerSO dialogueContainerSo)
        {
            ClearGraph();
            GenerateNodes(dialogueContainerSo);
            ConnectNodes(dialogueContainerSo);
        }

        #region Save

        private void SaveEdges(DialogueContainerSO dialogueContainerSo)
        {
            dialogueContainerSo.nodeLinkDatas.Clear();

            Edge[] connectedEdges = edges.Where(edge => edge.input.node != null).ToArray();
            for (int i = 0; i < connectedEdges.Count(); i++)
            {
                BaseNode outputNode = (BaseNode) connectedEdges[i].output.node;
                BaseNode inputNode = connectedEdges[i].input.node as BaseNode;

                dialogueContainerSo.nodeLinkDatas.Add(new DialogueContainerSO.NodeLinkDatas()
                {
                    baseNodeGuid = outputNode.NodeGuid,
                    basePortName = connectedEdges[i].output.portName,
                    targetNodeGuid = inputNode?.NodeGuid,
                    targetPortName = connectedEdges[i].input.portName,
                });
            }
        }

        private void SaveNodes(DialogueContainerSO dialogueContainerSo)
        {
            dialogueContainerSo.startDatas.Clear();
            dialogueContainerSo.dialogueDatas.Clear();
            dialogueContainerSo.eventDatas.Clear();
            dialogueContainerSo.branchDatas.Clear();
            dialogueContainerSo.choiceDatas.Clear();
            dialogueContainerSo.endDatas.Clear();

            nodes.ForEach(node =>
            {
                switch (node)
                {
                    case StartNode startNode:
                        dialogueContainerSo.startDatas.Add(SaveNodeData(startNode));
                        break;
                    case DialogueNode dialogueNode:
                        dialogueContainerSo.dialogueDatas.Add(SaveNodeData(dialogueNode));
                        break;
                    case EventNode eventNode:
                        dialogueContainerSo.eventDatas.Add(SaveNodeData(eventNode));
                        break;
                    case BranchNode branchNode:
                        dialogueContainerSo.branchDatas.Add(SaveNodeData(branchNode));
                        break;
                    case ChoiceNode choiceNode:
                        dialogueContainerSo.choiceDatas.Add(SaveNodeData(choiceNode));
                        break;
                    case EndNode endNode:
                        dialogueContainerSo.endDatas.Add(SaveNodeData(endNode));
                        break;
                    default:
                        break;
                }
            });
        }

        private DialogueData SaveNodeData(DialogueNode node)
        {
            DialogueData dialogueData = new DialogueData
            {
                nodeGuid = node.NodeGuid,
                position = node.GetPosition().position,
            };

            // Set ID
            for (int i = 0; i < node.DialogueData.dialogueBaseContainers.Count; i++)
            {
                node.DialogueData.dialogueBaseContainers[i].id.value = i;
            }

            foreach (DialogueDataBaseContainer baseContainer in node.DialogueData.dialogueBaseContainers)
            {
                // Name
                if (baseContainer is DialogueDataName)
                {
                    DialogueDataName tmp = (baseContainer as DialogueDataName);
                    DialogueDataName tmpData = new DialogueDataName();

                    tmpData.id.value = tmp.id.value;
                    tmpData.characterName.value = tmp.characterName.value;

                    dialogueData.dialogueDataNames.Add(tmpData);
                }

                // Text
                if (baseContainer is DialogueDataText)
                {
                    DialogueDataText tmp = (baseContainer as DialogueDataText);
                    DialogueDataText tmpData = new DialogueDataText();

                    tmpData.id = tmp.id;
                    tmpData.guidID = tmp.guidID;
                    tmpData.text = tmp.text;
                    tmpData.audioClips = tmp.audioClips;

                    dialogueData.dialogueDataTexts.Add(tmpData);
                }

                // Images
                if (baseContainer is DialogueDataImages)
                {
                    DialogueDataImages tmp = (baseContainer as DialogueDataImages);
                    DialogueDataImages tmpData = new DialogueDataImages();

                    tmpData.id.value = tmp.id.value;
                    tmpData.spriteLeft.value = tmp.spriteLeft.value;
                    tmpData.spriteRight.value = tmp.spriteRight.value;

                    dialogueData.dialogueDataImages.Add(tmpData);
                }
            }

            // Port
            foreach (DialogueDataPort port in node.DialogueData.dialogueDataPorts)
            {
                DialogueDataPort portData = new DialogueDataPort();

                portData.outputGuid = string.Empty;
                portData.inputGuid = string.Empty;
                portData.portGuid = port.portGuid;

                foreach (Edge edge in edges)
                {
                    if (edge.output.portName == port.portGuid)
                    {
                        portData.outputGuid = (edge.output.node as BaseNode).NodeGuid;
                        portData.inputGuid = (edge.input.node as BaseNode).NodeGuid;
                    }
                }

                dialogueData.dialogueDataPorts.Add(portData);
            }

            return dialogueData;
        }

        private StartData SaveNodeData(StartNode node)
        {
            StartData nodeData = new StartData()
            {
                nodeGuid = node.NodeGuid,
                position = node.GetPosition().position,
            };

            return nodeData;
        }

        private EndData SaveNodeData(EndNode node)
        {
            EndData nodeData = new EndData()
            {
                nodeGuid = node.NodeGuid,
                position = node.GetPosition().position,
            };
            nodeData.endNodeType.value = node.EndData.endNodeType.value;

            return nodeData;
        }

        private EventData SaveNodeData(EventNode node)
        {
            EventData nodeData = new EventData()
            {
                nodeGuid = node.NodeGuid,
                position = node.GetPosition().position,
            };

            // Save Dialogue Event
            foreach (ContainerDialogueEventSo dialogueEvent in node.EventData.dialogueEventSos)
            {
                nodeData.dialogueEventSos.Add(dialogueEvent);
            }

            // Save String Event
            foreach (EventDataStringModifier stringEvents in node.EventData.stringModifiers)
            {
                EventDataStringModifier tmp = new EventDataStringModifier
                {
                    number =
                    {
                        value = stringEvents.number.value
                    },
                    stringEventText =
                    {
                        value = stringEvents.stringEventText.value
                    },
                    stringEventModifierType =
                    {
                        value = stringEvents.stringEventModifierType.value
                    }
                };

                nodeData.stringModifiers.Add(tmp);
            }

            return nodeData;
        }

        private BranchData SaveNodeData(BranchNode node)
        {
            List<Edge> tmpEdges = edges.Where(x => x.output.node == node).Cast<Edge>().ToList();

            Edge trueOutput = edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "True");
            Edge flaseOutput = edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "False");

            BranchData nodeData = new BranchData()
            {
                nodeGuid = node.NodeGuid,
                position = node.GetPosition().position,
                trueGuidNode = (trueOutput != null ? (trueOutput.input.node as BaseNode)?.NodeGuid : string.Empty),
                falseGuidNode = (flaseOutput != null ? (flaseOutput.input.node as BaseNode)?.NodeGuid : string.Empty),
            };

            foreach (EventDataStringCondition stringEvents in node.BranchData.stringConditions)
            {
                EventDataStringCondition tmp = new EventDataStringCondition
                {
                    number =
                    {
                        value = stringEvents.number.value
                    },
                    stringEventText =
                    {
                        value = stringEvents.stringEventText.value
                    },
                    stringEventConditionType =
                    {
                        value = stringEvents.stringEventConditionType.value
                    }
                };

                nodeData.stringConditions.Add(tmp);
            }

            return nodeData;
        }

        private ChoiceData SaveNodeData(ChoiceNode node)
        {
            ChoiceData nodeData = new ChoiceData
            {
                nodeGuid = node.NodeGuid,
                position = node.GetPosition().position,

                text = node.ChoiceData.text,
                audioClips = node.ChoiceData.audioClips,
                choiceStateTypes =
                {
                    value = node.ChoiceData.choiceStateTypes.value
                }
            };

            foreach (EventDataStringCondition stringEvents in node.ChoiceData.stringConditions)
            {
                EventDataStringCondition tmp = new EventDataStringCondition();
                tmp.stringEventText.value = stringEvents.stringEventText.value;
                tmp.number.value = stringEvents.number.value;
                tmp.stringEventConditionType.value = stringEvents.stringEventConditionType.value;

                nodeData.stringConditions.Add(tmp);
            }

            return nodeData;
        }

        #endregion

        #region Load

        private void ClearGraph()
        {
            edges.ForEach(edge => _graphView.RemoveElement(edge));

            foreach (BaseNode node in nodes)
            {
                _graphView.RemoveElement(node);
            }
        }

        private void GenerateNodes(DialogueContainerSO dialogueContainer)
        {
            // Start
            foreach (StartData node in dialogueContainer.startDatas)
            {
                StartNode tempNode = _graphView.CreateStartNode(node.position);
                tempNode.NodeGuid = node.nodeGuid;

                _graphView.AddElement(tempNode);
            }

            // End Node 
            foreach (EndData node in dialogueContainer.endDatas)
            {
                EndNode tempNode = _graphView.CreateEndNode(node.position);
                tempNode.NodeGuid = node.nodeGuid;
                tempNode.EndData.endNodeType.value = node.endNodeType.value;

                tempNode.LoadValueInToField();
                _graphView.AddElement(tempNode);
            }

            // Event Node
            foreach (EventData node in dialogueContainer.eventDatas)
            {
                EventNode tempNode = _graphView.CreateEventNode(node.position);
                tempNode.NodeGuid = node.nodeGuid;

                foreach (ContainerDialogueEventSo item in node.dialogueEventSos)
                {
                    tempNode.AddScriptableEvent(item);
                }

                foreach (EventDataStringModifier item in node.stringModifiers)
                {
                    tempNode.AddStringEvent(item);
                }

                tempNode.LoadValueInToField();
                _graphView.AddElement(tempNode);
            }

            // Breach Node
            foreach (BranchData node in dialogueContainer.branchDatas)
            {
                BranchNode tempNode = _graphView.CreateBranchNode(node.position);
                tempNode.NodeGuid = node.nodeGuid;

                foreach (EventDataStringCondition item in node.stringConditions)
                {
                    tempNode.AddCondition(item);
                }

                tempNode.LoadValueInToField();
                tempNode.ReloadLanguage();
                _graphView.AddElement(tempNode);
            }

            // Choice Node
            foreach (ChoiceData node in dialogueContainer.choiceDatas)
            {
                ChoiceNode tempNode = _graphView.CreateChoiceNode(node.position);
                tempNode.NodeGuid = node.nodeGuid;

                tempNode.ChoiceData.choiceStateTypes.value = node.choiceStateTypes.value;

                foreach (LanguageGeneric<string> dataText in node.text)
                {
                    foreach (LanguageGeneric<string> editorText in tempNode.ChoiceData.text)
                    {
                        if (editorText.languageType == dataText.languageType)
                        {
                            editorText.languageGenericType = dataText.languageGenericType;
                        }
                    }
                }

                foreach (LanguageGeneric<AudioClip> dataAudioClip in node.audioClips)
                {
                    foreach (LanguageGeneric<AudioClip> editorAudioClip in tempNode.ChoiceData.audioClips)
                    {
                        if (editorAudioClip.languageType == dataAudioClip.languageType)
                        {
                            editorAudioClip.languageGenericType = dataAudioClip.languageGenericType;
                        }
                    }
                }

                foreach (EventDataStringCondition item in node.stringConditions)
                {
                    tempNode.AddCondition(item);
                }

                tempNode.LoadValueInToField();
                tempNode.ReloadLanguage();
                _graphView.AddElement(tempNode);
            }

            // Dialogue Node
            foreach (var node in dialogueContainer.dialogueDatas)
            {
                DialogueNode tempNode = _graphView.CreateDialogueNode(node.position);
                Debug.Log("respect moi stp");
                tempNode.NodeGuid = node.nodeGuid;

                List<DialogueDataBaseContainer> dataBaseContainer = new List<DialogueDataBaseContainer>();

                dataBaseContainer.AddRange(node.dialogueDataImages);
                dataBaseContainer.AddRange(node.dialogueDataTexts);
                dataBaseContainer.AddRange(node.dialogueDataNames);

                dataBaseContainer.Sort(delegate(DialogueDataBaseContainer x, DialogueDataBaseContainer y)
                {
                    return x.id.value.CompareTo(y.id.value);
                });

                foreach (DialogueDataBaseContainer data in dataBaseContainer)
                {
                    switch (data)
                    {
                        case DialogueDataName Name:
                            tempNode.CharacterName(Name);
                            break;
                        case DialogueDataText Text:
                            tempNode.TextLine(Text);
                            break;
                        case DialogueDataImages image:
                            tempNode.ImagePic(image);
                            break;
                        default:
                            break;
                    }
                }

                foreach (DialogueDataPort port in node.dialogueDataPorts)
                {
                    tempNode.AddChoicePort(tempNode, port);
                }

                tempNode.LoadValueInToField();
                tempNode.ReloadLanguage();
                _graphView.AddElement(tempNode);
            }
        }

        private void ConnectNodes(DialogueContainerSO dialogueContainer)
        {
            // Make connection for all node.
            for (int i = 0; i < nodes.Count; i++)
            {
                List<DialogueContainerSO.NodeLinkDatas> connections = dialogueContainer.nodeLinkDatas
                    .Where(edge => edge.baseNodeGuid == nodes[i].NodeGuid).ToList();

                List<Port> allOutputPorts =
                    nodes[i].outputContainer.Children().Where(x => x is Port).Cast<Port>().ToList();

                for (int j = 0; j < connections.Count; j++)
                {
                    string targetNodeGuid = connections[j].targetNodeGuid;
                    var targetNode = nodes.FirstOrDefault(node => node.NodeGuid == targetNodeGuid);

                    if (targetNode == null)
                        continue;

                    foreach (Port item in allOutputPorts)
                    {
                        if (item.portName == connections[j].basePortName)
                        {
                            LinkNodesTogether(item, (Port) targetNode.inputContainer[0]);
                        }
                    }
                }
            }
        }

        private void LinkNodesTogether(Port outputPort, Port inputPort)
        {
            Edge tempEdge = new Edge()
            {
                output = outputPort,
                input = inputPort
            };
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            _graphView.Add(tempEdge);
        }

        #endregion
    }
}

