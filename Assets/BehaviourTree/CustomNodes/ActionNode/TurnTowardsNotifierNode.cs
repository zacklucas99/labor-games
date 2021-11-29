using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsNotifierNode : ActionNode
{
    protected override void OnStart()
    {
        base.OnStart();
    }
    protected override State OnUpdate()
    {
        if (Context.Officer.TurnToNotifier())
        {
            return State.Running;
        }
        return State.Success;
    }

    protected override void OnStop()
    {
        base.OnStop();
        Context.Officer.ResetTurn();
    }
}
