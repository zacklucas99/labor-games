using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsApproxNode : ActionNode
{

    protected override State OnUpdate()
    {
        if (Context.Officer.TurnToApproxPoint())
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
