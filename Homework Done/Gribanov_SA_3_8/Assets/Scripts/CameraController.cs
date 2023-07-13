using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AI
{
    public class CameraController : MonoBehaviour
    {
        private ZigguratControls _controls;

        [SerializeField]
        private WindowValuesHandler _window;


        [Space, SerializeField, Min(1)]
        private float _movementSpeed = 1;
        [SerializeField, Min(1)]
        private float _rotationSpeed = 5;

        //If the cursor should be visible = true. If it shouldn't be = false (also enables and disables camera movement and rotation)
        private bool _isCursor = false;

        private void Awake()
        {
            _controls = new ZigguratControls();
        }

        private void OnEnable()
        {
            _controls.Enable();
            _controls.Camera.Switch.started += OnSwitch;
        }

        private void OnDisable()
        {
            _controls.Camera.Switch.started -= OnSwitch;
            _controls.Disable();
        }

        private void OnDestroy()
        {
            _controls.Dispose();
        }

        private void LateUpdate()
        {
            OnRotation();
            OnMovement();
            
        }

        private void OnMovement()
        {
            if (_isCursor) return;
            var translation = _controls.Camera.Movement.ReadValue<Vector2>() * _movementSpeed;
            transform.position += (transform.forward * translation.y) + (transform.right * translation.x);
        }

        private void OnRotation()
        {
            if (_isCursor) return;
            var rotation = _controls.Camera.Rotation.ReadValue<Vector2>() * _rotationSpeed;
            /*
            var quaternion = transform.rotation;
            quaternion.x = -rotation.y;
            quaternion.y = rotation.x;
            quaternion.z = 0;
            transform.rotation = quaternion;
            */
            //
            var euler = transform.eulerAngles;
            euler.x += -rotation.y;
            euler.y += rotation.x;
            euler.z = 0;
            transform.eulerAngles = euler;
            //
        }

        private void OnSwitch(InputAction.CallbackContext obj)
        {
            _isCursor = !_isCursor;
        }
    }
}