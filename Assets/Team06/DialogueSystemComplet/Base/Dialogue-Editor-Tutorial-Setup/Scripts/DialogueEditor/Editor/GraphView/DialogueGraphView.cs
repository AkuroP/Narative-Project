using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Team06
{

    public class DialogueGraphView : GraphView
    {
        private string _graphViewStyleSheet = "USS/GraphView/GraphViewStyle";
        private DialogueEditorWindow editorWindow;
        private NodeSearchWindow _searchWindow;

        /// <summary>
        /// Add a search window to graph view.
        /// </summary>
        public DialogueGraphView(DialogueEditorWindow editorWindow)
        {
            this.editorWindow = editorWindow;
            StyleSheet tmpStyleSheet = Resources.Load<StyleSheet>(_graphViewStyleSheet);
            styleSheets.Add(tmpStyleSheet);

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());

            GridBackground grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddSearchWindow();
        }

        private void AddSearchWindow()
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            _searchWindow.Configure(editorWindow, this);
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            Port startPortView = startPort;

            ports.ForEach((port) =>
            {
                Port portView = port;

                if (startPortView != portView && startPortView.node != portView.node &&
                    startPortView.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }

        /// <summary>
        /// Reload the current selected language.
        /// Normally used when changing language.
        /// </summary>
        public void ReloadLanguage()
        {
            List<BaseNode> allNodes = nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();
            foreach (var node in allNodes)
            {
                node.ReloadLanguage();
            }
        }

        // Make Node's -----------------------------------------------------------------------------------

        /// <summary>
        /// Make new Start Node and set it's position.
        /// </summary>
        /// <param name="position">position of where to place the node</param>
        /// <returns>Start Node</returns>
        public StartNode CreateStartNode(Vector2 position)
        {
            return new StartNode(position, editorWindow, this);
        }

        /// <summary>
        /// Make new End Node and set it's position.
        /// </summary>
        /// <param name="position">position of where to place the node</param>
        /// <returns>End Node</returns>
        public EndNode CreateEndNode(Vector2 position)
        {
            return new EndNode(position, editorWindow, this);
        }

        /// <summary>
        /// Make new Event Node and set it's position.
        /// </summary>
        /// <param name="position">position of where to place the node</param>
        /// <returns>Event Node</returns>
        public EventNode CreateEventNode(Vector2 position)
        {
            return new EventNode(position, editorWindow, this);
        }

        /// <summary>
        /// Make new Dialogue Node and set it's position.
        /// </summary>
        /// <param name="position">position of where to place the node</param>
        /// <returns>Dialogue Node</returns>
        public DialogueNode CreateDialogueNode(Vector2 position)
        {
            return new DialogueNode(position, editorWindow, this);
        }

        /// <summary>
        /// Make new Branch Node and set it's position.
        /// </summary>
        /// <param name="position">position of where to place the node</param>
        /// <returns>Branch Node</returns>
        public BranchNode CreateBranchNode(Vector2 position)
        {
            return new BranchNode(position, editorWindow, this);
        }

        /// <summary>
        /// Make new Choice Node and set it's position.
        /// </summary>
        /// <param name="position">position of where to place the node</param>
        /// <returns>Choice Node</returns>
        public ChoiceNode CreateChoiceNode(Vector2 position)
        {
            return new ChoiceNode(position, editorWindow, this);
        }
    }
}
