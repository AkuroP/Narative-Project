using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class EventNode : BaseNode
{ 
    private EventData _eventData = new EventData();
    public EventData EventData { get => _eventData; set => _eventData = value; }
    public EventNode(){}

    public EventNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        base.editorWindow = editorWindow;
        base.graphView = graphView;
        
        StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/EventNodeStyle");
        styleSheets.Add(styleSheet);
        
        title = "Event";
        SetPosition(new Rect(position, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();
        
        AddInputPort("Input", Port.Capacity.Multi);
        AddOutputPort("Output", Port.Capacity.Single);
        
        TopButton();
        
    }
    
    private void TopButton()
    {
        ToolbarMenu menu = new ToolbarMenu
        {
            text = "Add Event"
        };

        menu.menu.AppendAction("String Event Modifier", new Action<DropdownMenuAction>(x => AddStringEvent()));
        menu.menu.AppendAction("Scriptable Object", new Action<DropdownMenuAction>(x => AddScriptableEvent()));

        titleContainer.Add(menu);
    }
    
    public void AddStringEvent(EventDataStringModifier stringEvent = null)
    {
        AddStringModifierEventBuild(_eventData.stringModifiers, stringEvent);
    }
    
    public void AddScriptableEvent(ContainerDialogueEventSo eventSoData = null)
    {
        ContainerDialogueEventSo tmpDialogueEventSo = new ContainerDialogueEventSo();

        // If we paramida value is not null we load in values.
        if (eventSoData != null)
        {
            tmpDialogueEventSo.dialogueEventSo = eventSoData.dialogueEventSo;
        }
        _eventData.dialogueEventSos.Add(tmpDialogueEventSo);

        // Container of all object.
        Box boxContainer = new Box();
        boxContainer.AddToClassList("EventBox");

        // Scriptable Object Event.
        ObjectField objectField = GetNewObjectFieldDialogueEvent(tmpDialogueEventSo, "EventObject");

        // Remove button.
        Button btn = GetNewButton("X", "removeBtn");
        btn.clicked += () =>
        {
            DeleteBox(boxContainer);
            EventData.dialogueEventSos.Remove(tmpDialogueEventSo);
        };

        // Add it to the box
        boxContainer.Add(objectField);
        boxContainer.Add(btn);

        mainContainer.Add(boxContainer);
        RefreshExpandedState();
    }

    
}
