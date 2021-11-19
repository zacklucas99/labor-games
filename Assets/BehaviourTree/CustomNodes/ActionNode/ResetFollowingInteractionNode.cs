using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetFollowingInteractionNode: ActionNode
{

    protected override State OnUpdate()
    {
        Context.Officer.ResetEnvironmentNotification();
        return State.Success;
    }

}
