using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex = -1;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void TryUpdateCheckpoint()
    {
        CheckpointManager.Instance.TryUpdateCheckpointIndex(checkpointIndex);
    }
}
