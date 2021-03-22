using System;
using System.Collections;
using System.Collections.Generic;
using GridMap;
using Labyrinth.Pathfinding;
using UnityEngine;
using Utils;

public class PathfindingTest : MonoBehaviour
{
    [SerializeField] private PathfindingVisual _pathfindingVisual;
    private Pathfinding _pathfinding;
    
    private void Start()
    {
        _pathfinding = new Pathfinding(10, 10, 10);
        _pathfindingVisual.SetGrid(_pathfinding.GetGrid());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            _pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = _pathfinding.FindPath(0, 0, x, y);
            if (path != null) {
                for (int i = 0; i < path.Count - 1; i++) {
                    Debug.DrawLine(new Vector3(path[i].X, path[i].Y) * 10f + Vector3.one * 5f, new Vector3(path[i+1].X, path[i+1].Y) * 10f + Vector3.one * 5f, Color.green, 5);
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            _pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            _pathfinding.GetNode(x,y).SetIsWalkable(!_pathfinding.GetNode(x,y).IsWalkable);
        }
    }
}
