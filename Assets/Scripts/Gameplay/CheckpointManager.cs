using System.Collections;
using Player;
using UnityEngine;

namespace Gameplay
{
    public class CheckpointManager : Singleton<CheckpointManager>
    {
        public Transform[] checkpoints;

        private int _currentCheckpointIndex;
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
    }
}
