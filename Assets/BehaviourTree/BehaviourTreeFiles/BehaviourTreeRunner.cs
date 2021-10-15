using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;
    // Start is called before the first frame update
    void Start()
    {
        if (tree)
        {
            tree = tree.Clone();
            Debug.Log(GetComponent<OfficerController>());

            Context.Object = gameObject;
            Context.Officer = GetComponent<OfficerController>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        tree.Update();
        
    }
}
