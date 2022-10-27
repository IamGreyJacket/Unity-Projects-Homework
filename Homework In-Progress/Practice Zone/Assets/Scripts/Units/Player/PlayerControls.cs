//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.3
//     from Assets/Scripts/Units/Player/PlayerControls.inputactions
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

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""GameMap"",
            ""id"": ""a7e92c76-eda9-4480-8dda-04b96a804b1e"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""f307913b-225b-450c-a2a9-15aae1d8355a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""FastAttack"",
                    ""type"": ""Button"",
                    ""id"": ""5f960b5c-695e-49bc-8724-0444f48342e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""StrongAttack"",
                    ""type"": ""Button"",
                    ""id"": ""493a2e8d-9dff-4754-a684-a44110972f7d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2d Vector"",
                    ""id"": ""5da55818-89ab-4445-93f8-c122c6f84222"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""494eb0ba-751e-4163-942f-acfbddf8a557"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5fb42615-04d5-4d2a-9803-dabceefc7b07"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6ff09ee9-260d-4121-9bd1-a22cf07d749b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cdd547d5-d9c8-4840-91c5-1fabe3aae771"",
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
                    ""id"": ""02057d9f-e2b9-4c1c-8499-704cf4d098b1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FastAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd9ce1ca-b55b-42ca-95b8-8bdf2af33023"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StrongAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GameMap
        m_GameMap = asset.FindActionMap("GameMap", throwIfNotFound: true);
        m_GameMap_Movement = m_GameMap.FindAction("Movement", throwIfNotFound: true);
        m_GameMap_FastAttack = m_GameMap.FindAction("FastAttack", throwIfNotFound: true);
        m_GameMap_StrongAttack = m_GameMap.FindAction("StrongAttack", throwIfNotFound: true);
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

    // GameMap
    private readonly InputActionMap m_GameMap;
    private IGameMapActions m_GameMapActionsCallbackInterface;
    private readonly InputAction m_GameMap_Movement;
    private readonly InputAction m_GameMap_FastAttack;
    private readonly InputAction m_GameMap_StrongAttack;
    public struct GameMapActions
    {
        private @PlayerControls m_Wrapper;
        public GameMapActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_GameMap_Movement;
        public InputAction @FastAttack => m_Wrapper.m_GameMap_FastAttack;
        public InputAction @StrongAttack => m_Wrapper.m_GameMap_StrongAttack;
        public InputActionMap Get() { return m_Wrapper.m_GameMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameMapActions set) { return set.Get(); }
        public void SetCallbacks(IGameMapActions instance)
        {
            if (m_Wrapper.m_GameMapActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GameMapActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameMapActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameMapActionsCallbackInterface.OnMovement;
                @FastAttack.started -= m_Wrapper.m_GameMapActionsCallbackInterface.OnFastAttack;
                @FastAttack.performed -= m_Wrapper.m_GameMapActionsCallbackInterface.OnFastAttack;
                @FastAttack.canceled -= m_Wrapper.m_GameMapActionsCallbackInterface.OnFastAttack;
                @StrongAttack.started -= m_Wrapper.m_GameMapActionsCallbackInterface.OnStrongAttack;
                @StrongAttack.performed -= m_Wrapper.m_GameMapActionsCallbackInterface.OnStrongAttack;
                @StrongAttack.canceled -= m_Wrapper.m_GameMapActionsCallbackInterface.OnStrongAttack;
            }
            m_Wrapper.m_GameMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @FastAttack.started += instance.OnFastAttack;
                @FastAttack.performed += instance.OnFastAttack;
                @FastAttack.canceled += instance.OnFastAttack;
                @StrongAttack.started += instance.OnStrongAttack;
                @StrongAttack.performed += instance.OnStrongAttack;
                @StrongAttack.canceled += instance.OnStrongAttack;
            }
        }
    }
    public GameMapActions @GameMap => new GameMapActions(this);
    public interface IGameMapActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnFastAttack(InputAction.CallbackContext context);
        void OnStrongAttack(InputAction.CallbackContext context);
    }
}