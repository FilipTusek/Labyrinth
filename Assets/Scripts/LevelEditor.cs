using System;
using System.Collections;
using System.Collections.Generic;
using GridMap;
using Labyrinth.Pathfinding;
using Managers;
using UnityEngine;
using Utils.Events;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private PathfindingVisual _pathfindingVisual;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public float CellSize { get; private set; }

    private bool _isActive = false;

    private Pathfinding _pathfinding;

    private void OnEnable()
    {
        EventManager.OnLevelEditorToggle.OnEventRaised += ToggleActiveState;
        EventManager.OnGridWidthChanged.OnEventRaised += SetWidth;
        EventManager.OnGridHeightChanged.OnEventRaised += SetHeight;
        EventManager.OnGenerateGrid.OnEventRaised += GenerateGrid;
        EventManager.OnLMBPressed.OnEventRaised += SetPathNodeUnWalkable;
        EventManager.OnRMBPressed.OnEventRaised += SetPathNodeWalkable;
    }

    private void OnDisable()
    {
        EventManager.OnLevelEditorToggle.OnEventRaised -= ToggleActiveState;
        EventManager.OnGridWidthChanged.OnEventRaised -= SetWidth;
        EventManager.OnGridHeightChanged.OnEventRaised -= SetHeight;
        EventManager.OnGenerateGrid.OnEventRaised -= GenerateGrid;
        EventManager.OnLMBPressed.OnEventRaised -= SetPathNodeUnWalkable;
        EventManager.OnRMBPressed.OnEventRaised -= SetPathNodeWalkable;
    }

    private void Start()
    {
        Width = 10;
        Height = 10;
        CellSize = 10f;
        
        GenerateGrid();
    }

    private void ToggleActiveState()
    {
        _isActive = !_isActive;
    }

    private void SetWidth(int value)
    {
        Width = value;
    }

    private void SetHeight(int value)
    {
        Height = value;
    }

    private void GenerateGrid()
    {
        _pathfinding = new Pathfinding(Width, Height, CellSize);
        _pathfindingVisual.SetGrid(_pathfinding.GetGrid());
    }

    private void SetPathNodeWalkable(Vector3 worldPosition)
    {
        if (!_isActive) return;
        _pathfinding.GetGrid().GetXY(worldPosition, out int x, out int y);
        _pathfinding.GetNode(x, y).SetIsWalkable(true);
    }

    private void SetPathNodeUnWalkable(Vector3 worldPosition)
    {
        if (!_isActive) return;
        _pathfinding.GetGrid().GetXY(worldPosition, out int x, out int y);
        _pathfinding.GetNode(x, y)?.SetIsWalkable(false);
    }
}