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
                EventManager.OnLMBHeld.OnEventRaised?.Invoke(UtilsClass.GetMouseWorldPosition());
            }

            if (Input.GetMouseButton(1)) {
                EventManager.OnRMBHeld.OnEventRaised?.Invoke(UtilsClass.GetMouseWorldPosition());
            }
            
            if (Input.GetMouseButtonUp(0)) {
                EventManager.OnLMBPressed.OnEventRaised?.Invoke(UtilsClass.GetMouseWorldPosition());
            }

            if (Input.GetMouseButtonUp(1)) {
                EventManager.OnRMBPressed.OnEventRaised?.Invoke(UtilsClass.GetMouseWorldPosition());
            }
        }
    }
}
