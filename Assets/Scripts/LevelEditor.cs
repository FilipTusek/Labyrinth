using GridMap;
using Labyrinth.Pathfinding;
using UnityEngine;
using Utils.Events;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private PathfindingVisual _pathfindingVisual;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public float CellSize { get; private set; }

    private Vector2Int _minMaxGridSize;

    private Pathfinding _pathfinding;

    private void OnEnable()
    {
        EventManager.OnGridWidthChanged.OnEventRaised += SetWidth;
        EventManager.OnGridHeightChanged.OnEventRaised += SetHeight;
        EventManager.OnGenerateGrid.OnEventRaised += GenerateGrid;
        EventManager.OnLMBHeld.OnEventRaised += SetPathNodeUnWalkable;
        EventManager.OnRMBHeld.OnEventRaised += SetPathNodeWalkable;
        EventManager.OnSaveLevel.OnEventRaised += SaveLevelData;
    }

    private void OnDisable()
    {
        EventManager.OnGridWidthChanged.OnEventRaised -= SetWidth;
        EventManager.OnGridHeightChanged.OnEventRaised -= SetHeight;
        EventManager.OnGenerateGrid.OnEventRaised -= GenerateGrid;
        EventManager.OnLMBHeld.OnEventRaised -= SetPathNodeUnWalkable;
        EventManager.OnRMBHeld.OnEventRaised -= SetPathNodeWalkable;
        EventManager.OnSaveLevel.OnEventRaised -= SaveLevelData;
    }

    private void Start()
    {
        Width = 100;
        Height = 100;
        CellSize = 1f;

        GenerateGrid();
    }

    private void SetWidth(int value)
    {
        if (value >= _minMaxGridSize.x)
            Width = value;
    }

    private void SetHeight(int value)
    {
        if (value >= _minMaxGridSize.y)
            Height = value;
    }

    private void GenerateGrid()
    {
        _pathfinding = new Pathfinding(Width, Height, CellSize);
        _pathfindingVisual.SetGrid(_pathfinding.GetGrid());
        EventManager.OnGridGenerated.OnEventRaised?.Invoke();
    }

    private void SetPathNodeWalkable(Vector3 worldPosition)
    {
        _pathfinding.GetGrid().GetXY(worldPosition, out int x, out int y);
        _pathfinding.GetNode(x, y).SetIsWalkable(true);
    }

    private void SetPathNodeUnWalkable(Vector3 worldPosition)
    {
        _pathfinding.GetGrid().GetXY(worldPosition, out int x, out int y);
        _pathfinding.GetNode(x, y)?.SetIsWalkable(false);
    }

    private void SaveLevelData()
    {
        
    }
}