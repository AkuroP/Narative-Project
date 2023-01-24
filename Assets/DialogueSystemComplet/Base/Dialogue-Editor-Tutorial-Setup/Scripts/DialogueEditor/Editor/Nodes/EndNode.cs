using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class EndNode : BaseNode
{
    private EndData _endData = new EndData();
    public EndData EndData { get => _endData; set => _endData = value; }
    public EndNode(){}

    public EndNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        base.editorWindow = editorWindow;
        base.graphView = graphView;

        StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/EndNodeStyle");
        styleSheets.Add(styleSheet);
        
        title = "End";
        SetPosition(new Rect(position, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();

        AddInputPort("Input", Port.Capacity.Multi);

        MakeMainContainer();
    }
    
    private void MakeMainContainer()
    {
        EnumField enumField = GetNewEnumFieldEndNodeType(_endData.endNodeType);

        mainContainer.Add(enumField);
    }

    public override void LoadValueInToField()
    {
        EndData.endNodeType.EnumField?.SetValueWithoutNotify(_endData.endNodeType.value);
    }
}
