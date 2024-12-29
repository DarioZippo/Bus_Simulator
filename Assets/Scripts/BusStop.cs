using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStop : MonoBehaviour
{
    [SerializeField] Transform routeWaypoint;

    public Transform GetRouteWaypoint(){
        return routeWaypoint;
    }
}
