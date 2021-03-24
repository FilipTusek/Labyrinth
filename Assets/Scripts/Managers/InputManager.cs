using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
using Utils.Events;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private void Update()
        {
            if(EventSystem.current.IsPointerOverGameObject()) return;
            
            if (Input.GetMouseButton(0)) {
                EventManager.OnLMBHeld.OnEventRaised?.Invoke(UtilsClass.GetMouseWorldPosition());
            }

            if (Input.GetMouseButton(1)) {
                EventManager.OnRMBHeld.OnEventRaised?.Invoke(UtilsClass.GetMouseWorldPosition());
            }
            
            if (Input.GetMouseButtonDown(0)) {
                EventManager.OnLMBPressed.OnEventRaised?.Invoke(UtilsClass.GetMouseWorldPosition());
            }

            if (Input.GetMouseButtonDown(1)) {
                EventManager.OnRMBPressed.OnEventRaised?.Invoke(UtilsClass.GetMouseWorldPosition());
            }

            if (Input.mouseScrollDelta.y != 0) {
                EventManager.OnMouswheelScrolled.OnEventRaised?.Invoke(Input.mouseScrollDelta.y);
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                EventManager.OnMoveCamera.OnEventRaised?.Invoke(Vector3.up);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                EventManager.OnMoveCamera.OnEventRaised?.Invoke(Vector3.left);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                EventManager.OnMoveCamera.OnEventRaised?.Invoke(Vector3.down);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                EventManager.OnMoveCamera.OnEventRaised?.Invoke(Vector3.right);
            }
        }
    }
}
