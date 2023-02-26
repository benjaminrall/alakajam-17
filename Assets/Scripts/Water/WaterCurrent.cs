using UnityEngine;

namespace Water
{
    public class WaterCurrent : MonoBehaviour
    {
        public bool PlayerColliding { get; private set; }
        public bool BotColliding { get; private set; }
    
        public Vector3 Direction { get; private set; }

        public void Start()
        {
            Direction = transform.GetChild(0).forward;
            PlayerColliding = false;
            BotColliding = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerColliding = true;
            }

            if (other.CompareTag("Bot"))
            {
                BotColliding = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerColliding = false;
            }

            if (other.CompareTag("Bot"))
            {
                BotColliding = false;
            }
        }
    }
}
