using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaveController : MonoBehaviour
{
    private MeshFilter _meshFilter;

    public MeshCollider Collider { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<MeshCollider>();
        _meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        
        Vector3[] vertices = _meshFilter.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x);
        }

        _meshFilter.mesh.vertices = vertices;
        _meshFilter.mesh.RecalculateNormals();
    }

    
}
