﻿using System.Collections.Generic;
using GridMap;
using Unity.Mathematics;
using UnityEngine;

namespace Labyrinth.Pathfinding
{
    public class Pathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;
        
        private Grid<PathNode> _grid;
        private List<PathNode> _openList;
        private List<PathNode> _closedList;
        
        public Pathfinding(int width, int height)
        {
            _grid = new Grid<PathNode>(width, height, 10f, Vector3.zero, (Grid<PathNode> _grid, int x, int y) => new PathNode(_grid, x, y));
        }

        public Grid<PathNode> GetGrid()
        {
            return _grid;
        }
        
        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = _grid.GetGridObject(startX, startY);
            PathNode endNode = _grid.GetGridObject(endX, endY);
            
            _openList = new List<PathNode> {startNode};
            _closedList = new List<PathNode>();

            for (int x = 0; x < _grid.GetWidth(); x++) {
                for (int y = 0; y < _grid.GetHeight(); y++) {
                    PathNode pathNode = _grid.GetGridObject(x, y);
                    pathNode.GCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.CameFromNode = null;
                }
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (_openList.Count > 0) {
                PathNode currentNode = GetLowestFCostNode(_openList);
                if (currentNode == endNode)
                    return CalculatePath(endNode);

                _openList.Remove(currentNode);
                _closedList.Add(currentNode);

                foreach (var neighbourNode in GetNeighbourList(currentNode)) {
                    if(_closedList.Contains(neighbourNode)) continue;
                    if (!neighbourNode.IsWalkable) {
                        _closedList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.GCost) {
                        neighbourNode.CameFromNode = currentNode;
                        neighbourNode.GCost = tentativeGCost;
                        neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();
                        
                        if(!_openList.Contains(neighbourNode)) _openList.Add(neighbourNode);
                    }
                }
            }

            return null;
        }

        public PathNode GetNode(int x, int y)
        {
            return _grid.GetGridObject(x, y);
        }
        
        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            if (currentNode.X - 1 >= 0) {
                //Left
                neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y));
                //Left down
                if(currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));
                //Left up
                if(currentNode.Y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
            }

            if (currentNode.X + 1 < _grid.GetWidth()) {
                //Right
                neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y));
                //Right down
                if(currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));
                //Right up
                if(currentNode.Y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
            }
            
            //Down
            if(currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X, currentNode.Y - 1));
            //Up
            if(currentNode.Y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.X, currentNode.Y + 1));

            return neighbourList;
        }
        
        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode currentNode = endNode;
            while (currentNode.CameFromNode != null) {
                path.Add(currentNode.CameFromNode);
                currentNode = currentNode.CameFromNode;
            }

            path.Reverse();
            return path;
        }
        
        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.X - b.X);
            int yDistance = Mathf.Abs(a.Y - b.Y);
            int remaining = Mathf.Abs(xDistance - yDistance);

            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];
            for (int i = 1; i < pathNodeList.Count; i++) {
                if (pathNodeList[i].FCost < lowestFCostNode.FCost)
                    lowestFCostNode = pathNodeList[i];
            }

            return lowestFCostNode;
        }
    }
}
