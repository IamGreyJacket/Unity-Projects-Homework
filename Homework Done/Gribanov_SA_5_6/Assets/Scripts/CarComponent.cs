using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cars
{
    [RequireComponent(typeof(WheelsComponent), typeof(BaseInputController), typeof(Rigidbody))]
    public class CarComponent : MonoBehaviour
    {
        private WheelsComponent _wheels;
        private BaseInputController _input;
        private Rigidbody _rigidbody;

        [SerializeField, Range(10f, 40f)]
        private float _maxSteerAngle = 25f;
        [SerializeField]
        private DrivetrainType _drivetrain;
        [SerializeField]
        private float _engineTorque = 2500f;
        [SerializeField, Range(0f, float.MaxValue)]
        private float _handBrakeTorque = float.MaxValue;
        [SerializeField]
        private Vector3 _centerOfMass;

        private void FixedUpdate()
        {
            _wheels.UpdateVisual(_input.Rotate * _maxSteerAngle);
            var torque = _input.Acceleration * _engineTorque / 2f;
            switch (_drivetrain)
            {
                case DrivetrainType.FrontWheelDrive:
                    foreach (var wheel in _wheels.GetFrontWheels)
                    {
                        wheel.motorTorque = torque;
                    }
                    break;

                case DrivetrainType.RearWheelDrive:
                    foreach (var wheel in _wheels.GetRearWheels)
                    {
                        wheel.motorTorque = torque;
                    }
                    break;

                case DrivetrainType.AllWheelDrive:
                    torque = _input.Acceleration * _engineTorque / 4f;
                    foreach (var wheel in _wheels.GetAllWheels)
                    {
                        wheel.motorTorque = torque;
                    }
                    break;
            }
        }

        private void OnHandBrake(bool value)
        {
            if (value)
            {
                foreach(var wheel in _wheels.GetRearWheels)
                {
                    wheel.brakeTorque = _handBrakeTorque;
                }
            }
            else
            {
                foreach (var wheel in _wheels.GetRearWheels)
                {
                    wheel.brakeTorque = 0f;
                }
            }
        }

        private void Start()
        {
            _wheels = GetComponent<WheelsComponent>();
            _input = GetComponent<BaseInputController>();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.centerOfMass = _centerOfMass;
            _input.OnHandBrakeEvent += OnHandBrake;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var point = _centerOfMass;
            point.z *= transform.localScale.z;
            point.x *= transform.localScale.x;
            //_centerOfMass.x *= transform.localScale.x;
           //_centerOfMass.z *= transform.localScale.z;

            Gizmos.DrawSphere(transform.TransformPoint(point), 0.4f);
        }

        private void OnDestroy()
        {
            _input.OnHandBrakeEvent -= OnHandBrake;
        }
        public enum DrivetrainType
        {
            FrontWheelDrive,
            RearWheelDrive,
            AllWheelDrive
        }
    }
}