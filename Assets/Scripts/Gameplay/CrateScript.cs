using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : MonoBehaviour
{
    public GameObject brokenCrate;

    private void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), Water.WaterController.Instance.Collider);
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject.Instantiate(brokenCrate, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
