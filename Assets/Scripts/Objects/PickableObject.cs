using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject parentObject;

    public BoxCollider boxCollider;
    public Rigidbody this_rigidbody;

    // Update is called once per frame
    void Update()
    {
        if (parentObject)
        {
            transform.position = parentObject.transform.position;
            transform.rotation = parentObject.transform.rotation;
        }
    }

    public void ResetCollider()
    {
        boxCollider.enabled = false;
        this_rigidbody.isKinematic = true;
    }
}
