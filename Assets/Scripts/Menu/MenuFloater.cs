using UnityEngine;

namespace Menu
{
    public class MenuFloater : MonoBehaviour
    {

        public Rigidbody rigidBody;
    
        public float depthBeforeSubmerged = .5f;
        public float displacementAmount = 1f;
        public int floaterCount = 1;
        public float waterDrag = 1f;
        public float waterAngularDrag = 0.5f;

        private void FixedUpdate()
        {
            Vector3 position = transform.position;
        
            rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, position, ForceMode.Acceleration);

            float waveHeight = MenuWaveManager.Instance.GetWaveHeight(position.x, position.z);
            
            if (position.y >= waveHeight) return;
        
            float displacementMul = Mathf.Clamp01((waveHeight - position.y) / depthBeforeSubmerged) * displacementAmount;
        
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMul), position, ForceMode.Acceleration);
            rigidBody.AddForce(displacementMul * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMul * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
