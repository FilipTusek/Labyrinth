using System;
using System.Security.Cryptography.X509Certificates;
using UnityEditorInternal;
using UnityEngine;
using Utils;

namespace GridMap
{
    public class Grid<TGridObject>
    {
        public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int X;
            public int Y;
        }

        private int _width;
        private int _height;
        private float _cellSize;
        private Vector3 _originPosition;
        private TGridObject[,] _gridArray;

        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _originPosition = originPosition;

            _gridArray = new TGridObject[_width, _height];

            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    _gridArray[x, y] = createGridObject(this, x, y);
                }
            }

            bool showDebug = false;
            if (showDebug) {
                TextMesh[,] debugTextArray = new TextMesh[_width, _height];

                for (int x = 0; x < _gridArray.GetLength(0); x++) {
                    for (int y = 0; y < _gridArray.GetLength(1); y++) {
                        debugTextArray[x, y] = UtilsClass.CreateWorldText(_gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 20, Color.white,
                            TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    }
                }

                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

                OnGridValueChanged += (sender, eventArgs) => { debugTextArray[eventArgs.X, eventArgs.Y].text = _gridArray[eventArgs.X, eventArgs.Y]?.ToString(); };
            }
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize + _originPosition;
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition.x - _originPosition.x) / _cellSize);
            y = Mathf.FloorToInt((worldPosition.y - _originPosition.y) / _cellSize);
        }

        public void SetGridObject(int x, int y, TGridObject value)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height) {
                _gridArray[x, y] = value;
                OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs() {X = x, Y = y});
            }
        }

        public void TriggerGridObjectChanged(int x, int y)
        {
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs() {X = x, Y = y});
        }
        
        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetGridObject(x, y, value);
        }

        public TGridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
                return _gridArray[x, y];
            else
                return default;
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        public float GetCellSize()
        {
            return _cellSize;
        }

        public TGridObject[,] GetGridArray()
        {
            return _gridArray;
        }
    }
}