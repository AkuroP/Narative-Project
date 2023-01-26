using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Team06
{
    

public class BranchNode : BaseNode
{
    private BranchData _branchData = new BranchData();
    public BranchData BranchData { get => _branchData; set => _branchData = value; }

    public BranchNode(){}

    public BranchNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        base.editorWindow = editorWindow;
        base.graphView = graphView;

        StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/BranchNodeStyle");
        styleSheets.Add(styleSheet);
        
        title = "Condition Branch";
        SetPosition(new Rect(position, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();
        
        AddInputPort("Input", Port.Capacity.Multi);
        AddOutputPort("True", Port.Capacity.Single);
        AddOutputPort("False", Port.Capacity.Single);

        TopButton();

    }

    private void TopButton()
    {
        ToolbarMenu Menu = new ToolbarMenu();
        Menu.text = "Add Condition";

        Menu.menu.AppendAction("String Event Condition", new Action<DropdownMenuAction>(x => AddCondition()));

        titleButtonContainer.Add(Menu);
    }

    public void AddCondition(EventDataStringCondition stringEvent = null)
    {
        AddStringConditionEventBuild(_branchData.stringConditions, stringEvent);
    }
    
}
}
