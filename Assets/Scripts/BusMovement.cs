using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BusMovement : MonoBehaviour{
    NavMeshAgent navMeshAgent;

    private void Awake(){
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start(){
        Transform nextRouteWaypoint = RouteManager.Instance.GetNextRouteWaypoint();
        navMeshAgent.SetDestination(nextRouteWaypoint.position);
    }

    void Update(){
        if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
            if(!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f){
                Transform nextRouteWaypoint = RouteManager.Instance.GetNextRouteWaypoint();
                navMeshAgent.SetDestination(nextRouteWaypoint.position);
            }
        }
    }
}
