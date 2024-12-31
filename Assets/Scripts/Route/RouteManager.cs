using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AYellowpaper.SerializedCollections;

// Direttive da pre-processore per l'uso di Handles nell'OnDrawGizmos
// Gli Handles non sono Buildabili, quindi li compilo solo in Editor, come l'inclusione della libreria UnityEditor
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RouteManager : MonoBehaviour{
    [Serializable]
    public struct Route{
        public List<Transform> waypoints;
        //Il colore delle linee di debug che punteranno ai BusStop di questa tratta
        public Color color;
    }

    public static RouteManager Instance = null;

    public Action<CrossroadsDirection> OnRouteChanged;
    
    [SerializeField] SerializedDictionary<CrossroadsDirection, Route> routes;
    [SerializeField] CrossroadsDirection currentRoute = CrossroadsDirection.Left;

    int currentRouteIndex = 0;
    int nextRouteIndex = 0;

    //Singleton Pattern
    void Awake(){
        if(Instance){
            Destroy(this);
        }
        else{
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    //Avanzamento ciclico tra i BusStop dell'attuale tratta
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

    //Funzione che controlla ad ogni modifica via editor l'attuale stato delle tratte
    //Se tra i Waypoints ci sono BusStop, ne ricava automaticamente il punto in cui il bus dovrebbe passare
    void OnValidate(){
        foreach (KeyValuePair<CrossroadsDirection, Route> keyValuePair in routes){
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

    //Viene compilato se in editor per via degli Handles
    //Mostra costamente a video delle linee curve per mostrare l'attuale stato delle tratte registrate
    //Le tratte si distinguono dal colore diverso di tali linee, ad eccezione del target attuale del bus, è che colorato di verde
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
