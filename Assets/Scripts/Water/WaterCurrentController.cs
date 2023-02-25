using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class WaterCurrentController : Singleton<WaterCurrentController>
{
    public Transform[] currentPoints;

    private void Start()
    {
        currentPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
          currentPoints[i] = transform.GetChild(i);
        }
    }
    
    public Vector3 GetCurrentDirection(Vector3 pos)
    {
        Transform point = currentPoints.OrderBy(point => Vector3.Distance(pos, point.transform.position)).ToList()[0];

        int index = Array.IndexOf(currentPoints, point);
        Vector3 dir = (currentPoints[index + 1].position - currentPoints[index].position).normalized;
        dir.y = 0;
        return dir;
    }
}
