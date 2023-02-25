using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int Index { get; set; }

    public void TryUpdateCheckpoint()
    {
        CheckpointManager.Instance.TryUpdateCheckpointIndex(Index);
    }
}
