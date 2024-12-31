using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingRouteTrigger : MonoBehaviour{
    static public Action OnPlayerEnter;
    static public Action OnPlayerExit;
    
    private void OnTriggerEnter(Collider collider){
        if (collider.tag == "Player"){
            OnPlayerEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider collider){
        if (collider.tag == "Player"){
            OnPlayerExit?.Invoke();
        }
    }
}
