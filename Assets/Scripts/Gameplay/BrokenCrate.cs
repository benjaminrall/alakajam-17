using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenCrate : MonoBehaviour
{

    public float destroyDelay;

    void Start()
    {
        foreach (Transform child in transform)
        {
            Physics.IgnoreCollision(child.GetComponent<Collider>(), Water.WaterController.Instance.Collider);
            Physics.IgnoreCollision(child.GetComponent<Collider>(), FindObjectOfType<Player.PlayerController>().transform.GetComponentInChildren<Collider>());
        }

        StartCoroutine(DestroyPieces());
    }

    IEnumerator DestroyPieces()
    {
        yield return new WaitForSeconds(destroyDelay);
        foreach (Transform child in transform)
        {
            Destroy(child.GetComponent<Water.Floater>());
        }
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
