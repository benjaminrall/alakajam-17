using System.Collections;
using Player;
using UnityEngine;

namespace Gameplay
{
    public class CheckpointManager : Singleton<CheckpointManager>
    {
        public Transform[] checkpoints;

        public int _currentCheckpointIndex { get; private set; }
        private int _finishIndex;

        public void Start()
        {
            for (int i = 0; i < checkpoints.Length; i++)
            {
                checkpoints[i].GetComponent<Checkpoint>().Index = i;
            }
            _finishIndex = checkpoints.Length - 1;
        }

        public void TryUpdateCheckpointIndex(int newIndex)
        {
            if (newIndex > _currentCheckpointIndex)
            {
                _currentCheckpointIndex = newIndex;
            }

            if (_currentCheckpointIndex == _finishIndex)
            {
                Debug.Log("Finished");
            }
        }

        public void Respawn(Transform player)
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            player.rotation = checkpoints[_currentCheckpointIndex].rotation;
            player.position = checkpoints[_currentCheckpointIndex].position;
        }

        public float GetCompletionPercentage(Vector3 position, int checkpointIndex)
        {
            Vector3 pos = checkpoints[checkpointIndex].position;
            try {
                Vector3 pos2 = checkpoints[checkpointIndex + 1].position;

                Vector3 dir = pos2 - pos;
                Vector3 playeroffset = position - pos;
                dir.y = 0;
                playeroffset.y = 0;
                float magnitude = dir.magnitude;
                float playermagnitude = playeroffset.magnitude;

                float checkpointCompletion = playermagnitude / magnitude;

                return ((((float)checkpointIndex + 1.0f) / _finishIndex) - 
                        ((float)checkpointIndex / _finishIndex)) * checkpointCompletion + 
                        ((float)checkpointIndex / _finishIndex);
            } catch {
                return 1.0f;
            }
        }
    }
}
