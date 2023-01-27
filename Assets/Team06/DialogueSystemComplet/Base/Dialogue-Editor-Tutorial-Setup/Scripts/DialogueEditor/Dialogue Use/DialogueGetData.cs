using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team06
{
    public class DialogueGetData : MonoBehaviour
    {
        [SerializeField] protected DialogueContainerSO dialogueContainer;

        protected BaseData GetNodeByGuid(string targetNodeGuid)
        {
            return dialogueContainer.AllDatas.Find(node => node.nodeGuid == targetNodeGuid);
        }

        protected BaseData GetNodeByNodePort(DialogueDataPort nodePort)
        {
            return dialogueContainer.AllDatas.Find(node => node.nodeGuid == nodePort.inputGuid);
        }

        protected BaseData GetNextNode(BaseData baseNodeData)
        {
            DialogueContainerSO.NodeLinkDatas nodeLinkData =
                dialogueContainer.nodeLinkDatas.Find(egde => egde.baseNodeGuid == baseNodeData.nodeGuid);

            return GetNodeByGuid(nodeLinkData.targetNodeGuid);
        }
    }
}
