using System;
using UnityEngine;

namespace Core.UI
{
    public class ContentToggle : MonoBehaviour
    {
        [SerializeField] private GameObject _on;
        [SerializeField] private GameObject _off;
        [SerializeField] private State _defaultState;

        private void Awake()
        {
            switch (_defaultState)
            {
                case State.On:
                    SetOn();
                    break;
                case State.Off:
                    SetOff();
                    break;
            }
        }

        public void SetOn()
        {
            _on.SetActive(true);
            _off.SetActive(false);
        }

        public void SetOff()
        {
            _on.SetActive(false);
            _off.SetActive(true);
        }

        private enum State
        {
            On,
            Off
        }
    }
}