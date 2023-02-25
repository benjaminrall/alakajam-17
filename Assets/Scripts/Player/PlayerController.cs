using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;

    public Vector2 _leftStick;
    public Vector2 _rightStick;
    public bool _leftDown;
    public bool _rightDown;

    public float _prevFrameLeft;
    public float _prevFrameRight;

    public float _minRotation;
    public float _maxRotation;

    public Transform _leftTransform;
    public Transform _rightTransform;

    public Transform _leftEnd;
    public Transform _rightEnd;

    public float _leftTarget;
    public float _rightTarget;

    public float _conformSpeed = 1.0f;
    public float _paddleStrength = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePaddles();
    }

    private void CalculatePaddles()
    {
        _leftTarget = Mathf.Lerp(180 + _minRotation, 180 + _maxRotation, (_leftStick.y / 2) + 0.5f);
        _rightTarget = Mathf.Lerp(_maxRotation, _minRotation, (_rightStick.y / 2) + 0.5f);

        Vector3 leftRot = _leftTransform.localRotation.eulerAngles;
        Vector3 rightRot = _rightTransform.localRotation.eulerAngles;

        leftRot.y = _leftTarget;
        rightRot.y = _rightTarget;

        leftRot.x = _leftDown ? 25 : 0;
        rightRot.x = _rightDown ? 25 : 0;

        _leftTransform.localRotation = Quaternion.Lerp(_leftTransform.localRotation, Quaternion.Euler(leftRot), _conformSpeed * Time.deltaTime);
        _rightTransform.localRotation = Quaternion.Lerp(_rightTransform.localRotation, Quaternion.Euler(rightRot), _conformSpeed * Time.deltaTime);

        float lx = _leftTarget - _prevFrameLeft;
        if (_leftDown)
            _rb.AddForceAtPosition((lx < 0 ? -1 : (lx == 0.0f ? 0 : 1)) * _paddleStrength * Time.deltaTime * transform.right, transform.position - transform.forward);

        float rx = _rightTarget - _prevFrameRight;
        if (_rightDown)
            _rb.AddForceAtPosition((rx < 0 ? -1 : (rx == 0.0f ? 0 : 1)) * _paddleStrength * Time.deltaTime * -transform.right, transform.position + transform.forward);

        _prevFrameLeft = _leftTarget;
        _prevFrameRight = _rightTarget;
    }

    public void InputLeftPaddle(InputAction.CallbackContext context)
    {
        _leftStick = context.ReadValue<Vector2>();
    }

    public void InputLeftDown(InputAction.CallbackContext context)
    {
        _leftDown = context.ReadValueAsButton();
    }

    public void InputRightPaddle(InputAction.CallbackContext context)
    {
        _rightStick = context.ReadValue<Vector2>();
    }

    public void InputRightDown(InputAction.CallbackContext context)
    {
        _rightDown = context.ReadValueAsButton();
    }
}
