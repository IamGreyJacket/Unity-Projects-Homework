//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.1
//     from Assets/MyInputActions.inputactions
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

public partial class @MyInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MyInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MyInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerOnFoot"",
            ""id"": ""04e5852c-dbf7-4c20-8a0a-a4a21f284eaf"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""0f35fa95-7452-4711-82d8-f79dd15a1443"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""377e7277-924e-4775-bb5b-50d57c724cb2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""08c292a5-59a0-45fb-a24c-643e595a1adc"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""9b6327a8-0022-47c5-bafa-310c06854972"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""faf4dfdf-d846-4498-92c2-7fd4ba4a74f7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""81f86d68-45d3-457c-8f05-70940b154dcc"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerOnFoot
        m_PlayerOnFoot = asset.FindActionMap("PlayerOnFoot", throwIfNotFound: true);
        m_PlayerOnFoot_Movement = m_PlayerOnFoot.FindAction("Movement", throwIfNotFound: true);
        m_PlayerOnFoot_Jump = m_PlayerOnFoot.FindAction("Jump", throwIfNotFound: true);
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

    // PlayerOnFoot
    private readonly InputActionMap m_PlayerOnFoot;
    private IPlayerOnFootActions m_PlayerOnFootActionsCallbackInterface;
    private readonly InputAction m_PlayerOnFoot_Movement;
    private readonly InputAction m_PlayerOnFoot_Jump;
    public struct PlayerOnFootActions
    {
        private @MyInputActions m_Wrapper;
        public PlayerOnFootActions(@MyInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerOnFoot_Movement;
        public InputAction @Jump => m_Wrapper.m_PlayerOnFoot_Jump;
        public InputActionMap Get() { return m_Wrapper.m_PlayerOnFoot; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerOnFootActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerOnFootActions instance)
        {
            if (m_Wrapper.m_PlayerOnFootActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerOnFootActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerOnFootActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerOnFootActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_PlayerOnFootActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerOnFootActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerOnFootActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_PlayerOnFootActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
            }
        }
    }
    public PlayerOnFootActions @PlayerOnFoot => new PlayerOnFootActions(this);
    public interface IPlayerOnFootActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
}
