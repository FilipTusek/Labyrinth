using System;
using UnityEngine;
using Utils;
using Utils.Events;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButton(0)) {
                EventManager.OnLMBPressed.OnEventRaised?.Invoke(UtilsClass.GetMouseWorldPosition());
            }

            if (Input.GetMouseButtonDown(1)) {
                EventManager.OnRMBPressed.OnEventRaised?.Invoke(UtilsClass.GetMouseWorldPosition());
            }
        }
    }
}
