using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class GrainAreaDisplay : MonoBehaviour
{
    Vector3 worldSpaceMin, worldSpaceMax;
    float waveFormwidth;
    public float grainAreaWidth;
    [SerializeField] uint _grainLengthInMilliseconds;
    public uint GrainLengthInMilliseconds
    { 
        get { return _grainLengthInMilliseconds; }
        set {
            if (value < 0)
                _grainLengthInMilliseconds = 0;
            else if (value > GetComponentInParent<GrainDisplay>().length)
                _grainLengthInMilliseconds = GetComponentInParent<GrainDisplay>().length;
            else
                _grainLengthInMilliseconds = value;
        }        
    } 

    [SerializeField] Transform wrapAroundArea;
    [SerializeField] Transform helper;

    RectTransform wrapAroundAreaRect;
    RectTransform mainAreaRect;
    RectTransform helperRect;

    Vector3 prevPosition;

    [Range(0f, 1f)] public float pos = 0.5f;

    private void Awake()
    {

        //Ecken des WaveFormFields erhalten
        Vector3[] corners = new Vector3[4];
        transform.parent.parent.gameObject.GetComponent<RectTransform>().GetWorldCorners(corners); //lu, lo, ro, ru
        worldSpaceMin = corners[1];
        worldSpaceMax = corners[2];
        waveFormwidth = corners[2].x - corners[1].x;

        //grainWidth = GetComponent<RectTransform>().rect.width;
        mainAreaRect = GetComponent<RectTransform>();
        prevPosition = transform.position;


        wrapAroundArea.position = worldSpaceMin;
        wrapAroundAreaRect = wrapAroundArea.gameObject.GetComponent<RectTransform>();
        wrapAroundAreaRect.pivot = new Vector2(0, 1);
        mainAreaRect.pivot = new Vector2(0, 1);

        helperRect = helper.GetComponent<RectTransform>();
        helperRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mainAreaRect.rect.height);
        helperRect.position = mainAreaRect.position;

        UpdateGrainWidthToMatchGrainLength();
    }


    private void Update()
    {
        transform.position = (worldSpaceMin + (Vector3.right *(waveFormwidth+grainAreaWidth) * pos)) - (Vector3.right * grainAreaWidth);
        UpdaeHelper();

        Vector3[] corners = GetCornersOfHelperRect();
        float leftOverlap = GetLeftOverlap(corners);
        float rightOverlap = GetRightOverlap(corners);
        ClipMainArea(leftOverlap, rightOverlap);
        UpdateWrapAroundArea(leftOverlap, rightOverlap);
    }

    void UpdaeHelper()
    {
        helper.position = (worldSpaceMin + (Vector3.right * (waveFormwidth + grainAreaWidth) * pos)) - (Vector3.right * grainAreaWidth);
        helperRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, grainAreaWidth);
    }

    private Vector3[] GetCornersOfHelperRect()
    {
        Vector3[] corners = new Vector3[4];
        helperRect.GetWorldCorners(corners);
        return corners;
    }

    private float GetLeftOverlap(Vector3[] corners)
    {
        Vector3 upperLeftCorner = corners[1];
        float overlapAmountLeft = worldSpaceMin.x - upperLeftCorner.x;
        return overlapAmountLeft;
    }

    private float GetRightOverlap(Vector3[] corners)
    {
        Vector3 upperRightCorner = corners[2];
        float overlapAmountRight = upperRightCorner.x - worldSpaceMax.x;
        return overlapAmountRight;
    }

    //Alternative: mit einem Shader!
    void ClipMainArea(float overlapAmountLeft, float overlapAmountRight)
    {
        if (overlapAmountRight <= 0)
        {
            overlapAmountRight = 0;
        }
        if (overlapAmountLeft <= 0)
        {
            overlapAmountLeft = 0;
        }

        if (overlapAmountRight != 0)
        {
            mainAreaRect.pivot = new Vector2(0, 1);
            mainAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, grainAreaWidth - overlapAmountRight);
        }


        else if (overlapAmountLeft != 0)
        {
             mainAreaRect.pivot = new Vector2(1, 1);
            mainAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, grainAreaWidth - overlapAmountLeft);
            //Pivot-Verschiebung ausgleichen
              transform.position += (Vector3.right * grainAreaWidth);
        }

        else
        {
             mainAreaRect.pivot = new Vector2(0, 1);
          //  Vector3 tmp = prevPosition;
           // prevPosition = transform.position;
            mainAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, grainAreaWidth);
         //   transform.position = tmp;
        }
    }

    void UpdateWrapAroundArea(float overlapAmountLeft, float overlapAmountRight)
    {

        if (overlapAmountRight <= 0)
        {
            overlapAmountRight = 0;
        }
        if (overlapAmountLeft <= 0)
        {
            overlapAmountLeft = 0;
        }
     
        if (overlapAmountLeft != 0 )
        {
            wrapAroundArea.position = worldSpaceMax;
            //Pivots umsetzen, damit sich die Breite von links aus ändert
            wrapAroundAreaRect.pivot = new Vector2(1, 1);
            wrapAroundAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, overlapAmountLeft);
        }
        else if(overlapAmountRight != 0)
        {
            wrapAroundArea.position = worldSpaceMin;
            wrapAroundAreaRect.pivot = new Vector2(0, 1);
            wrapAroundAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, overlapAmountRight);
        }

        else
        {
            wrapAroundAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        }
    }

    //An UI Button registrieren, wenn sich Grain-Länge ändert, ist diese Methode wirklich nötig?
    void UpdateSampleWidth()
    {

    }

    //Funktion nötig, da man keine Properties im Inspector haben kann
    private void OnValidate()
    {
        UpdateGrainWidthToMatchGrainLength();
    }

    private void UpdateGrainWidthToMatchGrainLength()
    {
        grainAreaWidth = ((float) _grainLengthInMilliseconds / GetComponentInParent<GrainDisplay>().length) * waveFormwidth;
    }
}
