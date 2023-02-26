using UnityEngine;

namespace Gameplay
{
    public class Checkpoint : MonoBehaviour
    {
        private int _index;

        public int TryUpdateCheckpoint(int current)
        {
            return CheckpointManager.Instance.TryUpdateCheckpointIndex(current, _index);
        }

        public void SetIndex(int i)
        {
            _index = i;
        }
    }
}
