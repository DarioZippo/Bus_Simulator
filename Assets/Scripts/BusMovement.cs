using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(PlayerInputManager))]
public class BusMovement : MonoBehaviour{
    NavMeshAgent navMeshAgent;
    PlayerInputManager playerInputManager;
    
    bool isChoosing = false;
    bool inBusStop = false;

    float initialSpeed;

    private void Awake(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerInputManager = GetComponent<PlayerInputManager>();

        initialSpeed = navMeshAgent.speed;
    }

    private void OnEnable(){
        RouteManager.Instance.OnRouteChanged += HandleRouteChanged;
    }

    private void OnDisable(){
        RouteManager.Instance.OnRouteChanged -= HandleRouteChanged;
    }

    void Start(){
        Transform nextRouteWaypoint = RouteManager.Instance.GetNextRouteWaypoint();
        navMeshAgent.SetDestination(nextRouteWaypoint.position);
    }

    void Update(){
        /*
        if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
            if(!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f){
                Transform nextRouteWaypoint = RouteManager.Instance.GetNextRouteWaypoint();
                navMeshAgent.SetDestination(nextRouteWaypoint.position);
            }
        }
        */

        if(isChoosing){
            var direction = playerInputManager.GetDirection();
            RouteManager.Instance.SetRoute(direction);
        }
        if(inBusStop){
            var breakGo = playerInputManager.GetBreakGo();
            if(breakGo){
                if(navMeshAgent.speed == 0){
                    navMeshAgent.speed = initialSpeed;
                }
                else{
                    navMeshAgent.speed = 0;
                }
            }
        }
    }

    public void SetNextRouteWaypoint(){
        Transform nextRouteWaypoint = RouteManager.Instance.GetNextRouteWaypoint();
        navMeshAgent.SetDestination(nextRouteWaypoint.position);
    }

    public void SetInBusStop(bool inBusStop){
        this.inBusStop = inBusStop;
    }

    public void HandleRouteChanged(CrossroadsDirection crossroadsDirection){
        Transform nextRouteWaypoint = RouteManager.Instance.GetNextRouteWaypoint();
        navMeshAgent.SetDestination(nextRouteWaypoint.position);
    }

    public void SetIsChoosing(bool isChoosing){
        this.isChoosing = isChoosing;
    }

    public bool GetIsChoosing(){
        return isChoosing;
    }
}
