using System;
using Gameplay;
using UnityEngine;
using Water;

namespace Player
{
    public abstract class Character : MonoBehaviour
    {
        public Transform leftPaddle;
        public Transform rightPaddle;

        public Vector2 paddleMovementSpeed;

        public float forwardSpeed = 50f;
        public float sidewaysSpeed = 10f;
        
        public float maxHealth = 4f;

        protected Rigidbody _rigidbody;

        protected float _leftPaddlePosition;
        protected float _rightPaddlePosition;
        protected float _leftPaddleHeight;
        protected float _rightPaddleHeight;
        
        protected float _leftTargetPosition;
        protected float _rightTargetPosition;
        protected float _leftTargetHeight;
        protected float _rightTargetHeight;
        
        protected Vector3 _previousLeftPaddlePosition;
        protected Vector3 _previousRightPaddlePosition;

        protected float _health;
        
        public int CurrentCheckpointIndex { get; private set; }

        protected void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.inertiaTensor = Vector3.one;
            _rigidbody.centerOfMass = Vector3.zero;
            
            foreach (Collider c in transform.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(c, WaterController.Instance.Collider);
            }
            
            _health = maxHealth;
        }

        protected void Update()
        {
            if (Vector3.Angle(Vector3.up, transform.up) > 95)
            {
                Respawn();
                return;
            }
            
            _previousLeftPaddlePosition = leftPaddle.GetChild(0).position;
            _previousRightPaddlePosition = rightPaddle.GetChild(0).position;
            
            UpdatePaddlePositions();
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

        protected Vector3 CalculateAdjustedMovement(Vector3 movement)
        {
            Quaternion localRotation = transform.localRotation;
            Vector3 rotatedMovement = Quaternion.Euler(0, -localRotation.eulerAngles.y, 0) * movement;
            Vector3 adjustedMovement = new(forwardSpeed * rotatedMovement.x, 0, sidewaysSpeed * rotatedMovement.z);
            return Quaternion.Euler(0, localRotation.eulerAngles.y, 0) * adjustedMovement;
        }

        public void Respawn()
        {
            CheckpointManager.Instance.Respawn(transform);
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Checkpoint"))
            {
                CurrentCheckpointIndex = other.GetComponent<Checkpoint>().TryUpdateCheckpoint(CurrentCheckpointIndex);
            }
            else if (other.CompareTag("HealthPack"))
            {
                Destroy(other.gameObject);
                _health = maxHealth;
            }
        }

        protected void OnCollisionEnter(Collision other)
        {
            _health -= _rigidbody.velocity.magnitude;

            if (_health <= 0)
            {
                Respawn();
                _health = maxHealth;
            }
        }
    }
}
