//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Cars
{
    public partial class @Controls : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Controls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Car"",
            ""id"": ""848f678d-2d1d-48a4-a849-f6bbb85bfded"",
            ""actions"": [
                {
                    ""name"": ""HandBrake"",
                    ""type"": ""Button"",
                    ""id"": ""83e2b6ea-606b-48d6-bdfd-d9985cac525d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Acceleration"",
                    ""type"": ""Value"",
                    ""id"": ""b8be7577-efc8-4e38-b206-ae597462e590"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Steering"",
                    ""type"": ""Value"",
                    ""id"": ""f5aeb9b0-ba79-44c5-bb2b-5f6657395fc7"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7572794a-d276-464e-9dbc-979281cf77a6"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandBrake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WS"",
                    ""id"": ""d64d145b-7c27-447d-9cd5-4cefcf073036"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""4ec674a1-f368-4e8e-8139-084f20112f63"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""51a8a5f6-b15a-4d13-ab47-abc6aa0f4920"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""AD"",
                    ""id"": ""2979a0df-0305-4d3d-ba03-6a18216eb76c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0ceda693-827e-461d-a5e0-749502ce316a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""336e1cbe-9d4d-465f-a20f-f2886a227f2a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Car
            m_Car = asset.FindActionMap("Car", throwIfNotFound: true);
            m_Car_HandBrake = m_Car.FindAction("HandBrake", throwIfNotFound: true);
            m_Car_Acceleration = m_Car.FindAction("Acceleration", throwIfNotFound: true);
            m_Car_Steering = m_Car.FindAction("Steering", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }
        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Car
        private readonly InputActionMap m_Car;
        private ICarActions m_CarActionsCallbackInterface;
        private readonly InputAction m_Car_HandBrake;
        private readonly InputAction m_Car_Acceleration;
        private readonly InputAction m_Car_Steering;
        public struct CarActions
        {
            private @Controls m_Wrapper;
            public CarActions(@Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @HandBrake => m_Wrapper.m_Car_HandBrake;
            public InputAction @Acceleration => m_Wrapper.m_Car_Acceleration;
            public InputAction @Steering => m_Wrapper.m_Car_Steering;
            public InputActionMap Get() { return m_Wrapper.m_Car; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CarActions set) { return set.Get(); }
            public void SetCallbacks(ICarActions instance)
            {
                if (m_Wrapper.m_CarActionsCallbackInterface != null)
                {
                    @HandBrake.started -= m_Wrapper.m_CarActionsCallbackInterface.OnHandBrake;
                    @HandBrake.performed -= m_Wrapper.m_CarActionsCallbackInterface.OnHandBrake;
                    @HandBrake.canceled -= m_Wrapper.m_CarActionsCallbackInterface.OnHandBrake;
                    @Acceleration.started -= m_Wrapper.m_CarActionsCallbackInterface.OnAcceleration;
                    @Acceleration.performed -= m_Wrapper.m_CarActionsCallbackInterface.OnAcceleration;
                    @Acceleration.canceled -= m_Wrapper.m_CarActionsCallbackInterface.OnAcceleration;
                    @Steering.started -= m_Wrapper.m_CarActionsCallbackInterface.OnSteering;
                    @Steering.performed -= m_Wrapper.m_CarActionsCallbackInterface.OnSteering;
                    @Steering.canceled -= m_Wrapper.m_CarActionsCallbackInterface.OnSteering;
                }
                m_Wrapper.m_CarActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @HandBrake.started += instance.OnHandBrake;
                    @HandBrake.performed += instance.OnHandBrake;
                    @HandBrake.canceled += instance.OnHandBrake;
                    @Acceleration.started += instance.OnAcceleration;
                    @Acceleration.performed += instance.OnAcceleration;
                    @Acceleration.canceled += instance.OnAcceleration;
                    @Steering.started += instance.OnSteering;
                    @Steering.performed += instance.OnSteering;
                    @Steering.canceled += instance.OnSteering;
                }
            }
        }
        public CarActions @Car => new CarActions(this);
        public interface ICarActions
        {
            void OnHandBrake(InputAction.CallbackContext context);
            void OnAcceleration(InputAction.CallbackContext context);
            void OnSteering(InputAction.CallbackContext context);
        }
    }
}