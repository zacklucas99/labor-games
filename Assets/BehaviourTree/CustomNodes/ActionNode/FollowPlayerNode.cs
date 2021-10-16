using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerNode : ActionNode
{
    protected override State OnUpdate()
    {

        if (Context.Officer.FollowingPlayer)
        {
            Context.Officer.FollowPlayer();
            return State.Running;
        }
        return State.Success;
    }
}
