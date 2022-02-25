using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class GrainAreaDisplay : MonoBehaviour
{
    Vector3 worldSpaceMin, worldSpaceMax;
    float waveFormwidth;
    public float grainWidth;
    [SerializeField] Transform wrapAroundArea;
    [SerializeField] Transform helper;

    RectTransform wrapAroundAreaRect;
    RectTransform mainAreaRect;
    RectTransform helperRect;

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

        wrapAroundArea.position = worldSpaceMin;
        wrapAroundAreaRect = wrapAroundArea.gameObject.GetComponent<RectTransform>();
        wrapAroundAreaRect.pivot = new Vector2(0, 1);
        mainAreaRect.pivot = new Vector2(0, 1);

        helperRect = helper.GetComponent<RectTransform>();
        helperRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mainAreaRect.rect.height);
        helperRect.position = mainAreaRect.position;
    }


    private void Update()
    {
        transform.position = (worldSpaceMin + new Vector3((waveFormwidth+grainWidth) * pos, 0, 0)) - new Vector3(grainWidth, 0, 0);
        UpdaeHelper();

        Vector3[] corners = GetCornersOfHelperRect();
        float leftOverlap = GetLeftOverlap(corners);
        float rightOverlap = GetRightOverlap(corners);
        ClipMainArea(leftOverlap, rightOverlap);
        UpdateWrapAroundArea(leftOverlap, rightOverlap);
    }

    void UpdaeHelper()
    {
        helper.position = (worldSpaceMin + new Vector3((waveFormwidth + grainWidth) * pos, 0, 0)) - new Vector3(grainWidth, 0, 0);
        helperRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, grainWidth);
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
            mainAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, grainWidth - overlapAmountRight);
        }


        else if (overlapAmountLeft != 0)
        {
            mainAreaRect.pivot = new Vector2(1, 1);
            mainAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, grainWidth - overlapAmountLeft);
            //Pivot-Verschiebung ausgleichen
            transform.position += new Vector3(grainWidth, 0, 0);
        }

        else
        {
            mainAreaRect.pivot = new Vector2(0, 1);
            mainAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, grainWidth);
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

}
