using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStop : MonoBehaviour
{
    [SerializeField] Transform routeWaypoint;

    private void OnTriggerEnter(Collider collider){
        if (collider.tag == "Player"){
            BusMovement bus = collider.gameObject.GetComponent<BusMovement>();
            bus.SetNextRouteWaypoint();
            bus.SetInBusStop(true);
        }
    }

    private void OnTriggerExit(Collider collider){
        if (collider.tag == "Player"){
            BusMovement bus = collider.gameObject.GetComponent<BusMovement>();
            bus.SetInBusStop(false);
        }
    }

    public Transform GetRouteWaypoint(){
        return routeWaypoint;
    }
}
