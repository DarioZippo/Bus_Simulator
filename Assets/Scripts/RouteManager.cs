using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RouteManager : MonoBehaviour{
    public static RouteManager Instance = null;
    
    [SerializeField] List<Transform> routeWaypoints = new List<Transform>();

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
        var result = routeWaypoints[currentRouteIndex];

        nextRouteIndex = (nextRouteIndex + 1) % routeWaypoints.Count;

        return result;
    }

    void OnValidate(){
        for (int i = 0; i < routeWaypoints.Count; i++){
            BusStop busStop = routeWaypoints[i].GetComponent<BusStop>();
            if(busStop){
                routeWaypoints[i] = busStop.GetRouteWaypoint();
            }
        }
    }

    #if UNITY_EDITOR
    void OnDrawGizmos(){
        Vector3 offset = Vector3.up * 10f;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

        int currentIndex = 0;

        foreach (Transform routeWaypoint in routeWaypoints){
            Color currentColor = currentIndex == currentRouteIndex ? Color.green : Color.red;

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
    }
    #endif
}
