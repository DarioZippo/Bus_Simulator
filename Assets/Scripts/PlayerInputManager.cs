using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This script acts as a single point for all other scripts to get
// the current input from. It uses Unity's new Input System and
// functions should be mapped to their corresponding controls
// using a PlayerInput component with Unity Events.

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputManager : MonoBehaviour{
    private PlayerInput playerInput;

    private CrossroadsDirection direction;
    private bool BreakGoPressed;

    private void Awake(){
        direction = RouteManager.Instance.GetRoute();
    }

    private void OnSelectNextRoute(InputValue value) {
        Vector2 input = value.Get<Vector2>(); 
        direction = GetDirection(input);
    }

    private void OnBreakGo(InputValue value){
        BreakGoPressed = value.isPressed;
    }

    private CrossroadsDirection GetDirection(Vector2 input){
        if (input == Vector2.zero){
            return direction;
        }
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y)){
            return input.x > 0 ? CrossroadsDirection.Right : CrossroadsDirection.Left;
        }
        else{
            return input.y > 0 ? CrossroadsDirection.Up : direction;
        }
    }

    public CrossroadsDirection GetDirection(){
        return direction;
    }

    public bool GetBreakGo(){
        var result = BreakGoPressed;
        BreakGoPressed = false;

        return result;
    }
}
