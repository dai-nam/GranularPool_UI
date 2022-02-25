using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera uiCamera;


    class GameViewCoordinates
    {
        internal static Vector3 mainCameraPosition = new Vector3(0, 0, 0);
        internal static Vector2 mainCameraRectXY = new Vector2(0, 0);
        internal static Vector2 mainCameraRectWH = new Vector2(1, 1);
    }

    class UiViewCoordinates
    {
        internal static Vector3 uiCameraPosition = new Vector3(500, 210, -60);
        internal static Vector2 uiCameraRectXY = new Vector2(0, 0);
        internal static Vector2 uiCameraRectWH = new Vector2(1, 1);
    }

    class SplitViewCoordinates
    {
        internal static Vector3 mainCameraPosition = new Vector3(0, 0, -50);
        internal static Vector2 mainCameraRectXY = new Vector2(0.5f, 0);
        internal static Vector2 mainCameraRectWH = new Vector2(0.5f, 1);

        internal static Vector3 uiCameraPosition = new Vector3(500, 200, -120);
        internal static Vector2 uiCameraRectXY = new Vector2(0, 0);
        internal static Vector2 uiCameraRectWH = new Vector2(0.5f, 1);
    }


   public void SetGameViewCamera()
    {
        mainCamera.gameObject.SetActive(true);
        uiCamera.gameObject.SetActive(false);
        mainCamera.transform.position = GameViewCoordinates.mainCameraPosition;
        mainCamera.rect = new Rect(GameViewCoordinates.mainCameraRectXY, GameViewCoordinates.mainCameraRectWH);
    }


    public void SetUiViewCamera()
    {
        mainCamera.gameObject.SetActive(false);
        uiCamera.gameObject.SetActive(true);
        uiCamera.transform.position = UiViewCoordinates.uiCameraPosition;
        uiCamera.rect = new Rect(UiViewCoordinates.uiCameraRectXY, UiViewCoordinates.uiCameraRectWH);
    }

    public void SetSplitViewCamera()
    {
        mainCamera.gameObject.SetActive(true);
        uiCamera.gameObject.SetActive(true);
        mainCamera.transform.position = SplitViewCoordinates.mainCameraPosition;
        mainCamera.rect = new Rect(SplitViewCoordinates.mainCameraRectXY, SplitViewCoordinates.mainCameraRectWH);
        uiCamera.transform.position = SplitViewCoordinates.uiCameraPosition;
        uiCamera.rect = new Rect(SplitViewCoordinates.uiCameraRectXY, SplitViewCoordinates.uiCameraRectWH);
    }

}
