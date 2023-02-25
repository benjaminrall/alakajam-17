using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private struct Inputs
        {
            public bool MoveLeftPaddle;
            public bool MoveRightPaddle;
            public bool LeftPaddleDown;
            public bool RightPaddleDown;
        }
        
        public Transform leftPaddle;
        public Transform rightPaddle;

        public Vector2 paddleMovementSpeed;

        public float forwardSpeed = 4.5f;
        public float sidewaysSpeed = 2f;

        private Rigidbody _rigidbody;

        private Inputs _inputs;

        private bool _leftPaddleDown;
        private bool _rightPaddleDown;
        
        private float _leftPaddlePosition;
        private float _rightPaddlePosition;
        private float _leftPaddleHeight;
        private float _rightPaddleHeight;

        private Vector3 _previousLeftPaddlePosition;
        private Vector3 _previousRightPaddlePosition;

        private float _leftTargetPosition;
        private float _rightTargetPosition;
        private float _leftTargetHeight;
        private float _rightTargetHeight;


        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _leftTargetPosition = _inputs.MoveLeftPaddle ? 1 : 0;
            _rightTargetPosition = _inputs.MoveRightPaddle ? 1 : 0;
            _leftTargetHeight = _inputs.LeftPaddleDown ? 1 : 0;
            _rightTargetHeight = _inputs.RightPaddleDown ? 1 : 0;
            
            _previousLeftPaddlePosition = leftPaddle.GetChild(0).position;
            _previousRightPaddlePosition = rightPaddle.GetChild(0).position;
            
            UpdatePaddlePositions();

            //Debug.Log(transform.localRotation.eulerAngles.y);
            //Vector3 adjustedSpeed = Quaternion.Euler(0, -transform.localRotation.eulerAngles.y, 0) * speed;
            //adjustedSpeed = new Vector3(Mathf.Abs(adjustedSpeed.x), 0, Mathf.Abs(adjustedSpeed.z));
            //Debug.Log(Mathf.Abs(adjustedSpeed.x) + " " + Mathf.Abs(adjustedSpeed.z));
            //Debug.Log(Quaternion.Euler(0, transform.eulerAngles.y, 0) * Vector3.right);
            //Debug.Log(transform.eulerAngles.y);

            if (_leftPaddleHeight > 0.8 && _inputs.LeftPaddleDown)
            {
                Vector3 movement = _previousLeftPaddlePosition - leftPaddle.GetChild(0).position;
                Vector3 rotatedMovement = Quaternion.Euler(0, -transform.localRotation.eulerAngles.y, 0) * movement;
                Vector3 adjustedMovement = new(forwardSpeed * rotatedMovement.x, 0, sidewaysSpeed * rotatedMovement.z);
                movement = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0) * adjustedMovement;
                _rigidbody.AddForceAtPosition(movement, leftPaddle.GetChild(0).position, ForceMode.Acceleration);
            }
            
            if (_rightPaddleHeight > 0.8 && _inputs.RightPaddleDown)
            {
                Vector3 movement = _previousRightPaddlePosition - rightPaddle.GetChild(0).position;
                Vector3 rotatedMovement = Quaternion.Euler(0, -transform.localRotation.eulerAngles.y, 0) * movement;
                Vector3 adjustedMovement = new(forwardSpeed * rotatedMovement.x, 0, sidewaysSpeed * rotatedMovement.z);
                movement = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0) * adjustedMovement;
                _rigidbody.AddForceAtPosition(movement, rightPaddle.GetChild(0).position, ForceMode.Acceleration);
            }
        }

        private void UpdatePaddlePositions()
        {
            Vector3 leftRot = leftPaddle.localRotation.eulerAngles;
            _leftPaddlePosition = Mathf.Lerp(_leftPaddlePosition, _leftTargetPosition, paddleMovementSpeed.x * Time.deltaTime);
            _leftPaddleHeight = Mathf.Lerp(_leftPaddleHeight, _leftTargetHeight, paddleMovementSpeed.y * Time.deltaTime);
            
            leftRot.x = _leftPaddleHeight * 40;
            leftRot.y = 120 + (1 - _leftPaddlePosition) * 120;
            leftPaddle.localRotation = Quaternion.Euler(leftRot);
            
            Vector3 rightRot = rightPaddle.localRotation.eulerAngles;
            _rightPaddlePosition = Mathf.Lerp(_rightPaddlePosition, _rightTargetPosition, paddleMovementSpeed.x * Time.deltaTime);
            _rightPaddleHeight = Mathf.Lerp(_rightPaddleHeight, _rightTargetHeight, paddleMovementSpeed.y * Time.deltaTime);
            
            rightRot.x = _rightPaddleHeight * 40;
            rightRot.y = 60 - (1 - _rightPaddlePosition) * 120;
            rightPaddle.localRotation = Quaternion.Euler(rightRot);
        }
        
        public void MoveLeftPaddleInput(InputAction.CallbackContext context)
        {
            _inputs.MoveLeftPaddle = context.ReadValueAsButton();
        }
        
        public void MoveRightPaddleInput(InputAction.CallbackContext context)
        {
            _inputs.MoveRightPaddle = context.ReadValueAsButton();
        }

        public void LeftPaddleDownInput(InputAction.CallbackContext context)
        {
            _inputs.LeftPaddleDown = context.ReadValueAsButton();
        }

        public void RightPaddleDownInput(InputAction.CallbackContext context)
        {
            _inputs.RightPaddleDown = context.ReadValueAsButton();
        }
    }
}
