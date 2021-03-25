using System;
using UnityEngine;
using Utils.Events;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public void SetGridWidth(string value)
        {
            int width = 0;
            try {
                width = int.Parse(value);}
            catch (Exception e) {
                Console.WriteLine(e);
            }
            EventManager.OnGridWidthChanged.OnEventRaised?.Invoke(width);
        }

        public void SetGridHeight(string value)
        {
            int height = 0;
            try {
                height = int.Parse(value); }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            EventManager.OnGridHeightChanged.OnEventRaised?.Invoke(height);
        }

        public void GenerateGrid()
        {
            EventManager.OnGenerateGrid.OnEventRaised?.Invoke();
        }

        public void SaveLevel()
        {
            EventManager.OnSaveLevel.OnEventRaised?.Invoke();
        }

        public void LoadLevel()
        {
            EventManager.OnLoadLevel.OnEventRaised?.Invoke();
        }
    }
}
