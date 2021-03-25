using System.Collections.Generic;
using GridMap;
using Labyrinth.Pathfinding;
using SaveLoadScripts;
using UnityEngine;
using Utils.Events;

public class GridManager : MonoBehaviour
{
    [SerializeField] private PathfindingVisual _pathfindingVisual;

    [SerializeField] private bool _levelEditorActive = false; //Can be handled much better, but this will suffice for this use case
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
        EventManager.OnSaveLevel.OnEventRaised += Save;
        EventManager.OnLoadLevel.OnEventRaised += Load;

        if (!_levelEditorActive) return;
        EventManager.OnLMBHeld.OnEventRaised += SetPathNodeUnWalkable;
        EventManager.OnRMBHeld.OnEventRaised += SetPathNodeWalkable;
    }

    private void OnDisable()
    {
        EventManager.OnGridWidthChanged.OnEventRaised -= SetWidth;
        EventManager.OnGridHeightChanged.OnEventRaised -= SetHeight;
        EventManager.OnGenerateGrid.OnEventRaised -= GenerateGrid;
        EventManager.OnSaveLevel.OnEventRaised -= Save;
        EventManager.OnLoadLevel.OnEventRaised -= Load;
        
        if (!_levelEditorActive) return;
        EventManager.OnLMBHeld.OnEventRaised -= SetPathNodeUnWalkable;
        EventManager.OnRMBHeld.OnEventRaised -= SetPathNodeWalkable;
    }

    private void Start()
    {
        Load();
    }

    private void Save()
    {
        List<PathNode.SaveObject> pathNodeSaveObjectList = new List<PathNode.SaveObject>();
        for (int x = 0; x < _pathfinding.GetGrid().GetWidth(); x++) {
            for (int y = 0; y < _pathfinding.GetGrid().GetHeight(); y++) {
                PathNode pathNode = _pathfinding.GetGrid().GetGridObject(x, y);
                pathNodeSaveObjectList.Add(pathNode.Save());
            }
        }

        SaveObject saveObject = new SaveObject
        {
            Width = _pathfinding.GetGrid().GetWidth(), Height = _pathfinding.GetGrid().GetHeight(), CellSize = _pathfinding.GetGrid().GetCellSize(),
            PathNodeSaveObjectArray = pathNodeSaveObjectList.ToArray()
        };

        SaveSystem.SaveObject(saveObject);
    }

    private void Load()
    {
        SaveObject saveObject = SaveSystem.LoadMostRecentObject<SaveObject>();
        if (saveObject == null) {
            Width = 100;
            Height = 100;
            CellSize = 1f;
            GenerateGrid();
            SetPathfindingVisualGrid();
            return;
        }

        Width = saveObject.Width;
        Height = saveObject.Height;
        CellSize = saveObject.CellSize;
        GenerateGrid();
        foreach (var pathNodeSaveObject in saveObject.PathNodeSaveObjectArray) {
            PathNode pathNode = _pathfinding.GetGrid().GetGridObject(pathNodeSaveObject.X, pathNodeSaveObject.Y);
            pathNode.Load(pathNodeSaveObject);
        }

        SetPathfindingVisualGrid();
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

    private void SetPathfindingVisualGrid()
    {
        _pathfindingVisual.SetGrid(_pathfinding.GetGrid());
    }

    public class SaveObject
    {
        public int Width;
        public int Height;
        public float CellSize;
        public PathNode.SaveObject[] PathNodeSaveObjectArray;
    }
}