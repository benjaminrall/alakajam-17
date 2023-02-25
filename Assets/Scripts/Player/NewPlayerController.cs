using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerController : MonoBehaviour
{
    Rigidbody _rb;

    public bool _leftDown;
    public bool _rightDown;

    public AnimationCurve _paddleAnimationCurve;

    [Range(0.0f, 1.0f)]
    public float _leftPaddlePosition = 1.0f;
    [Range(0.0f, 1.0f)]
    public float _rightPaddlePosition = 1.0f;

    public Transform _leftPaddle;
    public Transform _rightPaddle;

    public float _paddleSpeed;

    public float _leftTarget;
    public float _rightTarget;

    public float _lastLeftPaddle;
    public float _lastRightPaddle;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _rb.AddForce(WaterCurrentController.Instance.GetCurrentDirection(transform.position) * 0.25f, ForceMode.Acceleration);

        _leftTarget = _leftDown ? 0 : 1;
        _rightTarget = _rightDown ? 0 : 1;

        _leftPaddlePosition = Mathf.Lerp(_leftPaddlePosition, _leftTarget, _paddleSpeed * Time.deltaTime);
        _rightPaddlePosition = Mathf.Lerp(_rightPaddlePosition, _rightTarget, _paddleSpeed * Time.deltaTime);

        /*
        if (_leftDown && _lastPaddleLeft < (Time.time - _paddleDelay))
        {
            _rb.AddForceAtPosition(_paddleStrength * -transform.right, transform.position + transform.forward);
            _rb.AddForceAtPosition(_paddleStrength * transform.right * -3, transform.position);

            _lastPaddleLeft = Time.time;
        }
        if (_rightDown && _lastPaddleRight < (Time.time - _paddleDelay))
        {
            _rb.AddForceAtPosition(_paddleStrength * -transform.right, transform.position - transform.forward);
            _rb.AddForceAtPosition(_paddleStrength * transform.right * -3, transform.position);

            _lastPaddleRight = Time.time;
        }
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        Debug.DrawRay(transform.position, transform.right, Color.red);
        */

        UpdatePaddlePos();
    }

    void UpdatePaddlePos()
    {
        Vector3 leftRot = _leftPaddle.localRotation.eulerAngles;
        leftRot.y = 120 + _leftPaddlePosition * 120;
        leftRot.x = _paddleAnimationCurve.Evaluate(1 - _leftPaddlePosition) * 25;
        _leftPaddle.localRotation = Quaternion.Euler(leftRot);

        Vector3 rightRot = _rightPaddle.localRotation.eulerAngles;
        rightRot.y = 60 - _rightPaddlePosition * 120;
        rightRot.x = _paddleAnimationCurve.Evaluate(1 - _rightPaddlePosition) * 25;
        _rightPaddle.localRotation = Quaternion.Euler(rightRot);
    }

    public void InputLeftDown(InputAction.CallbackContext context)
    {
        _leftDown = context.ReadValueAsButton();

        if (context.action.WasPressedThisFrame())
            _lastLeftPaddle = Time.time;
    }

    public void InputRightDown(InputAction.CallbackContext context)
    {
        _rightDown = context.ReadValueAsButton();

        if (context.action.WasPressedThisFrame())
            _lastRightPaddle = Time.time;
    }
}
