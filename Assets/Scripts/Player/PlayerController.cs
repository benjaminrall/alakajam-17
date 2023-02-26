using System;
using System.Collections;
using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Water;

namespace Player
{
    public class PlayerController : Character
    {
        private struct Inputs
        {
            public bool MoveLeftPaddle;
            public bool MoveRightPaddle;
            public bool LeftPaddleDown;
            public bool RightPaddleDown;
        }
        
        public Slider healthSlider;
        public Gradient healthGradient;
        public Image healthFill;
        public Image healthBorder;
        public float healthDisplayTime = .75f;
        public float healthFadeOutTime = .25f;
        
        private Inputs _inputs;
        
        private new void Start()
        {
            base.Start();
            ResetHealthSlider();
        }

        private new void Update()
        {
            _leftTargetPosition = _inputs.MoveLeftPaddle ? 1 : 0;
            _rightTargetPosition = _inputs.MoveRightPaddle ? 1 : 0;
            _leftTargetHeight = _inputs.LeftPaddleDown ? 1 : 0;
            _rightTargetHeight = _inputs.RightPaddleDown ? 1 : 0;
            
            base.Update();
            
            if (_leftPaddleHeight > 0.8 && _inputs.LeftPaddleDown)
            {
                Vector3 movement = CalculateAdjustedMovement(_previousLeftPaddlePosition - leftPaddle.GetChild(0).position);
                _rigidbody.AddForceAtPosition(movement, leftPaddle.GetChild(0).position, ForceMode.Acceleration);
            }
            
            if (_rightPaddleHeight > 0.8 && _inputs.RightPaddleDown)
            {
                Vector3 movement = CalculateAdjustedMovement(_previousRightPaddlePosition - rightPaddle.GetChild(0).position);
                _rigidbody.AddForceAtPosition(movement, rightPaddle.GetChild(0).position, ForceMode.Acceleration);
            }
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
        

        private new void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if (other.CompareTag("HealthPack"))
            {
                UpdateHealthSlider();
            }
        }

        private new void OnCollisionEnter(Collision other)
        {
            base.OnCollisionEnter(other);
            
            if (_health <= 0)
            {
                ResetHealthSlider();
            }
            else
            {
                UpdateHealthSlider();
            }
        }

        private void UpdateHealthSlider()
        {
            healthSlider.value = _health / maxHealth;
            healthFill.color = healthGradient.Evaluate(_health / maxHealth);
            SetAlpha(healthBorder, 1f);
            SetAlpha(healthFill, 1f);
            StartCoroutine(FadeOutHealth());
        }

        private void ResetHealthSlider()
        {
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
