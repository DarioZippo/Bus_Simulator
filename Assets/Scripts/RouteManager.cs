using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AYellowpaper.SerializedCollections;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class RouteManager : MonoBehaviour{
    [Serializable]
    public struct Route{
        public List<Transform> waypoints;
        public Color color;
    }

    public static RouteManager Instance = null;

    public Action<CrossroadsDirection> OnRouteChanged;
    
    [SerializeField] SerializedDictionary<CrossroadsDirection, Route> routes;
    [SerializeField] CrossroadsDirection currentRoute = CrossroadsDirection.Left;

    int currentRouteIndex = 0;
    int nextRouteIndex = 0;

    void Awake(){
        if(Instance){
            Destroy(this);
        }
        else{
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public Transform GetNextRouteWaypoint(){
        currentRouteIndex = nextRouteIndex;

        var routeWaypoints = routes[currentRoute].waypoints;
        var result = routeWaypoints[currentRouteIndex];

        nextRouteIndex = (nextRouteIndex + 1) % routeWaypoints.Count;

        return result;
    }

    public void SetRoute(CrossroadsDirection newRoute){
        if(currentRoute != newRoute){
            currentRoute = newRoute;
            nextRouteIndex = 0;

            OnRouteChanged?.Invoke(currentRoute);
        }
    }

    public CrossroadsDirection GetRoute(){
        return currentRoute;
    }

    void OnValidate(){
        //Debug.Log("Validate");
        foreach (KeyValuePair<CrossroadsDirection, Route> keyValuePair in routes){
            //Debug.Log("Direction" + keyValuePair.Key + ", with " + keyValuePair.Value.waypoints.Count + " elements");
            var routeWaypoints = keyValuePair.Value.waypoints;
            if(routeWaypoints == null)
                return;
                
            for (int i = 0; i < routeWaypoints.Count; i++){
                if(routeWaypoints[i]){
                    BusStop busStop = routeWaypoints[i].GetComponent<BusStop>();
                    if(busStop){
                        routeWaypoints[i] = busStop.GetRouteWaypoint();
                    }
                }
            }
        }
    }

    #if UNITY_EDITOR
    void OnDrawGizmos(){
        Vector3 offset = Vector3.up * 10f;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

        int currentIndex = 0;

        foreach (KeyValuePair<CrossroadsDirection, Route> keyValuePair in routes){
            var routeCrossroadsDirection = keyValuePair.Key;
            var routeColor = keyValuePair.Value.color;
            var routeWaypoints = keyValuePair.Value.waypoints;
            foreach (Transform routeWaypoint in routeWaypoints){
                Color currentColor = currentRoute == routeCrossroadsDirection && currentIndex == currentRouteIndex ? Color.green : routeColor;
                
                if(routeWaypoint){
                    Handles.DrawBezier(
                        transform.position,
                        routeWaypoint.position,
                        transform.position - offset,
                        routeWaypoint.position + offset,
                        currentColor,
                        EditorGUIUtility.whiteTexture,
                        3f
                    );
                }
                currentIndex++;
            }
            currentIndex = 0;
        }
    }
    #endif
}
