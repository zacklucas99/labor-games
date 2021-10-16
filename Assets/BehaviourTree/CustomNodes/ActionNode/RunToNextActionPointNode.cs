using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToNextActionPointNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.FollowingPlayer)
        {
            return State.Failure;
        }
        if (Context.Officer.RunToLastActionPoint())
        {
            return State.Running;
        }
        return State.Success;
    }
}
