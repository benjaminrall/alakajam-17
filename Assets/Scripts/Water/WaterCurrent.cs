using UnityEngine;

namespace Water
{
    public class WaterCurrent : MonoBehaviour
    {
        public bool Colliding { get; private set; }
    
        public Vector3 Direction { get; private set; }

        public void Start()
        {
            Direction = transform.GetChild(0).forward;
            Colliding = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Colliding = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Colliding = false;
            }
        }
    }
}
