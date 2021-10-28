using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingSoundNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.isFollowingSound)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
