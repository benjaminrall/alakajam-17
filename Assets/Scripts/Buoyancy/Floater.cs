using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    Rigidbody _rb;
    float depthBeforeSubmerged = 1f;
    float displacementAmount = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0f)
        {
            float displacementMul = Mathf.Clamp01(-transform.position.y / depthBeforeSubmerged) * displacementAmount;
            _rb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMul, 0), ForceMode.Acceleration);
        }
    }
}
