using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingRouteTrigger : MonoBehaviour{
    private void OnTriggerEnter(Collider collider){
        if (collider.tag == "Player"){
            BusMovement bus = collider.gameObject.GetComponent<BusMovement>();
            bus.SetIsChoosing(true);
        }
    }

    private void OnTriggerExit(Collider collider){
        if (collider.tag == "Player"){
            BusMovement bus = collider.gameObject.GetComponent<BusMovement>();
            bus.SetIsChoosing(false);
        }
    }
}
