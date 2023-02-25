using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Water;

public class Floater : MonoBehaviour
{
    public Rigidbody rb;
    
    public float depthBeforeSubmerged = .5f;
    public float displacementAmount = 1f;
    public int floaterCount = 1;
    public float waterDrag = 1f;
    public float waterAngularDrag = 0.5f;
    public float waterNoiseAdjustment = 0.25f;

    private void FixedUpdate()
    {
        Vector3 position = transform.position - Vector3.up * waterNoiseAdjustment;
        
        rb.AddForceAtPosition(Physics.gravity / floaterCount, position, ForceMode.Acceleration);
        
        Ray ray = new (position + Vector3.up * 20, Vector3.down);
        
        if (!WaterController.Instance.Collider.Raycast(ray, out RaycastHit hit, Mathf.Infinity) || position.y >= hit.point.y) return;
        
        float displacementMul = Mathf.Clamp01((hit.point.y - position.y) / depthBeforeSubmerged) * displacementAmount;
        
        rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMul), position, ForceMode.Acceleration);
        rb.AddForce(displacementMul * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.AddTorque(displacementMul * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
