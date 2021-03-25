using System;
using GridMap;
using UnityEngine;

namespace Labyrinth.Pathfinding
{
    [Serializable]
    public class PathNode
    {
        private Grid<PathNode> _grid;
        public int X;
        public int Y;

        public int GCost;
        public int HCost;
        public int FCost;

        public bool IsWalkable;
        public PathNode CameFromNode;

        public PathNode(Grid<PathNode> grid, int x, int y)
        {
            _grid = grid;
            X = x;
            Y = y;
            IsWalkable = true;
        }

        public void CalculateFCost()
        {
            FCost = GCost + HCost;
        }

        public void SetIsWalkable(bool isWalkable)
        {
            IsWalkable = isWalkable;
            _grid.TriggerGridObjectChanged(X, Y);
        }

        public override string ToString()
        {
            //return X + "," + Y;
            return "" + IsWalkable;
        }

        [Serializable]
        public class SaveObject
        {
            public bool IsWalkable;
            public int X;
            public int Y;
        }

        public SaveObject Save()
        {
            return new SaveObject
            {
                IsWalkable = IsWalkable,
                X = X,
                Y = Y
            };
        }

        public void Load(SaveObject saveObject)
        {
            IsWalkable = saveObject.IsWalkable;
        }
    }
}