using System;
using System.Collections.Generic;
using Labyrinth.Pathfinding;
using UnityEngine;
using Utils.Events;

namespace CharacterScripts
{
    public class CharacterPathfindingMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;

        private List<Vector3> _pathVectorList;
        
        private int _currentPathIndex;

        private bool _isActive = true;

        private void OnEnable()
        {
            EventManager.OnLMBPressed.OnEventRaised += SetTargetPosition;
        }

        private void OnDisable()
        {
            EventManager.OnLMBPressed.OnEventRaised -= SetTargetPosition;
        }

        private void Update()
        {
            if (!_isActive) return;
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (_pathVectorList == null) return;

            Vector3 targetPosition = _pathVectorList[_currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > _speed * Time.deltaTime * Pathfinding.Instance.GetGrid().GetCellSize()) {
                Vector3 moveDirection = (targetPosition - transform.position).normalized;
                transform.position += moveDirection * _speed * Time.deltaTime;
            }
            else {
                _currentPathIndex++;
                if (_currentPathIndex >= _pathVectorList.Count) {
                    StopMoving();
                }
            }
        }
        
        private void StopMoving()
        {
            _pathVectorList = null;
        }

        private void ToggleActiveStatus()
        {
            _isActive = !_isActive;
        }
        
        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetTargetPosition(Vector3 targetPosition)
        {
            if (!_isActive) return;
            _currentPathIndex = 0;
            _pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);
            
            if (_pathVectorList != null && _pathVectorList.Count > 1) {
                _pathVectorList.RemoveAt(0);
            }
        }
    }
}
