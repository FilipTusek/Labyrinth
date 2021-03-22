using GridMap;
using UnityEngine;

namespace Labyrinth.Pathfinding
{
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
    }
}