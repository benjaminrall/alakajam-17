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
        public Rigidbody bot;
        
        public MeshCollider Collider { get; private set; }
        
        private void Awake()
        {
            Collider = GetComponent<MeshCollider>();
        }

        private void Update()
        {
            if (applyWaterCurrents)
            {
                Vector3 playerForce = waterCurrents.Where(current => current.PlayerColliding).Aggregate(Vector3.zero, (current1, current) => current1 + current.Direction).normalized;
                Vector3 botForce = waterCurrents.Where(current => current.BotColliding).Aggregate(Vector3.zero, (current1, current) => current1 + current.Direction).normalized;
                
                player.AddForce(waterSpeed * playerForce, ForceMode.Acceleration);
                
                
                bot.AddForce(waterSpeed * botForce, ForceMode.Acceleration);
                Quaternion rotation = Quaternion.Slerp(bot.transform.localRotation, Quaternion.LookRotation(Quaternion.Euler(0, 90, 0) * botForce),
                    Time.deltaTime);
                
                bot.transform.localRotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            }
        }
    }
}
