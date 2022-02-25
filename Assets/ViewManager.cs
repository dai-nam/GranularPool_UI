using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class ViewManager : MonoBehaviour
{
    public Views activeView;

    public delegate void OnSwitchToGameView();
    public OnSwitchToGameView SwitchToGameView;
    public delegate void OnSwitchToUiView();
    public OnSwitchToUiView SwitchToUiView; 
    public delegate void OnSwitchToSplitView();
    public OnSwitchToSplitView SwitchToSplitView;

    private static ViewManager _instance;
    public static ViewManager Instance
    {
        get { return _instance; }
    }

    public enum Views
    {
        UiView, GameView, SplitView
    }

    private void Awake()
    {
        _instance = this;
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        UIElements uiElements = FindObjectOfType<UIElements>();
        CameraSwitch cs = FindObjectOfType<CameraSwitch>();
        SwitchToGameView += cs.SetGameViewCamera;
        SwitchToUiView += cs.SetUiViewCamera;
        SwitchToSplitView += cs.SetSplitViewCamera;
        SwitchToGameView += uiElements.DisableUiElements;
        SwitchToUiView += uiElements.EnableUiElements;
        SwitchToSplitView += uiElements.DisableUiElements;
    }

    private void Start()
    {
        activeView = Views.GameView;
        InvokeViewChange();
    }

    public void InvokeViewChange()
    {
        switch (activeView)
        {
            case Views.GameView :
                SwitchToGameView?.Invoke();
                break;
            case Views.UiView:
                SwitchToUiView?.Invoke();
                break;
            case Views.SplitView:
                SwitchToSplitView?.Invoke();
                break;
            default:
                break;
        }
    }
}
