﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_RoadGenerator : MonoBehaviour
{

    public int segmenCount = 100;
    public float segmentLength = 2;
    LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        lr = transform.GetComponent<LineRenderer>();
        List<Vector3> positions = new List<Vector3>();


        float pY = 0;
        float pX = 0;
        for (int i = 0; i < 5; i++){
            positions.Add(new Vector3(pX, 0, 0));
            pX += segmentLength;
        }
        
        for (int i = 5; i < segmenCount; i++)
        {
            Vector3 p = new Vector3();
            p.x = pX;
            pX = pX + segmentLength;
            p.y = pY<1 && pY>-1 ? Random.Range(pY-0.75f, pY+0.75f) : Random.Range(-1f,1f);
            pY = p.y;
            p.z = 0;
            positions.Add(p);
        }
        lr.positionCount=positions.Count;
        lr.SetPositions(positions.ToArray());
       
        GetComponent<EdgeCollider2D>().points = ConvertArray(positions.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Vector2[] ConvertArray(Vector3[] v3)
    {
        Vector2[] v2 = new Vector2[v3.Length];
        for (int i = 0; i < v3.Length; i++)
        {
            Vector3 tempV3 = v3[i];
            v2[i] = new Vector2(tempV3.x, tempV3.y);
        }
        return v2;
    }

}

