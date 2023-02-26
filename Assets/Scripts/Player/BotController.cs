using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class BotController : Character
    {
        private enum State
        {
            Drifting,
            Forward,
            Backward,
            Left,
            SharpLeft,
            Right,
            SharpRight
        }
        
        private State _state;

        public float sightRayLength;
        public LayerMask sightRayMask;

        private new void Start()
        {
            _state = State.Drifting;
            base.Start();
        }

        private IEnumerable<(Vector3, Vector3)> GetRays()
        {
            Vector3 origin = transform.position + .5f * Vector3.up;
            Vector3 forward = -transform.right;
            Vector3 right = transform.forward;

            Vector3 frontOrigin = origin + 1.85f * forward;
            
            // Forward ray
            yield return (frontOrigin, forward);
            
            // Shallow Diagonals
            yield return (frontOrigin, (forward + 0.5f * right).normalized);
            yield return (frontOrigin, (forward - 0.5f * right).normalized);
            
            // Steep Diagonals
            yield return (origin + forward, (0.5f * forward + right).normalized);
            yield return (origin + forward, (0.5f * forward - right).normalized);
            
            // Backward ray
            yield return (origin - 1.85f * forward, -forward);
        }

        private new void Update()
        {
            if (_state == State.Drifting)
            {
                bool front = Physics.Raycast(transform.position, transform.right, sightRayLength, sightRayMask);
                bool left = Physics.Raycast(transform.position, transform.forward, sightRayLength, sightRayMask);

                Vector3 origin = transform.position + .5f * Vector3.up;
                


                foreach ((Vector3 o, Vector3 d) in GetRays())
                {
                    Debug.DrawRay(o, sightRayLength * d, Color.red);
                }
                
                Debug.DrawRay(origin, sightRayLength * transform.forward, Color.green); //right
                
                Debug.DrawRay(origin, sightRayLength * -transform.forward, Color.yellow); //left
            }
            base.Update();
        }
    }
}
