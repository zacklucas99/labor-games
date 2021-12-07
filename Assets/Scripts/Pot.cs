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
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("Collide:"+rb.velocity.magnitude);
        if(rb.velocity.magnitude < breakVelocity)
        {
            return;
        }
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
