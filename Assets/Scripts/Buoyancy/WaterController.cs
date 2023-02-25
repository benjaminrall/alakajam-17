using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : Singleton<WaterController>
{
    public MeshCollider Collider { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<MeshCollider>();
    }
}
