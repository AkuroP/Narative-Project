using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ChoiceNode : BaseNode
{
    private ChoiceData _choiceData = new ChoiceData();
    public ChoiceData ChoiceData { get => _choiceData; set => _choiceData = value; }
    
    private Box _choiceStateEnumBox;
    public ChoiceNode(){}

    public ChoiceNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        base.editorWindow = editorWindow;
        base.graphView = graphView;

        StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/ChoiceNodeStyle");
        styleSheets.Add(styleSheet);

        title = "Choice";                                   
        SetPosition(new Rect(position, defaultNodeSize));   
        nodeGuid = Guid.NewGuid().ToString();               

        // Add standard ports.
        Port inputPort = AddInputPort("Input", Port.Capacity.Multi);
        AddOutputPort("Output", Port.Capacity.Single);
        
        inputPort.portColor = Color.yellow;
        
        TopButton();

        TextLine();
        ChoiceStateEnum();

    }
    
    private void TopButton()
    {
        ToolbarMenu menu = new ToolbarMenu
        {
            text = "Add Condition"
        };

        menu.menu.AppendAction("String Event Condition", new Action<DropdownMenuAction>(x => AddCondition()));

        titleButtonContainer.Add(menu);
    }

    public void AddCondition(EventDataStringCondition stringEvent = null)
    {
        AddStringConditionEventBuild(ChoiceData.stringConditions, stringEvent);
        ShowHideChoiceEnum();
    }
    
    public void TextLine()
    {
        // Make Container Box
        Box boxContainer = new Box();
        boxContainer.AddToClassList("TextLineBox");

        // Text
        TextField textField = GetNewTextFieldTextLanguage(ChoiceData.text, "Text", "TextBox");
        ChoiceData.TextField = textField;
        boxContainer.Add(textField);

        // Audio
        ObjectField objectField = GetNewObjectFieldAudioClipsLanguage(ChoiceData.audioClips, "AudioClip");
        ChoiceData.ObjectField = objectField;
        boxContainer.Add(objectField);

        // Reload the current selected language
        ReloadLanguage();

        mainContainer.Add(boxContainer);
    }
    
    private void ChoiceStateEnum()
    {
        _choiceStateEnumBox = new Box();
        _choiceStateEnumBox.AddToClassList("BoxRow");
        ShowHideChoiceEnum();

        // Make fields.
        Label enumLabel = GetNewLabel("If the condition is not met", "ChoiceLabel");
        EnumField choiceStateEnumField = GetNewEnumFieldChoiceStateType(ChoiceData.choiceStateTypes, "enumHide");

        // Add fields to box.
        _choiceStateEnumBox.Add(choiceStateEnumField);
        _choiceStateEnumBox.Add(enumLabel);

        mainContainer.Add(_choiceStateEnumBox);
    }
    
    protected override void DeleteBox(Box boxContainer)
    {
        base.DeleteBox(boxContainer);
        ShowHideChoiceEnum();
    }

    private void ShowHideChoiceEnum()
    {
        ShowHide(ChoiceData.stringConditions.Count > 0, _choiceStateEnumBox);
    }
    
    public override void LoadValueInToField()
    {
        ChoiceData.choiceStateTypes.EnumField?.SetValueWithoutNotify(ChoiceData.choiceStateTypes.value);
    }

    
}
