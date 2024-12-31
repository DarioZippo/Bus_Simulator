using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

//Per mantenere un buon livello di disaccoppiamento tra GameLogic ed UI ho gestito tutto tramite Observer Pattern
public class UIManager : MonoBehaviour{
    [SerializeField] GameObject BusStopActionPanel;
    [SerializeField] GameObject BusStopActionErrorPanel;
    [SerializeField] TextMeshProUGUI BusStopActionText;
    [SerializeField] SerializedDictionary<CrossroadsDirection, GameObject> RoutePanels;
    [SerializeField] GameObject RouteChoosePanel;

    CrossroadsDirection currentRoute;

    private void OnEnable(){
        RouteManager.Instance.OnRouteChanged += HandleRouteChanged;

        BusStop.OnPlayerEnter += HandleBusStopPlayerEnter;
        BusStop.OnPlayerExit += HandleBusStopPlayerExit;
        
        BusMovement.OnBreak += HandleBusBreak;
        BusMovement.OnGo += HandleBusGo;

        ChoosingRouteTrigger.OnPlayerEnter += HandleChoosingRouteTriggerPlayerEnter;
        ChoosingRouteTrigger.OnPlayerExit += HandleChoosingRouteTriggerPlayerExit;
    }

    public void Start(){
        BusStopActionPanel.SetActive(false);
        BusStopActionErrorPanel.SetActive(false);
        RouteChoosePanel.SetActive(false);

        currentRoute = RouteManager.Instance.GetRoute();
    }

    private void HandleRouteChanged(CrossroadsDirection newRoute){
        RoutePanels[currentRoute].GetComponent<CanvasGroup>().alpha = 0.25f;

        RoutePanels[newRoute].GetComponent<CanvasGroup>().alpha = 1f;
        currentRoute = newRoute;
    }

    private void HandleBusStopPlayerEnter(){
        BusStopActionPanel.SetActive(true);
    }

    private void HandleBusStopPlayerExit(){
        BusStopActionPanel.SetActive(false);
    }

    private void HandleBusBreak(bool isValid){
        if(isValid)
            BusStopActionText.text = "GO";
        else{
            StartCoroutine(ShowErrorPanel());
        }
    }

    private IEnumerator ShowErrorPanel(){ 
        BusStopActionErrorPanel.SetActive(true); 
        yield return new WaitForSeconds(1);

        BusStopActionErrorPanel.SetActive(false);
    }

    private void HandleBusGo(){
        BusStopActionText.text = "BREAK";
    }

    private void HandleChoosingRouteTriggerPlayerEnter(){
        RouteChoosePanel.SetActive(true);
    }

    private void HandleChoosingRouteTriggerPlayerExit(){
        RouteChoosePanel.SetActive(false);
    }
}
