using System;
using System.Collections.Specialized;
using Labyrinth.Pathfinding;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace GridMap
{
    public class PathfindingVisual : MonoBehaviour
    {
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private Transform _tilesParent;
        
        [SerializeField] private Material _walkableMaterial;
        [SerializeField] private Material _unwalkableMaterial;

        private MeshRenderer[,] _instantiatedTileRenderers;

        private Grid<PathNode> _grid;

        public void SetGrid(Grid<PathNode> grid)
        {
            _grid = grid;
            CreateGridVisuals();

            _grid.OnGridValueChanged += OnGridValueChanged;
        }

        private void OnGridValueChanged(object sender, Grid<PathNode>.OnGridValueChangedEventArgs e)
        {
            UpdateTileVisual(e.X, e.Y);
        }

        private void UpdateTileVisual(int x, int y)
        {
            _instantiatedTileRenderers[x, y].material = _grid.GetGridObject(x, y).IsWalkable ? _walkableMaterial : _unwalkableMaterial;
        }

        private void CreateGridVisuals()
        {
            _instantiatedTileRenderers = new MeshRenderer[_grid.GetWidth(), _grid.GetHeight()];

            for (int x = 0; x < _grid.GetWidth(); x++) {
                for (int y = 0; y < _grid.GetHeight(); y++) {
                    Vector3 position = new Vector3(x * _grid.GetCellSize(), y * _grid.GetCellSize(), 0) + _tilePrefab.transform.localScale * _grid.GetCellSize() / 2;
                    var tile = Instantiate(_tilePrefab, position, quaternion.identity, _tilesParent).GetComponent<MeshRenderer>();
                    tile.transform.localScale = Vector2.one * _grid.GetCellSize();
                    _instantiatedTileRenderers[x, y] = tile;

                    tile.material = _grid.GetGridObject(x, y).IsWalkable ? _walkableMaterial : _unwalkableMaterial;
                }
            }
        }
    }
}