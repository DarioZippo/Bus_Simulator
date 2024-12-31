using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(PlayerInputManager))]
public class BusMovement : MonoBehaviour{
    static public Action<bool> OnBreak;
    static public Action OnGo;

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
    
        BusStop.OnPlayerEnter += HandleBusStopPlayerEnter;
        BusStop.OnPlayerExit += HandleBusStopPlayerExit;

        ChoosingRouteTrigger.OnPlayerEnter += HandleChoosingRouteTriggerPlayerEnter;
        ChoosingRouteTrigger.OnPlayerExit += HandleChoosingRouteTriggerPlayerExit;
    }

    private void OnDisable(){
        RouteManager.Instance.OnRouteChanged -= HandleRouteChanged;
    
        BusStop.OnPlayerEnter -= HandleBusStopPlayerEnter;
        BusStop.OnPlayerExit -= HandleBusStopPlayerExit;

        ChoosingRouteTrigger.OnPlayerEnter -= HandleChoosingRouteTriggerPlayerEnter;
        ChoosingRouteTrigger.OnPlayerExit -= HandleChoosingRouteTriggerPlayerExit;
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

        //Nelle prossimità di un incrocio posso scegliere una tratta
        if(isChoosing){
            var direction = playerInputManager.GetDirection();
            RouteManager.Instance.SetRoute(direction);
        }
        
        //Nelle prossimità di un Bus Stop posso frenare/ripartire con lo stesso tasto
        var breakGo = playerInputManager.GetBreakGo();
        if(breakGo){
            if(inBusStop){
                if(navMeshAgent.speed == 0){
                    navMeshAgent.speed = initialSpeed;

                    OnGo?.Invoke();
                }
                else{
                    navMeshAgent.speed = 0;

                    OnBreak?.Invoke(true);
                }
            }
            //Nel caso non sia vicino ad un Bus Stop do un alert di errore a schermo
            else{
                OnBreak?.Invoke(false);
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

    private void HandleBusStopPlayerExit(){
        SetInBusStop(false);
    }

    private void HandleBusStopPlayerEnter(){
        SetNextRouteWaypoint();
        SetInBusStop(true);
    }

    private void HandleChoosingRouteTriggerPlayerEnter(){
        SetIsChoosing(true);
    }

    private void HandleChoosingRouteTriggerPlayerExit(){
        SetIsChoosing(false);
    }


    public void SetIsChoosing(bool isChoosing){
        this.isChoosing = isChoosing;
    }

    public bool GetIsChoosing(){
        return isChoosing;
    }
}
