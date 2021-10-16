using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWayPointDestinationNode : ActionNode
{

    protected override State OnUpdate()
    {
        Context.Officer.ResetWayPointTarget();
        return State.Success;
    }
}
