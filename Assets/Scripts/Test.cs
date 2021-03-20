using System;
using System.Collections;
using System.Collections.Generic;
using GridMap;
using UnityEngine;
using Utils;

public class Test : MonoBehaviour
{
    [SerializeField] private HeatMapVisual _heatMapVisual;
    private Grid<HeatMapGridObject> _grid;

    private void Start()
    {
        _grid = new Grid<HeatMapGridObject>(10, 10, 10f, Vector3.zero, (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
        _heatMapVisual.SetGrid(_grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            HeatMapGridObject heatMapGridObject = _grid.GetGridObject(position);
            heatMapGridObject?.AddValue(5);
        }

        if (Input.GetMouseButtonDown(1)) {
            Debug.Log(_grid.GetGridObject(UtilsClass.GetMouseWorldPosition()));
        }
    }
}

public class HeatMapGridObject
{
    private const int MIN = 0;
    private const int MAX = 100;

    private Grid<HeatMapGridObject> _grid;
    private int _x;
    private int _y;
    private int _value;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
    {
        _grid = grid;
        _x = x;
        _y = y;
    }

    public void AddValue(int addValue)
    {
        _value += addValue;
        _value = Mathf.Clamp(_value, MIN, MAX);
        _grid.TriggerGridObjectChanged(_x, _y);
    }

    public float GetValueNormalized()
    {
        return (float) _value / MAX;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}