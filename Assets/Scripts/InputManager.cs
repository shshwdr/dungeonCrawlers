//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class InputManager : Singleton<InputManager>
//{
//    private PlayerControll playerControls;
//    private void Awake()
//    {
//        playerControls = new PlayerControll();
//    }
//    private void OnEnable()
//    {
//        playerControls.Enable();
//    }
//    private void OnDisable()
//    {
//        playerControls.Disable();
//    }
//    public Vector2 GetPlayerMovement()
//    {
//        return playerControls.Player.Movement.ReadValue<Vector2>();
//    }
//    public Vector2 GetPlayerLook()
//    {
//        return playerControls.Player.Look.ReadValue<Vector2>();
//    }
//}
