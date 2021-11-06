using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerNodeCamera : ActionNode
{
    protected override State OnUpdate()
    {

        if (Context.Camera.FoundPlayer)
        {
            Context.Camera.FollowPlayer();
            return State.Running;
        }
        return State.Success;
    }
}
