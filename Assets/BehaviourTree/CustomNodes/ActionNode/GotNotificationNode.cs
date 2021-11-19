using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotNotificationNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.GotEnvironmentNotification)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
