//// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerControll.inputactions'

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.Utilities;

//public class @PlayerControll : IInputActionCollection, IDisposable
//{
//    public InputActionAsset asset { get; }
//    public @PlayerControll()
//    {
//        asset = InputActionAsset.FromJson(@"{
//    ""name"": ""PlayerControll"",
//    ""maps"": [
//        {
//            ""name"": ""Player"",
//            ""id"": ""77416aa8-e8b0-4e8b-81b6-daac4e915be7"",
//            ""actions"": [
//                {
//                    ""name"": ""Movement"",
//                    ""type"": ""PassThrough"",
//                    ""id"": ""7d4166f5-9fa9-4f99-998a-9bdff7a7c90b"",
//                    ""expectedControlType"": ""Vector2"",
//                    ""processors"": """",
//                    ""interactions"": """"
//                },
//                {
//                    ""name"": ""Look"",
//                    ""type"": ""PassThrough"",
//                    ""id"": ""d6dc6d57-c964-4e09-8553-2e02c3709d48"",
//                    ""expectedControlType"": ""Vector2"",
//                    ""processors"": """",
//                    ""interactions"": """"
//                }
//            ],
//            ""bindings"": [
//                {
//                    ""name"": ""WASD"",
//                    ""id"": ""0402e684-7ea1-4bcf-9279-da2e897ece53"",
//                    ""path"": ""2DVector"",
//                    ""interactions"": """",
//                    ""processors"": """",
//                    ""groups"": """",
//                    ""action"": ""Movement"",
//                    ""isComposite"": true,
//                    ""isPartOfComposite"": false
//                },
//                {
//                    ""name"": ""up"",
//                    ""id"": ""068c9cf5-8bb8-41bc-b198-6ea7b79101a5"",
//                    ""path"": ""<Keyboard>/w"",
//                    ""interactions"": """",
//                    ""processors"": """",
//                    ""groups"": """",
//                    ""action"": ""Movement"",
//                    ""isComposite"": false,
//                    ""isPartOfComposite"": true
//                },
//                {
//                    ""name"": ""down"",
//                    ""id"": ""aec53cc8-7edd-4d42-ac1d-5a399eaba34b"",
//                    ""path"": ""<Keyboard>/s"",
//                    ""interactions"": """",
//                    ""processors"": """",
//                    ""groups"": """",
//                    ""action"": ""Movement"",
//                    ""isComposite"": false,
//                    ""isPartOfComposite"": true
//                },
//                {
//                    ""name"": ""left"",
//                    ""id"": ""2b2d2767-217c-4bec-ba63-e073d2879525"",
//                    ""path"": ""<Keyboard>/a"",
//                    ""interactions"": """",
//                    ""processors"": """",
//                    ""groups"": """",
//                    ""action"": ""Movement"",
//                    ""isComposite"": false,
//                    ""isPartOfComposite"": true
//                },
//                {
//                    ""name"": ""right"",
//                    ""id"": ""9d16aaa9-5c7f-4a95-85b5-f3c573c24d77"",
//                    ""path"": ""<Keyboard>/d"",
//                    ""interactions"": """",
//                    ""processors"": """",
//                    ""groups"": """",
//                    ""action"": ""Movement"",
//                    ""isComposite"": false,
//                    ""isPartOfComposite"": true
//                },
//                {
//                    ""name"": """",
//                    ""id"": ""39c8d8a7-b7d3-42bf-b3f6-b72b0429f2d8"",
//                    ""path"": ""<Mouse>/delta"",
//                    ""interactions"": """",
//                    ""processors"": """",
//                    ""groups"": """",
//                    ""action"": ""Look"",
//                    ""isComposite"": false,
//                    ""isPartOfComposite"": false
//                }
//            ]
//        }
//    ],
//    ""controlSchemes"": []
//}");
//        // Player
//        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
//        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
//        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
//    }

//    public void Dispose()
//    {
//        UnityEngine.Object.Destroy(asset);
//    }

//    public InputBinding? bindingMask
//    {
//        get => asset.bindingMask;
//        set => asset.bindingMask = value;
//    }

//    public ReadOnlyArray<InputDevice>? devices
//    {
//        get => asset.devices;
//        set => asset.devices = value;
//    }

//    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

//    public bool Contains(InputAction action)
//    {
//        return asset.Contains(action);
//    }

//    public IEnumerator<InputAction> GetEnumerator()
//    {
//        return asset.GetEnumerator();
//    }

//    IEnumerator IEnumerable.GetEnumerator()
//    {
//        return GetEnumerator();
//    }

//    public void Enable()
//    {
//        asset.Enable();
//    }

//    public void Disable()
//    {
//        asset.Disable();
//    }

//    // Player
//    private readonly InputActionMap m_Player;
//    private IPlayerActions m_PlayerActionsCallbackInterface;
//    private readonly InputAction m_Player_Movement;
//    private readonly InputAction m_Player_Look;
//    public struct PlayerActions
//    {
//        private @PlayerControll m_Wrapper;
//        public PlayerActions(@PlayerControll wrapper) { m_Wrapper = wrapper; }
//        public InputAction @Movement => m_Wrapper.m_Player_Movement;
//        public InputAction @Look => m_Wrapper.m_Player_Look;
//        public InputActionMap Get() { return m_Wrapper.m_Player; }
//        public void Enable() { Get().Enable(); }
//        public void Disable() { Get().Disable(); }
//        public bool enabled => Get().enabled;
//        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
//        public void SetCallbacks(IPlayerActions instance)
//        {
//            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
//            {
//                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
//                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
//                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
//                @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
//                @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
//                @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
//            }
//            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
//            if (instance != null)
//            {
//                @Movement.started += instance.OnMovement;
//                @Movement.performed += instance.OnMovement;
//                @Movement.canceled += instance.OnMovement;
//                @Look.started += instance.OnLook;
//                @Look.performed += instance.OnLook;
//                @Look.canceled += instance.OnLook;
//            }
//        }
//    }
//    public PlayerActions @Player => new PlayerActions(this);
//    public interface IPlayerActions
//    {
//        void OnMovement(InputAction.CallbackContext context);
//        void OnLook(InputAction.CallbackContext context);
//    }
//}
