using UnityEngine;

namespace Player
{
    public class FloppyMan : MonoBehaviour
    {
        Rigidbody _rb;

        public Transform _bodyBone;

        public Transform _leftArm;
        public Transform _rightArm;

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

            Vector3 target = (Quaternion.LookRotation(-_rb.velocity * 0.4f + Vector3.up) * Quaternion.Euler(0, -90, -90)).eulerAngles;

            target.x = 0;
            target.y = 0;

            _bodyBone.localRotation = Quaternion.Euler(target);
            _bodyBone.localRotation = Quaternion.Lerp(_bodyBone.localRotation, Quaternion.identity, 5.0f * Time.deltaTime);

            _leftArm.rotation = Quaternion.LookRotation(_leftPaddle.position - _leftArm.position, Vector3.up) * Quaternion.Euler(-100, -15, 200);
            _rightArm.rotation = Quaternion.LookRotation(_rightPaddle.position - _rightArm.position, Vector3.up) * Quaternion.Euler(100, -175, -170);
        }
    }
}
