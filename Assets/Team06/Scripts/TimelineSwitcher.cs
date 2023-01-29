using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Team06 
{
    
    public class TimelineSwitcher : MonoBehaviour {
        public bool canSwitch;
        private Vector3 _startPosition;
        private GameObject[] _timelines;
        private int _timelineIndex;
        public PostProcess grayScale;

        [Range(0, 1)] public float offset;

        private TeamSixPlayer player;


        private void Start() {
            _timelines = new GameObject[2];

            _timelines[0] = transform.GetChild(0).gameObject;
            _timelines[1] = transform.GetChild(1).gameObject;
            player = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<TeamSixPlayer>();
        }

        void Update() {
            if(!canSwitch)return;
            foreach (Touch touch in Input.touches) {
                if (touch.phase == TouchPhase.Began)
                    _startPosition = touch.position;
                if (touch.phase == TouchPhase.Ended) {
                    Vector3 direction = ((Vector3)touch.position - _startPosition).normalized;

                    if (direction.y >= 0 && direction.y >= Vector3.up.y - offset) {
                        //Debug.Log("UUUPPPP");

                        if (_timelineIndex == 0)
                        {
                            player.playerAnim.SetTrigger("Switch");
                            _timelineIndex = 1;
                            _timelines[_timelineIndex].SetActive(true);
                            _timelines[_timelineIndex == 1 ? 0 : 1].SetActive(false);

                        }

                    }

                    if (direction.y <= 0 && direction.y <= (Vector3.up.y - offset) * -1) {
                        //Debug.Log("dooown");

                        if (_timelineIndex == 1) {
                            player.playerAnim.SetTrigger("Switch");
                            _timelineIndex = 0;
                            _timelines[_timelineIndex].SetActive(true);
                            _timelines[_timelineIndex == 0 ? 1 : 0].SetActive(false);
                        }
                    }
                    player.StopAllCoroutines();
                }
            }

            if (_timelineIndex == 0)
            {
                grayScale.enabled = false;
            }
            else if(_timelineIndex == 1)
            {
                grayScale.enabled = true;
            }

        }
    }
}