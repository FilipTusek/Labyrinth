using System;
using System.Collections.Specialized;
using UnityEngine;
using Utils;

namespace GridMap
{
    public class HeatMapVisual : MonoBehaviour
    {
        private Grid<HeatMapGridObject> _grid;
        private Mesh _mesh;
        private bool _updateMesh;

        private void Awake()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
        }

        public void SetGrid(Grid<HeatMapGridObject> grid)
        {
            _grid = grid;
            UpdateHeatMapVisual();

            _grid.OnGridValueChanged += OnGridValueChanged;
        }

        private void OnGridValueChanged(object sender, Grid<HeatMapGridObject>.OnGridValueChangedEventArgs e)
        {
            _updateMesh = true;
        }

        private void LateUpdate()
        {
            if (_updateMesh) {
                _updateMesh = false;
                UpdateHeatMapVisual();
            }
        }

        private void UpdateHeatMapVisual()
        {
            MeshUtils.CreateEmptyMeshArrays(_grid.GetWidth() * _grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

            for (int x = 0; x < _grid.GetWidth(); x++) {
                for (int y = 0; y < _grid.GetHeight(); y++) {
                    int index = x * _grid.GetHeight() + y;
                    Vector3 quadSize = new Vector3(1, 1) * _grid.GetCellSize();

                    HeatMapGridObject gridObject = _grid.GetGridObject(x, y);
                    float gridValueNormalized = gridObject.GetValueNormalized();
                    Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);
                    MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, _grid.GetWorldPosition(x, y) + quadSize * 0.5f, 0f, quadSize, gridValueUV, gridValueUV);
                }
            }

            _mesh.vertices = vertices;
            _mesh.uv = uv;
            _mesh.triangles = triangles;
        }
    }
}
