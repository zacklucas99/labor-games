using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{

    public GameObject PotFracture;
    private Rigidbody rb;
    public float breakVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.forward * 10, ForceMode.Impulse); // just to debug
    }
    private void OnCollisionEnter(Collision collision)
    {
        var potFractureInstantiate = Instantiate(PotFracture);
        potFractureInstantiate.transform.position = transform.position;
        
        for(int i = 0; i < PotFracture.transform.childCount; i++)
        {
            var child_rb = potFractureInstantiate.transform.GetChild(i).GetComponent<Rigidbody>();
            if (child_rb != null)
            {
                child_rb.velocity = rb.velocity;
            }
        }
        Destroy(this.gameObject);
    }
}
