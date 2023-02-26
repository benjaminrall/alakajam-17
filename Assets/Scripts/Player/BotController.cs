using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public class BotController : Character
    {
        //private static readonly int[] ForwardMasks = {0b0000000, 0b0000011};
        //private static readonly int[] LeftMasks = {0b1010101, 0b1010100, 0b0010101, 0b0010100};
        //private static readonly int[] SmallLeftMasks = {0b1000101, 0b1010101, 0b0000101, 0b0010101};
        //private static readonly int[] RightMasks = {0b1101010, 0b1101000, 0b0101010, 0b0101000};
        //private static readonly int[] SmallRightMasks = {0b1001010, 0b1101010, 0b0001010, 0b0101010};

        //private static readonly int[] ForwardMasks = {0b000000, 0b000011};
        //private static readonly int[] LeftMasks = {0b010100,0b010101,0b011100,0b011101,0b110100,0b110101,0b111100,0b111101};
        //private static readonly int[] SmallLeftMasks = {0b000101,0b001101,0b100101,0b101101};
        //private static readonly int[] RightMasks = {0b101000,0b101010,0b101100,0b101110,0b111000,0b111010,0b111100,0b111110};
        //private static readonly int[] SmallRightMasks = {0b001010,0b001110,0b011010,0b011110};

        private static readonly int[] ForwardMasks = {0b0000, 0b1111, 0b1100, 0b0011};
        //0000
        //1111
        //1100
        //0011
        
        private static readonly int[] LeftMasks = {0b0101,0b0100,0b0111,0b1101};
        //0101
        //0100
        //0111
        //1101
        
        private static readonly int[] SmallLeftMasks = {0b0001,0b0110};
        //0001
        //0110
        
        private static readonly int[] RightMasks = {0b1010,0b1000,0b1011,0b1110};
        //1010
        //1000
        //1011
        //1110
        
        private static readonly int[] SmallRightMasks = {0b0010,0b1001};
        //0010
        //1001

        private static readonly (((float, float), (float, float)), float)[] ForwardActions =
        {
            (((0, 1), (0, 1)), .3f),
            (((1, 1), (1, 1)), .6f),
            (((1, 0), (1, 0)), .3f),
            (((0, 0), (0, 0)), .6f),
        };
        
        private static readonly (((float, float), (float, float)), float)[] LeftActions =
        {
            (((0, 0), (0, 1)), .3f),
            (((0, 0), (.5f, 1)), .2f),
            (((0, 0), (.5f, 0)), .3f),
            (((0, 0), (0, 0)), .6f),
        };
        
        private static readonly (((float, float), (float, float)), float)[] SmallLeftActions =
        {
            (((0, 1), (0, 1)), .1f),
            (((.1f, 1), (.5f, 1)), .05f),
            (((.1f, 0), (.5f, 0)), .3f),
            (((0, 0), (0, 0)), .6f),
        };
        
        private static readonly (((float, float), (float, float)), float)[] RightActions =
        {
            (((0, 1), (0, 0)), .3f),
            (((.5f, 1), (0, 0)), .2f),
            (((.5f, 0), (0, 0)), .3f),
            (((0, 0), (0, 0)), .6f),
        };

        private static readonly (((float, float), (float, float)), float)[] SmallRightActions =
        {
            (((0, 1), (0, 1)), .1f),
            (((.5f, 1), (.1f, 1)), .05f),
            (((.5f, 0), (.1f, 0)), .3f),
            (((0, 0), (0, 0)), .6f),
        };
        
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

        private Queue<(((float, float), (float, float)), float)> _actionQueue;
        private (((float pos, float h) left, (float pos, float h) right) state, float t) _currentAction;
        private float _currentActionDuration;
        private bool _actionInProgress;

        private new void Start()
        {
            _state = State.Drifting;
            _actionQueue = new Queue<(((float, float), (float, float)), float)>();
            base.Start();
        }

        private IEnumerable<(Vector3, Vector3)> GetRays()
        {
            Vector3 origin = transform.position + .5f * Vector3.up;
            Vector3 forward = -transform.right;
            Vector3 right = transform.forward;

            Vector3 frontOrigin = origin + 1.85f * forward;

            // Steep Diagonals
            yield return (origin + forward, (0.5f * forward - right).normalized);
            yield return (origin + forward, (0.5f * forward + right).normalized);

            // Shallow Diagonals
            yield return (frontOrigin, (forward - 0.5f * right).normalized);
            yield return (frontOrigin, (forward + 0.5f * right).normalized);

            // Sideways diagonals
            //yield return (origin, -right);
            //yield return (origin, right);
        }

        private new void Update()
        {
            if (_actionInProgress)
            {
                _currentActionDuration += Time.deltaTime;
                if (_currentActionDuration > _currentAction.t)
                {
                    _actionInProgress = false;
                }
            }

            foreach ((Vector3 origin, Vector3 direction) in GetRays())
            {
                Debug.DrawRay(origin, sightRayLength * direction, Color.red);
            }

            _leftTargetHeight = _leftTargetPosition = _rightTargetHeight = _rightTargetPosition = 0;
            
            if (_actionQueue.Count == 0)
            {
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
                        Debug.Log(_state);
                        break;
                    case State.Forward:
                        foreach ((((float, float), (float, float)), float) action in ForwardActions)
                        {
                            _actionQueue.Enqueue(action);
                        }
                        _state = State.Drifting;
                        break;
                    case State.Left:
                        foreach ((((float, float), (float, float)), float) action in LeftActions)
                        {
                            _actionQueue.Enqueue(action);
                        }
                        _state = State.Drifting;
                        break;
                    case State.SmallLeft:
                        foreach ((((float, float), (float, float)), float) action in SmallLeftActions)
                        {
                            _actionQueue.Enqueue(action);
                        }
                        _state = State.Drifting;
                        break;
                    case State.Right:
                        foreach ((((float, float), (float, float)), float) action in RightActions)
                        {
                            _actionQueue.Enqueue(action);
                        }
                        _state = State.Drifting;
                        break;
                    case State.SmallRight:
                        foreach ((((float, float), (float, float)), float) action in SmallRightActions)
                        {
                            _actionQueue.Enqueue(action);
                        }
                        _state = State.Drifting;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                if (!_actionInProgress)
                {
                    _currentAction = _actionQueue.Dequeue();
                    _currentActionDuration = 0;
                    _actionInProgress = true;
                }

                _leftTargetPosition = _currentAction.state.left.pos;
                _leftTargetHeight = _currentAction.state.left.h;
                _rightTargetPosition = _currentAction.state.right.pos;
                _rightTargetHeight = _currentAction.state.right.h;
            }

            base.Update();
        }

        private static State DecideNextState(int input)
        {
            if (LeftMasks.Any(mask => (input ^ mask) == 0))
                return State.Left;
            
            if (RightMasks.Any(mask => (input ^ mask) == 0))
                return State.Right;
            
            if (SmallLeftMasks.Any(mask => (input ^ mask) == 0))
                return State.SmallLeft;
            
            if (SmallRightMasks.Any(mask => (input ^ mask) == 0))
                return State.SmallRight;

            if (ForwardMasks.Any(mask => (input ^ mask) == 0))
                return State.Forward;

            return State.Drifting;
        }

        private new void Respawn()
        {
            _actionQueue.Clear();
            base.Respawn();
        }
    }
}
