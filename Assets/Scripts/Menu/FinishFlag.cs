using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishFlag : MonoBehaviour
{
    public static bool _playerWon = true;

    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
