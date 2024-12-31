using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Questo componente fa da punto di accesso generale per i valori dati in input dal giocatore
// mappando le funzioni con gli eventi del NewInputSystem

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
        /*
        else{
            return input.y > 0 ? CrossroadsDirection.Up : direction;
        }
        */
        return direction;
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
