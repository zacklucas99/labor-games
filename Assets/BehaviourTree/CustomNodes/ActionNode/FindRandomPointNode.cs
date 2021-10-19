using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindRandomPointNode : ActionNode
{
    public string message;
    protected override State OnUpdate()
    {
        Context.Officer.FindRandomPoint();
        return State.Success;
    }
}
