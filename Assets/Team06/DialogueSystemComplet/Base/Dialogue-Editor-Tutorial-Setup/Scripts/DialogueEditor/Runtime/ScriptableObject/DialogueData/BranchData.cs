using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team06
{
    [System.Serializable]
    public class BranchData : BaseData
    {
        public string trueGuidNode;
        public string falseGuidNode;
        public List<EventDataStringCondition> stringConditions = new List<EventDataStringCondition>();
    }
}
