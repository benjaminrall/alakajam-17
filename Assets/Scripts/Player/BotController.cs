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
        private static readonly int[] ForwardMasks = {0, 3, 1, 2};
        //0000
        //1111
        //1100
        //0011
        //0010
        //0001
        
        private static readonly int[] LeftMasks = {5,4,7,13};
        //0101
        //0100
        //0111
        //1101
        
        private static readonly int[] SmallLeftMasks = {6};
        //0110
        
        private static readonly int[] RightMasks = {10,8,11,14};
        //1010
        //1000
        //1011
        //1110
        
        private static readonly int[] SmallRightMasks = {0b1001};
        //1001

        private static readonly (((float, float), (float, float)), float)[] ForwardActions =
        {
            (((0, 1), (0, 1)), .2f),
            (((.5f, 1), (.5f, 1)), .08f),
            (((1, 0), (1, 0)), .2f),
            (((0, 0), (0, 0)), 1f),
        };
        
        private static readonly (((float, float), (float, float)), float)[] LeftActions =
        {
            (((0, 1), (0, 1)), .1f),
            (((.35f, 1), (1, 1)), .2f),
            (((.35f, 0), (1, 0)), .3f),
            (((0, 0), (0, 0)), .8f),
        };
        
        private static readonly (((float, float), (float, float)), float)[] SmallLeftActions =
        {
            (((0, 1), (0, 1)), .3f),
            (((.03f, 1), (.2f, 1)), .08f),
            (((.03f, 0), (.2f, 0)), .2f),
            (((0, 0), (0, 0)), .4f),
        };
        
        private static readonly (((float, float), (float, float)), float)[] RightActions =
        {
            (((0, 1), (0, 1)), .1f),
            (((1, 1), (.35f, 1)), .2f),
            (((1, 0), (.35f, 0)), .3f),
            (((0, 0), (0, 0)), .8f),
        };

        private static readonly (((float, float), (float, float)), float)[] SmallRightActions =
        {
            (((0, 1), (0, 1)), .3f),
            (((.2f, 1), (.03f, 1)), .08f),
            (((.2f, 0), (.03f, 0)), .2f),
            (((0, 0), (0, 0)), .4f),
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
            Vector3 origin = transform.position + Vector3.up;
            Vector3 forward = -transform.right;
            forward = new Vector3(forward.x, 0, forward.z).normalized;
            Vector3 right = transform.forward;
            right = new Vector3(right.x, 0, right.z).normalized;
            
            // Steep Diagonals
            

            // Shallow Diagonals
            yield return (origin, (forward - 0.5f * right).normalized);
            yield return (origin, (forward + 0.5f * right).normalized);
            yield return (origin, (0.5f * forward - right).normalized);
            yield return (origin, (0.5f * forward + right).normalized);
        }

        private int GetBotInput()
        {
            int input = 0;
            
            IEnumerable<(Vector3, Vector3)> rays = GetRays();
            foreach ((Vector3 origin, Vector3 direction) in rays)
            {
                input <<= 1;
                bool hit = Physics.Raycast(origin, direction, sightRayLength, sightRayMask);
                if (hit) input ++;
            }

            return input;
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

            //foreach ((Vector3 origin, Vector3 direction) in GetRays())
            //{
            //    Debug.DrawRay(origin, sightRayLength * direction, Color.red);
            //}

            _leftTargetHeight = _leftTargetPosition = _rightTargetHeight = _rightTargetPosition = 0;

            if (_actionQueue.Count == 0)
            {
                switch (_state)
                {
                    case State.Drifting:
                        _actionQueue.Clear();
                        int input = GetBotInput();
                        _state = DecideNextState(input);
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
            if (ForwardMasks.Any(mask => input == mask))
                return State.Forward;
            
            if (LeftMasks.Any(mask => input == mask))
                return State.Left;
            
            if (RightMasks.Any(mask => input == mask))
                return State.Right;
            
            if (SmallLeftMasks.Any(mask => input == mask))
                return State.SmallLeft;
            
            if (SmallRightMasks.Any(mask => input == mask))
                return State.SmallRight;

            return State.Drifting;
        }
        
        private new void OnCollisionEnter(Collision other) { }

        private new void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            if (CurrentCheckpointIndex == Gameplay.CheckpointManager.Instance._finishIndex)
            {
                FinishFlag._playerWon = false;
            }

            if (other.CompareTag("HealthPack"))
            {
                // TODO
                //base.UpdateHealthSlider();
            }
        }
    }
}
