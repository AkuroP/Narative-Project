using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueNode : BaseNode
{
    private DialogueData dialogueData = new DialogueData();
    public DialogueData DialogueData { get => dialogueData; set => dialogueData = value; }
    
    private List<Box> boxs = new List<Box>();
    public DialogueNode(){}
    
    public DialogueNode(Vector2 _position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        base.editorWindow = editorWindow;
        base.graphView = graphView;
        
        StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/DialogueNodeStyle");
        styleSheets.Add(styleSheet);
        
        title = "Dialogue";
        SetPosition(new Rect(_position, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();
        
        AddInputPort("Input", Port.Capacity.Multi);
        AddOutputPort("Continue", Port.Capacity.Single);
        
        TopContainer();
    }
    
    private void TopContainer()
    {
        AddPortButton();
        AddDropdownMenu();
    }
    
    private void AddPortButton()
    {
        Button btn = new Button()
        {
            text = "Add Choice",
        };
        btn.AddToClassList("TopBtn");

        btn.clicked += () =>
        {
            AddChoicePort(this);
        };

        titleButtonContainer.Add(btn);
    }
    
    private void AddDropdownMenu()
    {
        ToolbarMenu Menu = new ToolbarMenu();
        Menu.text = "Add Content";

        Menu.menu.AppendAction("Text", new Action<DropdownMenuAction>(x => TextLine()));
        Menu.menu.AppendAction("Image", new Action<DropdownMenuAction>(x => ImagePic()));
        Menu.menu.AppendAction("Name", new Action<DropdownMenuAction>(x => CharacterName()));

        titleButtonContainer.Add(Menu);
    }
    
     // Port ---------------------------------------------------------------------------------------

        public Port AddChoicePort(BaseNode baseNode, DialogueDataPort dialogueDataPort = null)
        {
            Port port = GetPortInstance(Direction.Output);
            DialogueDataPort newDialoguePort = new DialogueDataPort();

            // Check if we load it in with values
            if (dialogueDataPort != null)
            {
                newDialoguePort.inputGuid = dialogueDataPort.inputGuid;
                newDialoguePort.outputGuid = dialogueDataPort.outputGuid;
                newDialoguePort.portGuid = dialogueDataPort.portGuid;
            }
            else
            {
                newDialoguePort.portGuid = Guid.NewGuid().ToString();
            }

            // Delete button
            {
                Button deleteButton = new Button(() => DeletePort(baseNode, port))
                {
                    text = "X",
                };
                port.contentContainer.Add(deleteButton);
            }

            port.portName = newDialoguePort.portGuid;                      // We use portName as port ID
            Label portNameLabel = port.contentContainer.Q<Label>("type");   // Get Labal in port that is used to contain the port name.
            portNameLabel.AddToClassList("PortName");                       // Here we add a uss class to it so we can hide it in the editor window.

            // Set color of the port.
            port.portColor = Color.yellow;

            DialogueData.dialogueDataPorts.Add(newDialoguePort);

            baseNode.outputContainer.Add(port);

            // Refresh
            baseNode.RefreshPorts();
            baseNode.RefreshExpandedState();

            return port;
        }

        private void DeletePort(BaseNode node, Port port)
        {
            DialogueDataPort tmp = DialogueData.dialogueDataPorts.Find(findPort => findPort.portGuid == port.portName);
            DialogueData.dialogueDataPorts.Remove(tmp);

            IEnumerable<Edge> portEdge = graphView.edges.ToList().Where(edge => edge.output == port);

            if (portEdge.Any())
            {
                Edge edge = portEdge.First();
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                graphView.RemoveElement(edge);
            }

            node.outputContainer.Remove(port);

            // Refresh
            node.RefreshPorts();
            node.RefreshExpandedState();
        }
        
         // Menu dropdown --------------------------------------------------------------------------------------

        public void TextLine(DialogueDataText dataText = null)
        {
            DialogueDataText newDialogueBaseContainerText = new DialogueDataText();
            DialogueData.dialogueBaseContainers.Add(newDialogueBaseContainerText);

            // Add Container Box
            Box boxContainer = new Box();
            boxContainer.AddToClassList("DialogueBox");

            // Add Fields
            AddLabelAndButton(newDialogueBaseContainerText, boxContainer, "Text", "TextColor");
            AddTextField(newDialogueBaseContainerText, boxContainer);
            AddAudioClips(newDialogueBaseContainerText, boxContainer);

            // Load in data if it got any
            if (dataText != null)
            {
                // Guid ID
                newDialogueBaseContainerText.guidID = dataText.guidID;

                // Text
                foreach (LanguageGeneric<string> _dataText in dataText.text)
                {
                    foreach (LanguageGeneric<string> text in newDialogueBaseContainerText.text)
                    {
                        if (text.languageType == _dataText.languageType)
                        {
                            text.languageGenericType = _dataText.languageGenericType;
                        }
                    }
                }

                // Audio
                foreach (LanguageGeneric<AudioClip> dataAudioclip in dataText.audioClips)
                {
                    foreach (LanguageGeneric<AudioClip> audioclip in newDialogueBaseContainerText.audioClips)
                    {
                        if (audioclip.languageType == dataAudioclip.languageType)
                        {
                            audioclip.languageGenericType = dataAudioclip.languageGenericType;
                        }
                    }
                }
            }
            else
            {
                // Make New Guid ID
                newDialogueBaseContainerText.guidID.value = Guid.NewGuid().ToString();
            }

            // Reaload the current selected language
            ReloadLanguage();

            mainContainer.Add(boxContainer);
        }

        public void ImagePic(DialogueDataImages dataImages = null)
        {
            DialogueDataImages dialogueImages = new DialogueDataImages();
            if (dataImages != null)
            {
                dialogueImages.spriteLeft.value = dataImages.spriteLeft.value;
                dialogueImages.spriteRight.value = dataImages.spriteRight.value;
            }
            DialogueData.dialogueBaseContainers.Add(dialogueImages);

            Box boxContainer = new Box();
            boxContainer.AddToClassList("DialogueBox");

            AddLabelAndButton(dialogueImages, boxContainer, "Image", "ImageColor");
            AddImages(dialogueImages, boxContainer);

            mainContainer.Add(boxContainer);
        }

        public void CharacterName(DialogueDataName dataName = null)
        {
            DialogueDataName dialogueName = new DialogueDataName();
            if (dataName != null)
            {
                dialogueName.characterName.value = dataName.characterName.value;
            }
            DialogueData.dialogueBaseContainers.Add(dialogueName);

            Box boxContainer = new Box();
            boxContainer.AddToClassList("CharacterNameBox");

            AddLabelAndButton(dialogueName, boxContainer, "Name", "NameColor");
            AddTextField_CharacterName(dialogueName, boxContainer);

            mainContainer.Add(boxContainer);
        }
        
         // Fields --------------------------------------------------------------------------------------

        private void AddLabelAndButton(DialogueDataBaseContainer container, Box boxContainer, string labelName, string uniqueUSS = "")
        {
            Box topBoxContainer = new Box();
            topBoxContainer.AddToClassList("TopBox");

            // Label Name
            Label label_texts = GetNewLabel(labelName, "LabelText", uniqueUSS);

            Box buttonsBox = new Box();
            buttonsBox.AddToClassList("BtnBox");

            // Move Up button.
            Button btnMoveUpBtn = GetNewButton("", "MoveUpBtn");
            btnMoveUpBtn.clicked += () =>
            {
                MoveBox(container, true);
            };

            // Move Down button.
            Button btnMoveDownBtn = GetNewButton("", "MoveDownBtn");
            btnMoveDownBtn.clicked += () =>
            {
                MoveBox(container, false);
            };

            // Remove button.
            Button btnRemove = GetNewButton("X", "TextRemoveBtn");
            btnRemove.clicked += () =>
            {
                DeleteBox(boxContainer);
                boxs.Remove(boxContainer);
                DialogueData.dialogueBaseContainers.Remove(container);
            };

            boxs.Add(boxContainer);

            buttonsBox.Add(btnMoveUpBtn);
            buttonsBox.Add(btnMoveDownBtn);
            buttonsBox.Add(btnRemove);
            topBoxContainer.Add(label_texts);
            topBoxContainer.Add(buttonsBox);

            boxContainer.Add(topBoxContainer);
        }

        private void AddTextField_CharacterName(DialogueDataName container, Box boxContainer)
        {
            TextField textField = GetNewTextField(container.characterName, "Name", "CharacterName");

            boxContainer.Add(textField);
        }

        private void AddTextField(DialogueDataText container, Box boxContainer)
        {
            TextField textField = GetNewTextFieldTextLanguage(container.text, "Text areans", "TextBox");

            container.TextField = textField;

            boxContainer.Add(textField);
        }

        private void AddAudioClips(DialogueDataText container, Box boxContainer)
        {
            ObjectField objectField = GetNewObjectFieldAudioClipsLanguage(container.audioClips, "AudioClip");

            container.ObjectField = objectField;

            boxContainer.Add(objectField);
        }

        private void AddImages(DialogueDataImages container, Box boxContainer)
        {
            Box imagePreviewBox = new Box();
            Box imagesBox = new Box();

            imagePreviewBox.AddToClassList("BoxRow");
            imagesBox.AddToClassList("BoxRow");

            // Set up Image Preview.
            Image leftImage = GetNewImage("ImagePreview", "ImagePreviewLeft");
            Image rightImage = GetNewImage("ImagePreview", "ImagePreviewRight");

            imagePreviewBox.Add(leftImage);
            imagePreviewBox.Add(rightImage);

            // Set up Sprite.
            ObjectField objectFieldLeft = GetNewObjectFieldSprite(container.spriteLeft, leftImage, "SpriteLeft");
            ObjectField objectFieldRight = GetNewObjectFieldSprite(container.spriteRight, rightImage, "SpriteRight");

            imagesBox.Add(objectFieldLeft);
            imagesBox.Add(objectFieldRight);

            // Add to box container.
            boxContainer.Add(imagePreviewBox);
            boxContainer.Add(imagesBox);
        }
        
        // ------------------------------------------------------------------------------------------

        private void MoveBox(DialogueDataBaseContainer container, bool moveUp)
        {
            List<DialogueDataBaseContainer> tmpDialogueBaseContainers = new List<DialogueDataBaseContainer>();
            tmpDialogueBaseContainers.AddRange(dialogueData.dialogueBaseContainers);

            foreach (Box item in boxs)
            {
                mainContainer.Remove(item);
            }

            boxs.Clear();

            for (int i = 0; i < tmpDialogueBaseContainers.Count; i++)
            {
                tmpDialogueBaseContainers[i].id.value = i;
            }

            if (container.id.value > 0 && moveUp)
            {
                DialogueDataBaseContainer tmp01 = tmpDialogueBaseContainers[container.id.value];
                DialogueDataBaseContainer tmp02 = tmpDialogueBaseContainers[container.id.value - 1];

                tmpDialogueBaseContainers[container.id.value] = tmp02;
                tmpDialogueBaseContainers[container.id.value - 1] = tmp01;
            }
            else if (container.id.value < tmpDialogueBaseContainers.Count - 1 && !moveUp)
            {
                DialogueDataBaseContainer tmp01 = tmpDialogueBaseContainers[container.id.value];
                DialogueDataBaseContainer tmp02 = tmpDialogueBaseContainers[container.id.value + 1];

                tmpDialogueBaseContainers[container.id.value] = tmp02;
                tmpDialogueBaseContainers[container.id.value + 1] = tmp01;
            }

            dialogueData.dialogueBaseContainers.Clear();

            foreach (DialogueDataBaseContainer data in tmpDialogueBaseContainers)
            {
                switch (data)
                {
                    case DialogueDataName Name:
                        CharacterName(Name);
                        break;
                    case DialogueDataText Text:
                        TextLine(Text);
                        break;
                    case DialogueDataImages image:
                        ImagePic(image);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void LoadValueInToField()
        {

        }
}
