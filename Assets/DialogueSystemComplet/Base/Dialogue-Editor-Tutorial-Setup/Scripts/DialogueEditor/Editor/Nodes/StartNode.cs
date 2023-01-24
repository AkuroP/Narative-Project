using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class StartNode : BaseNode
{
    public StartNode(){}

    public StartNode(Vector2 _position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        base.editorWindow = editorWindow;
        base.graphView = graphView;
        
        StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/StartNodeStyle");
        styleSheets.Add(styleSheet);
        
        title = "Start";
        SetPosition(new Rect(_position, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();

        AddOutputPort("Output", Port.Capacity.Single);

        RefreshExpandedState();
        RefreshPorts();
        
    }
}
