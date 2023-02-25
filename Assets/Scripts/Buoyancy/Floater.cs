using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody _rb;

    void Start()
    {

    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * 20, Vector3.down);
        if (WaterController.GetCollider().Raycast(ray, out hit, Mathf.Infinity))
        {
            if (transform.position.y < hit.point.y)
            {
                float displacementMul = Mathf.Clamp01((hit.point.y - transform.position.y) / 1.0f);
                _rb.AddForceAtPosition(hit.normal * -Physics.gravity.y * displacementMul, transform.position);
            }
        }
    }
}
