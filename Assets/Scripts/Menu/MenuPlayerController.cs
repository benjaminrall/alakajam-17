using UnityEngine;
using Water;

namespace Menu
{
    public class MenuPlayerController : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.inertiaTensor = Vector3.one;
            _rigidbody.centerOfMass = Vector3.zero;
        }

        void Update()
        {
            
        }
    }
}
