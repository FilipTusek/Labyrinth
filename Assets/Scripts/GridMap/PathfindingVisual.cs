using System;
using System.Collections.Specialized;
using Labyrinth.Pathfinding;
using UnityEngine;
using Utils;

namespace GridMap
{
    public class PathfindingVisual : MonoBehaviour
    {
        private Grid<PathNode> _grid;
        private Mesh _mesh;
        private bool _updateMesh;

        private void Awake()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
        }

        public void SetGrid(Grid<PathNode> grid)
        {
            _grid = grid;
            UpdateVisual();

            _grid.OnGridValueChanged += OnGridValueChanged;
        }

        private void OnGridValueChanged(object sender, Grid<PathNode>.OnGridValueChangedEventArgs e)
        {
            _updateMesh = true;
        }

        private void LateUpdate()
        {
            if (_updateMesh) {
                _updateMesh = false;
                UpdateVisual();
            }
        }

        private void UpdateVisual()
        {
            MeshUtils.CreateEmptyMeshArrays(_grid.GetWidth() * _grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

            for (int x = 0; x < _grid.GetWidth(); x++) {
                for (int y = 0; y < _grid.GetHeight(); y++) {
                    int index = x * _grid.GetHeight() + y;
                    Vector3 quadSize = new Vector3(1, 1) * _grid.GetCellSize();

                    PathNode pathNode = _grid.GetGridObject(x, y);

                    if (pathNode.IsWalkable)
                        quadSize = Vector3.zero;
                    
                    MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, _grid.GetWorldPosition(x, y) + quadSize * 0.5f, 0f, quadSize, Vector2.zero, Vector2.one);
                }
            }

            _mesh.vertices = vertices;
            _mesh.uv = uv;
            _mesh.triangles = triangles;
        }
    }
}
