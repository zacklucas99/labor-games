using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSoundNode : ActionNode
{
    protected override State OnUpdate()
    {

        if (Context.Officer.isFollowingSound &&!Context.Officer.NearSound())
        {
            Context.Officer.FollowSound();
            return State.Running;
        }
        return State.Success;
    }
}
