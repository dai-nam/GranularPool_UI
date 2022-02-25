using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class Playhead : MonoBehaviour
{
    [Range(0, 1)] public float pos;
    LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.startWidth = 20;
        lr.endWidth = 20;
    }

    private void Start()
    {
        SetPosition(transform.position);
    }

    void Update()
    {
        
    }

    void SetPosition(Vector3 pos)
    {
        lr.SetPosition(0, pos);
        lr.SetPosition(1, pos + new Vector3(0, 100, 0));
    }
}
