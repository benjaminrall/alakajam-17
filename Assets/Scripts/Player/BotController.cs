using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public class BotController : Character
    {
        private static readonly int[] ForwardMasks = {0b0000000, 0b0000011};
        private static readonly int[] LeftMasks = {0b1010101, 0b1010100, 0b0010101, 0b0010100};
        private static readonly int[] RightMasks = {0b1101010, 0b1101000, 0b0101010, 0b0101000};
        private static readonly int[] SmallLeftMasks = {0b1000101, 0b1010101, 0b0000101, 0b0010101};
        private static readonly int[] SmallRightMasks = {0b1001010, 0b1101010, 0b0001010, 0b0101010};
        
        private enum State
        {
            Drifting,
            Forward,
            Left,
            SmallLeft,
            Right,
            SmallRight
        }

        private State _state;

        public float sightRayLength;
        public LayerMask sightRayMask;

        private Queue<((float, float), float)> _actionQueue;

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
            yield return (frontOrigin, (forward - 0.5f * right).normalized);
            yield return (frontOrigin, (forward + 0.5f * right).normalized);
            
            // Steep Diagonals
            yield return (origin + forward, (0.5f * forward - right).normalized);
            yield return (origin + forward, (0.5f * forward + right).normalized);
            
            // Sideways diagonals
            yield return (origin, -right);
            yield return (origin, right);
        }

        private new void Update()
        {
            foreach ((Vector3 origin, Vector3 direction) in GetRays())
            {
                Debug.DrawRay(origin, sightRayLength * direction, Color.red);
            }

            switch (_state)
            {
                case State.Drifting:
                    int input = 0b0;
                    foreach ((Vector3 origin, Vector3 direction) in GetRays())
                    {
                        input <<= 1;
                        if (Physics.Raycast(origin, direction, sightRayLength, sightRayMask))
                            input += 1;
                    }

                    _state = DecideNextState(input);
                    break;
                case State.Forward:
                    break;
                case State.Left:
                    break;
                case State.SmallLeft:
                    break;
                case State.Right:
                    break;
                case State.SmallRight:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            base.Update();
        }

        private static State DecideNextState(int input)
        {
            if (LeftMasks.Any(mask => (input & mask) == mask))
                return State.Left;
            
            if (RightMasks.Any(mask => (input & mask) == mask))
                return State.Right;
            
            if (SmallLeftMasks.Any(mask => (input & mask) == mask))
                return State.SmallLeft;
            
            if (SmallRightMasks.Any(mask => (input & mask) == mask))
                return State.SmallRight;
            
            if (ForwardMasks.Any(mask => (input & mask) == mask))
                return State.Forward;

            return State.Drifting;
        }
    }
}
