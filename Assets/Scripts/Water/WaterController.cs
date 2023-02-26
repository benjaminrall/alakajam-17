using System;
using System.Linq;
using UnityEngine;

namespace Water
{
    public class WaterController : Singleton<WaterController>
    {
        public bool applyWaterCurrents = true;
        public float waterSpeed = .15f;
        public WaterCurrent[] waterCurrents;
        public Rigidbody player;

        public MeshCollider Collider { get; private set; }
        
        private void Awake()
        {
            Collider = GetComponent<MeshCollider>();
        }

        private void Update()
        {
            if (applyWaterCurrents)
            {
                Vector3 force = waterCurrents.Where(current => current.Colliding).Aggregate(Vector3.zero, (current1, current) => current1 + current.Direction).normalized;
                player.AddForce(waterSpeed * force, ForceMode.Acceleration);
            }
        }
    }
}
