using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team06
{
    public class Transition : MonoBehaviour
    {
        public void DisableTransition()
        {
            this.gameObject.SetActive(false);       
        }
    }

}
