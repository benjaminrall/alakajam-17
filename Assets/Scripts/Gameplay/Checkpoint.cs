using UnityEngine;

namespace Gameplay
{
    public class Checkpoint : MonoBehaviour
    {
        public int Index { get; set; }

        public void TryUpdateCheckpoint()
        {
            CheckpointManager.Instance.TryUpdateCheckpointIndex(Index);
        }
    }
}