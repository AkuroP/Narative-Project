using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Team06
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueEditorWindow _editorWindow;
        private DialogueGraphView _graphView;
        private Texture2D _iconImage;

        public void Configure(DialogueEditorWindow editorWindow, DialogueGraphView graphView)
        {
            _editorWindow = editorWindow;
            _graphView = graphView;

            _iconImage = new Texture2D(1, 1);
            _iconImage.SetPixel(0, 0, new Color(0, 0, 0, 0));
            _iconImage.Apply();

        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Dialogue Editor"), 0),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),

                AddNodeSearch("Start Node", new StartNode()),
                AddNodeSearch("Dialogue Node", new DialogueNode()),
                AddNodeSearch("Event Node", new EventNode()),
                AddNodeSearch("Branch Node", new BranchNode()),
                AddNodeSearch("Choice Node", new ChoiceNode()),
                AddNodeSearch("End Node", new EndNode()),
            };

            return tree;
        }


        private SearchTreeEntry AddNodeSearch(string nodeName, BaseNode baseNode)
        {
            SearchTreeEntry tmp = new SearchTreeEntry(new GUIContent(nodeName))
            {
                level = 2,
                userData = baseNode
            };

            return tmp;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            // Get mouse position on the screen.
            Vector2 mousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo(
                _editorWindow.rootVisualElement.parent, context.screenMousePosition - _editorWindow.position.position);

            // Use mouse position to calculator where it is in the graph view.
            Vector2 graphMousePosition = _graphView.contentViewContainer.WorldToLocal(mousePosition);

            return CheckForNodeType(searchTreeEntry, graphMousePosition);
        }

        private bool CheckForNodeType(SearchTreeEntry searchTreeEntry, Vector2 pos)
        {
            switch (searchTreeEntry.userData)
            {
                case StartNode node:
                    _graphView.AddElement(_graphView.CreateStartNode(pos));
                    return true;
                case DialogueNode node:
                    _graphView.AddElement(_graphView.CreateDialogueNode(pos));
                    return true;
                case EventNode node:
                    _graphView.AddElement(_graphView.CreateEventNode(pos));
                    return true;
                case BranchNode node:
                    _graphView.AddElement(_graphView.CreateBranchNode(pos));
                    return true;
                case ChoiceNode node:
                    _graphView.AddElement(_graphView.CreateChoiceNode(pos));
                    return true;
                case EndNode node:
                    _graphView.AddElement(_graphView.CreateEndNode(pos));
                    return true;
                default:
                    break;
            }

            return false;
        }
    }
}
