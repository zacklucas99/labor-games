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
            Context.Object = gameObject;
            Context.Officer = GetComponent<OfficerController>();
            tree = tree.Clone();
        }

    }

    // Update is called once per frame
    void Update()
    {
        tree.Update();
        
    }
}
