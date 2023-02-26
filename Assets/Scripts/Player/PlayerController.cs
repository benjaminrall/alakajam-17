using System;
using System.Collections;
using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Water;

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

        public float forwardSpeed = 50f;
        public float sidewaysSpeed = 10f;
        
        public float maxHealth = 5f;
        public Slider healthSlider;
        public Gradient healthGradient;
        public Image healthFill;
        public Image healthBorder;
        public float healthDisplayTime = 1;
        public float healthFadeOutTime = 1;
        
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
        
        public float Health { get; private set; }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.inertiaTensor = Vector3.one;
            _rigidbody.centerOfMass = Vector3.zero;
            
            foreach (Collider c in transform.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(c, WaterController.Instance.Collider);
            }

            ResetHealthSlider();
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

            if (Vector3.Angle(Vector3.up, transform.up) > 95)
            {
                RespawnPlayer();
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

        public void RespawnPlayer()
        {
            CheckpointManager.Instance.Respawn(transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Checkpoint"))
            {
                other.GetComponent<Checkpoint>().TryUpdateCheckpoint();
            } else if (other.CompareTag("HealthPack"))
            {
                Destroy(other.gameObject);
                Debug.Log("Get Health Pack");
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            Health -= _rigidbody.velocity.magnitude;
            
            if (Health <= 0)
            {
                ResetHealthSlider();
                RespawnPlayer();
            }
            else
            {
                UpdateHealthSlider();
            }
        }

        private void UpdateHealthSlider()
        {
            healthSlider.value = Health / maxHealth;
            healthFill.color = healthGradient.Evaluate(Health / maxHealth);
            SetAlpha(healthBorder, 1f);
            SetAlpha(healthFill, 1f);
            StartCoroutine(FadeOutHealth());
        }

        private void ResetHealthSlider()
        {
            Health = maxHealth;
            healthSlider.value = 1;
            healthFill.color = healthGradient.Evaluate(1);
            SetAlpha(healthBorder, 0f);
            SetAlpha(healthFill, 0f);
        }

        private static void SetAlpha(Image image, float alpha)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
    
        private IEnumerator FadeOutHealth()
        {
            yield return new WaitForSeconds(healthDisplayTime);
            
            for (float i = healthFadeOutTime; i >= 0f; i -= Time.deltaTime)
            {
                SetAlpha(healthBorder, 255 * (i / healthFadeOutTime) / 255f);
                SetAlpha(healthFill, 255 * (i / healthFadeOutTime) / 255f);
                yield return null;
            }
        }
    }
}
