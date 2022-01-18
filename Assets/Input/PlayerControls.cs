// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""GroundMovement"",
            ""id"": ""0bab2409-3ef2-48bb-adca-7f251a507203"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b9a3c695-ac09-4996-bd19-78c8e5ffb21a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a032b98e-a33e-4c81-9ad1-b7820eca0f37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseX"",
                    ""type"": ""PassThrough"",
                    ""id"": ""de2c39d2-5950-4ce9-b2ee-a731ef7522d1"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseY"",
                    ""type"": ""PassThrough"",
                    ""id"": ""820633ad-ceb1-4b02-b721-8a60daf7516a"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""28139b61-0e85-4cc1-a72d-4812be3887f3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""db6cf4ba-8655-48d4-9075-e0a2bb8ca68c"",
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
                    ""id"": ""1d7b8ec4-31bd-434f-b3fa-13dfacad8e0d"",
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
                    ""id"": ""fbb75e78-e643-4611-89b8-511813fa2a27"",
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
                    ""id"": ""934dafb0-1319-40ca-8e0c-b95ae757fe7c"",
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
                    ""id"": ""8fb59a30-2326-4d23-812d-9b8456bb21c3"",
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
                    ""id"": ""f9010c87-65a4-4dd5-aa90-9fb9ae081d08"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e878524-f136-4732-8d02-3e1c292914c0"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""475b6708-6482-4256-8203-424e6568e51d"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7febd67b-8378-4dc5-966f-eb82164e2670"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""SwordAttack"",
            ""id"": ""2f692361-4356-4acd-8514-334f9e9fbd5e"",
            ""actions"": [
                {
                    ""name"": ""Swing"",
                    ""type"": ""Button"",
                    ""id"": ""f78964dc-42cc-4137-9c62-c004413d66ce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeMode"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f57f6bd0-b282-4ba2-8088-effdc5ad8279"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Stab"",
                    ""type"": ""Button"",
                    ""id"": ""0d7d5b2e-2d2d-4a29-b082-f2b452a31731"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b60a1b6e-dbf1-4113-b308-a480e8e1af0e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d20cb1d0-34d4-47c4-aaed-ef31665ec1c2"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a152b85e-6412-4f36-b210-f1be53313c18"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Stab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GroundMovement
        m_GroundMovement = asset.FindActionMap("GroundMovement", throwIfNotFound: true);
        m_GroundMovement_Movement = m_GroundMovement.FindAction("Movement", throwIfNotFound: true);
        m_GroundMovement_Jump = m_GroundMovement.FindAction("Jump", throwIfNotFound: true);
        m_GroundMovement_MouseX = m_GroundMovement.FindAction("MouseX", throwIfNotFound: true);
        m_GroundMovement_MouseY = m_GroundMovement.FindAction("MouseY", throwIfNotFound: true);
        m_GroundMovement_Dash = m_GroundMovement.FindAction("Dash", throwIfNotFound: true);
        // SwordAttack
        m_SwordAttack = asset.FindActionMap("SwordAttack", throwIfNotFound: true);
        m_SwordAttack_Swing = m_SwordAttack.FindAction("Swing", throwIfNotFound: true);
        m_SwordAttack_ChangeMode = m_SwordAttack.FindAction("ChangeMode", throwIfNotFound: true);
        m_SwordAttack_Stab = m_SwordAttack.FindAction("Stab", throwIfNotFound: true);
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

    // GroundMovement
    private readonly InputActionMap m_GroundMovement;
    private IGroundMovementActions m_GroundMovementActionsCallbackInterface;
    private readonly InputAction m_GroundMovement_Movement;
    private readonly InputAction m_GroundMovement_Jump;
    private readonly InputAction m_GroundMovement_MouseX;
    private readonly InputAction m_GroundMovement_MouseY;
    private readonly InputAction m_GroundMovement_Dash;
    public struct GroundMovementActions
    {
        private @PlayerControls m_Wrapper;
        public GroundMovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_GroundMovement_Movement;
        public InputAction @Jump => m_Wrapper.m_GroundMovement_Jump;
        public InputAction @MouseX => m_Wrapper.m_GroundMovement_MouseX;
        public InputAction @MouseY => m_Wrapper.m_GroundMovement_MouseY;
        public InputAction @Dash => m_Wrapper.m_GroundMovement_Dash;
        public InputActionMap Get() { return m_Wrapper.m_GroundMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GroundMovementActions set) { return set.Get(); }
        public void SetCallbacks(IGroundMovementActions instance)
        {
            if (m_Wrapper.m_GroundMovementActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnJump;
                @MouseX.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMouseX;
                @MouseX.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMouseX;
                @MouseX.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMouseX;
                @MouseY.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMouseY;
                @MouseY.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMouseY;
                @MouseY.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnMouseY;
                @Dash.started -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_GroundMovementActionsCallbackInterface.OnDash;
            }
            m_Wrapper.m_GroundMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @MouseX.started += instance.OnMouseX;
                @MouseX.performed += instance.OnMouseX;
                @MouseX.canceled += instance.OnMouseX;
                @MouseY.started += instance.OnMouseY;
                @MouseY.performed += instance.OnMouseY;
                @MouseY.canceled += instance.OnMouseY;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
            }
        }
    }
    public GroundMovementActions @GroundMovement => new GroundMovementActions(this);

    // SwordAttack
    private readonly InputActionMap m_SwordAttack;
    private ISwordAttackActions m_SwordAttackActionsCallbackInterface;
    private readonly InputAction m_SwordAttack_Swing;
    private readonly InputAction m_SwordAttack_ChangeMode;
    private readonly InputAction m_SwordAttack_Stab;
    public struct SwordAttackActions
    {
        private @PlayerControls m_Wrapper;
        public SwordAttackActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Swing => m_Wrapper.m_SwordAttack_Swing;
        public InputAction @ChangeMode => m_Wrapper.m_SwordAttack_ChangeMode;
        public InputAction @Stab => m_Wrapper.m_SwordAttack_Stab;
        public InputActionMap Get() { return m_Wrapper.m_SwordAttack; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SwordAttackActions set) { return set.Get(); }
        public void SetCallbacks(ISwordAttackActions instance)
        {
            if (m_Wrapper.m_SwordAttackActionsCallbackInterface != null)
            {
                @Swing.started -= m_Wrapper.m_SwordAttackActionsCallbackInterface.OnSwing;
                @Swing.performed -= m_Wrapper.m_SwordAttackActionsCallbackInterface.OnSwing;
                @Swing.canceled -= m_Wrapper.m_SwordAttackActionsCallbackInterface.OnSwing;
                @ChangeMode.started -= m_Wrapper.m_SwordAttackActionsCallbackInterface.OnChangeMode;
                @ChangeMode.performed -= m_Wrapper.m_SwordAttackActionsCallbackInterface.OnChangeMode;
                @ChangeMode.canceled -= m_Wrapper.m_SwordAttackActionsCallbackInterface.OnChangeMode;
                @Stab.started -= m_Wrapper.m_SwordAttackActionsCallbackInterface.OnStab;
                @Stab.performed -= m_Wrapper.m_SwordAttackActionsCallbackInterface.OnStab;
                @Stab.canceled -= m_Wrapper.m_SwordAttackActionsCallbackInterface.OnStab;
            }
            m_Wrapper.m_SwordAttackActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Swing.started += instance.OnSwing;
                @Swing.performed += instance.OnSwing;
                @Swing.canceled += instance.OnSwing;
                @ChangeMode.started += instance.OnChangeMode;
                @ChangeMode.performed += instance.OnChangeMode;
                @ChangeMode.canceled += instance.OnChangeMode;
                @Stab.started += instance.OnStab;
                @Stab.performed += instance.OnStab;
                @Stab.canceled += instance.OnStab;
            }
        }
    }
    public SwordAttackActions @SwordAttack => new SwordAttackActions(this);
    public interface IGroundMovementActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnMouseX(InputAction.CallbackContext context);
        void OnMouseY(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
    }
    public interface ISwordAttackActions
    {
        void OnSwing(InputAction.CallbackContext context);
        void OnChangeMode(InputAction.CallbackContext context);
        void OnStab(InputAction.CallbackContext context);
    }
}
