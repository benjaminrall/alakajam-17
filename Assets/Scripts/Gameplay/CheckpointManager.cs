using System.Collections;
using Player;
using UnityEngine;

namespace Gameplay
{
    public class CheckpointManager : Singleton<CheckpointManager>
    {
        public Transform[] checkpoints;

        public int _finishIndex;

        public void Start()
        {
            for (int i = 0; i < checkpoints.Length; i++)
            {
                checkpoints[i].GetComponent<Checkpoint>().SetIndex(i);
            }
            _finishIndex = checkpoints.Length - 1;
        }

        public int TryUpdateCheckpointIndex(int currentIndex, int newIndex)
        {
            if (newIndex <= currentIndex) return currentIndex;
            
            if (newIndex == _finishIndex)
            {
                Debug.Log("Finished");
            }
            
            return newIndex;
        }

        public void Respawn(Transform player)
        {
            Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
            Character playerScript = player.GetComponent<Character>();
            int i = playerScript.CurrentCheckpointIndex;

            playerRigidbody.rotation = player.rotation = checkpoints[i].rotation;
            playerRigidbody.position = player.position = checkpoints[i].position + checkpoints[i].forward *
                (1.8f * (playerScript.GetType() == typeof(PlayerController) ? -1 : 1));

            playerRigidbody.velocity = playerRigidbody.angularVelocity = Vector3.zero;
        }

        public float GetCompletionPercentage(Vector3 position, int checkpointIndex)
        {
            Vector3 pos = checkpoints[checkpointIndex].position;
            try {
                Vector3 pos2 = checkpoints[checkpointIndex + 1].position;

                Vector3 dir = pos2 - pos;
                Vector3 playerOffset = position - pos;
                dir.y = 0;
                playerOffset.y = 0;
                float magnitude = dir.magnitude;
                float playerMagnitude = playerOffset.magnitude;

                float checkpointCompletion = playerMagnitude / magnitude;

                return ((checkpointIndex + 1f) / _finishIndex - 
                        (float) checkpointIndex / _finishIndex) * checkpointCompletion + 
                        (float) checkpointIndex / _finishIndex;
            } catch {
                return 1f;
            }
        }
    }
}
