using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cars
{
    public class PlayerInputController : BaseInputController
    {
        private Controls _controls;

        private void Awake()
        {
            _controls = new Controls();
            _controls.Car.HandBrake.performed += _ => CallHandBrake(true);
            _controls.Car.HandBrake.canceled += _ => CallHandBrake(false);
        }

        protected override void FixedUpdate()
        {
            if(!GameManager.Self.IsCountdown && !GameManager.Self.IsFinished)
                {
                var direction = _controls.Car.Steering.ReadValue<float>();
                if (direction == 0f && Rotate != 0f)
                {
                    Rotate = Rotate > 0f
                        ? Rotate - Time.fixedDeltaTime
                        : Rotate + Time.fixedDeltaTime;
                }
                else
                {
                    Rotate = Mathf.Clamp(Rotate + direction * Time.fixedDeltaTime, -1f, 1f);
                }

                Acceleration = _controls.Car.Acceleration.ReadValue<float>();
            }
            else if (GameManager.Self.IsFinished) CallHandBrake(true);

        }

        private void OnEnable()
        {
            _controls.Car.Enable();
        }

        private void OnDisable()
        {
            _controls.Car.Disable();
        }

        private void OnDestroy()
        {
            _controls.Dispose();
        }
    }
}