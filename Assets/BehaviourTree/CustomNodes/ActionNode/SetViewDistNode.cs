using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetViewDistNode : ActionNode
{
    public float viewDist;
    protected override State OnUpdate()
    {
        Context.Officer.SetViewDist(viewDist);
        return State.Success;
    }
}
