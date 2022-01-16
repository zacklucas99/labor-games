using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public BoxCollider collider;
    public float timeDelta = 0.5f;
    void Start()
    {
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if((timeDelta -= Time.deltaTime) <= 0f)
        {
            collider.enabled = true;
        }
        
    }
}
