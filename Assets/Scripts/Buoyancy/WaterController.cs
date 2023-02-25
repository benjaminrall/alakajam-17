using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public static WaterController instance;

    [SerializeField]
    public MeshCollider waterCollider;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static MeshCollider GetCollider()
    {
        return instance.waterCollider;
    }
}
