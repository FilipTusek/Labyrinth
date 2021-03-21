using UnityEngine;

namespace GridMap
{
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
}