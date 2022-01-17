using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingNotificationNode : ActionNode
{
    private Vector3 lastPos;
    protected override State OnUpdate()
    {
        if (Context.Officer.GotEnvironmentNotification && Context.Officer.FollowNotification())
        {
            if (Context.Officer.transform.position == lastPos)
            {
                return State.Success;
            }
            lastPos = Context.Officer.transform.position;
            return State.Running;
        }
        return State.Success;
    }
}
