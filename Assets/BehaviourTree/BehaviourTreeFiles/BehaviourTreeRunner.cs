using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;
    // Start is called before the first frame update
    void Start()
    {
        Context context = new Context();
        if (tree)
        {
            context.Object = gameObject;
            context.Officer = GetComponent<OfficerController>();
            tree = tree.Clone();

            foreach(var node in tree.Nodes)
            {
                node.Context = context;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        tree.Update();
        
    }
}
