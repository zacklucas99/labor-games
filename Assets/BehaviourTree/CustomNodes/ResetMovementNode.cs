using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMovementNode : ActionNode
{

    protected override State OnUpdate()
    {
        Context.Officer.Reset();
        return State.Success;
    }
}
