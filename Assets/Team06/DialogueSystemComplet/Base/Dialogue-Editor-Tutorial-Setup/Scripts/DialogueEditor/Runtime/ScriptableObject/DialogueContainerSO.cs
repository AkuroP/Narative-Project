using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Team06
{


    [CreateAssetMenu(menuName = "Dialogue/New Dialogue")]
    [System.Serializable]
    public class DialogueContainerSO : ScriptableObject
    {
        public List<NodeLinkDatas> nodeLinkDatas = new List<NodeLinkDatas>();

        public List<StartData> startDatas = new List<StartData>();
        public List<DialogueData> dialogueDatas = new List<DialogueData>();
        public List<EventData> eventDatas = new List<EventData>();
        public List<BranchData> branchDatas = new List<BranchData>();
        public List<ChoiceData> choiceDatas = new List<ChoiceData>();
        public List<EndData> endDatas = new List<EndData>();

        public List<BaseData> AllDatas
        {
            get
            {
                List<BaseData> tmp = new List<BaseData>();
                tmp.AddRange(startDatas);
                tmp.AddRange(dialogueDatas);
                tmp.AddRange(eventDatas);
                tmp.AddRange(branchDatas);
                tmp.AddRange(choiceDatas);
                tmp.AddRange(endDatas);

                return tmp;
            }
        }

        [System.Serializable]
        public class NodeLinkDatas
        {
            public string baseNodeGuid;
            public string basePortName;
            public string targetNodeGuid;
            public string targetPortName;
        }

    }
}


