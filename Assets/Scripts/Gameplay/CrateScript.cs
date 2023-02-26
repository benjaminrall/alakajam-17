using UnityEngine;

namespace Gameplay
{
    public class CrateScript : MonoBehaviour
    {
        public GameObject brokenCrate;

        private void Start()
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), Water.WaterController.Instance.Collider);
        }

        public void OnCollisionEnter(Collision collision)
        {
            Instantiate(brokenCrate, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
