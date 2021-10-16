using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInViewNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.FollowingPlayer)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
