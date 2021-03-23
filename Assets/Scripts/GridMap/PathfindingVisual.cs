using System;
using System.Collections.Generic;
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

        [SerializeField] private Color _walkableColor;
        [SerializeField] private Color _unwalkableColor;

        private SpriteRenderer[,] _instantiatedTileRenderers;
        private Queue<SpriteRenderer> _tileMeshRenderersQueue = new Queue<SpriteRenderer>();

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
            _instantiatedTileRenderers[x, y].color = _grid.GetGridObject(x, y).IsWalkable ? _walkableColor : _unwalkableColor;
        }

        private void CreateGridVisuals()
        {
            EnqueueInstantiatedTiles();
            _instantiatedTileRenderers = new SpriteRenderer[_grid.GetWidth(), _grid.GetHeight()];

            for (int x = 0; x < _grid.GetWidth(); x++) {
                for (int y = 0; y < _grid.GetHeight(); y++) {
                    Vector3 position = new Vector3(x * _grid.GetCellSize(), y * _grid.GetCellSize(), 0) + _tilePrefab.transform.localScale * _grid.GetCellSize() / 2;
                    SpriteRenderer tileSpriteRenderer = _tileMeshRenderersQueue.Count > 0 ? _tileMeshRenderersQueue.Dequeue() : Instantiate(_tilePrefab, _tilesParent).GetComponent<SpriteRenderer>();
                    Transform tileTransform = tileSpriteRenderer.transform;
                    tileSpriteRenderer.gameObject.SetActive(true);
                    tileTransform.localScale = Vector2.one * _grid.GetCellSize();
                    tileTransform.position = position;
                    _instantiatedTileRenderers[x, y] = tileSpriteRenderer;
                    tileSpriteRenderer.color = _grid.GetGridObject(x, y).IsWalkable ? _walkableColor : _unwalkableColor;
                }
            }
        }

        private void EnqueueInstantiatedTiles()
        {
            if (_instantiatedTileRenderers == null) return;
            for (int x = 0; x < _instantiatedTileRenderers.GetLength(0); x++) {
                for (int y = 0; y < _instantiatedTileRenderers.GetLength(1); y++) {
                    _instantiatedTileRenderers[x,y].gameObject.SetActive(false);
                    _tileMeshRenderersQueue.Enqueue(_instantiatedTileRenderers[x,y]);
                }
            }
        }
    }
}