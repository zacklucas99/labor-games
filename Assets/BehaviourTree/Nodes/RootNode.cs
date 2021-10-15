using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : Node
{
    public Node Child { get; set; }

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Child)
        {
            return Child.Update();
        }
        return State.Failure;
    }

    public override Node Clone()
    {
        RootNode node = Instantiate(this);
        node.Child = Child.Clone();
        return node;
    }
}
