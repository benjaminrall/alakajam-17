using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : Singleton<CheckpointManager>
{
    public Transform[] _checkpoints;

    public int _currentCheckpointIndex = 0;
    public int _finishIndex;

    public void Start()
    {
        for (int i = 0; i < _checkpoints.Length; i++)
        {
            _checkpoints[i].GetComponent<Checkpoint>().checkpointIndex = i;
        }
        _finishIndex = _checkpoints.Length - 1;
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
        player.position = _checkpoints[_currentCheckpointIndex].position;
        player.rotation = Quaternion.identity;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
