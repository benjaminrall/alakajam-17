using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloppyMan : MonoBehaviour
{
    Rigidbody _rb;

    public Transform _bodyBone;

    public Transform _leftPaddle;
    public Transform _rightPaddle;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = _rb.velocity;
        velocity.y = 0;
        velocity.Normalize();

        Vector3 lookPos = _bodyBone.position - _rb.velocity * 0.3f + Vector3.up;

        Quaternion target = Quaternion.LookRotation(-_rb.velocity * 0.3f + Vector3.up) * Quaternion.Euler(0, -90, -90);

        _bodyBone.rotation = Quaternion.Lerp(_bodyBone.rotation, target, Time.deltaTime);
    }
}
