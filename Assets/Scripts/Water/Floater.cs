using UnityEngine;

namespace Water
{
    public class Floater : MonoBehaviour
    {
        public Rigidbody rigidBody;
    
        public float depthBeforeSubmerged = .5f;
        public float displacementAmount = 1f;
        public int floaterCount = 1;
        public float waterDrag = 1f;
        public float waterAngularDrag = 1f;
        public float waterNoiseAdjustment = 0.25f;

        private void FixedUpdate()
        {
            Vector3 position = transform.position - Vector3.up * waterNoiseAdjustment;
        
            rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, position, ForceMode.Acceleration);
        
            Ray ray = new (position + Vector3.up * 20, Vector3.down);
        
            if (!WaterController.Instance.Collider.Raycast(ray, out RaycastHit hit, Mathf.Infinity) || position.y >= hit.point.y) return;
        
            float displacementMul = Mathf.Clamp01((hit.point.y - position.y) / depthBeforeSubmerged) * displacementAmount;
        
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMul), position, ForceMode.Acceleration);
            rigidBody.AddForce(displacementMul * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMul * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
