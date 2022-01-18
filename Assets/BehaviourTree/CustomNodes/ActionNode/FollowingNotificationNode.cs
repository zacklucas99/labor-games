using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingNotificationNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.GotEnvironmentNotification && Context.Officer.FollowNotification())
        {
            return State.Running;
        }
        return State.Success;
    }
}
