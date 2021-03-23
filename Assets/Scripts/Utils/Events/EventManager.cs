using TMPro;
using UnityEngine;
using Event = Utils.Events;

namespace Utils.Events
{
    public class EventManager
    {
        public static readonly EventSingle<Vector3> OnLMBPressed = new EventSingle<Vector3>();
        public static readonly EventSingle<Vector3> OnRMBPressed = new EventSingle<Vector3>();
        public static readonly EventSingle<Vector3> OnLMBHeld = new EventSingle<Vector3>();
        public static readonly EventSingle<Vector3> OnRMBHeld = new EventSingle<Vector3>();

        public static readonly EventSingle<int> OnGridWidthChanged = new EventSingle<int>();
        public static readonly EventSingle<int> OnGridHeightChanged = new EventSingle<int>();
        
        public static readonly Event OnLevelEditorToggle = new Event();
        public static readonly Event OnCharacterStatusToggle = new Event();
        public static readonly Event OnGenerateGrid = new Event();
    }
}
