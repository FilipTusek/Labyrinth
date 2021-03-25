using System;
using System.Collections;
using System.Collections.Generic;
using GridMap;
using Labyrinth.Pathfinding;
using UnityEngine;
using Utils.Events;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _zoomSpeed = 5f;

    private Camera _camera;

    private Vector2 _minMaxZoom;
    private float _currentZoom;

    private void OnEnable()
    {
        EventManager.OnMoveCamera.OnEventRaised += MoveCamera;
        EventManager.OnMouswheelScrolled.OnEventRaised += ZoomCamera;
        EventManager.OnGridGenerated.OnEventRaised += SetMinMaxZoom;
        EventManager.OnGridGenerated.OnEventRaised += PositionCameraToLowerLeftGridCorner;
    }

    private void OnDisable()
    {
        EventManager.OnMoveCamera.OnEventRaised -= MoveCamera;
        EventManager.OnMouswheelScrolled.OnEventRaised -= ZoomCamera;
        EventManager.OnGridGenerated.OnEventRaised -= SetMinMaxZoom;
        EventManager.OnGridGenerated.OnEventRaised -= PositionCameraToLowerLeftGridCorner;
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        _currentZoom = _camera.orthographicSize;
    }

    private void SetMinMaxZoom()
    {
        Pathfinding pathfinding = Pathfinding.Instance;
        Vector2 minXY = pathfinding.GetGrid().GetWorldPosition(0, 0);
        Vector2 maxXY = pathfinding.GetGrid().GetWorldPosition(pathfinding.GetGrid().GetWidth(), pathfinding.GetGrid().GetHeight());

        var distanceX = (maxXY.x - minXY.x) * pathfinding.GetGrid().GetCellSize();
        var distanceY = (maxXY.y - minXY.y) * pathfinding.GetGrid().GetCellSize();
        var aspectRatio = (float)Screen.width / Screen.height;

        _minMaxZoom = new Vector2(15, distanceX / distanceY >= aspectRatio ? distanceY / 2 * aspectRatio + pathfinding.GetGrid().GetCellSize() : distanceY / 2 + pathfinding.GetGrid().GetCellSize() * 2);
    }

    private void PositionCameraToLowerLeftGridCorner()
    {
        transform.position = new Vector3(_camera.orthographicSize + 10, _camera.orthographicSize, -10);
    }

    private void ZoomCamera(float value)
    {
        _currentZoom += Time.deltaTime * value > 0 ? -_zoomSpeed : _zoomSpeed;
        _currentZoom = Mathf.Clamp(_currentZoom, _minMaxZoom.x, _minMaxZoom.y);
        _camera.orthographicSize = _currentZoom;
    }

    private void MoveCamera(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * _moveSpeed;
    }
}