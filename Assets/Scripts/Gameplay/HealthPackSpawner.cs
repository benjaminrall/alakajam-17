using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviour
{
    public GameObject _healthPack;
    public int _numberOfHealthPacks = 6;

    void Start()
    {
        List<Transform> healthPacks = new List<Transform>();
        foreach (Transform child in transform)
        {
            healthPacks.Add(child);
        }

        int n = healthPacks.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Transform value = healthPacks[k];
            healthPacks[k] = healthPacks[n];
            healthPacks[n] = value;
        }

        for (int i = 0; i < _numberOfHealthPacks; i++)
        {
            Instantiate(_healthPack, healthPacks[i].position, Quaternion.identity);
        }
    }

}
