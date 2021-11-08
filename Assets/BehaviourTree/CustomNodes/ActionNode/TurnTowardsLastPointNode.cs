using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsLastPointNode : ActionNode
{

    protected override State OnUpdate()
    {
        if (Context.Officer.NeedsMoveFlag)
        {
            return State.Failure;
        }
        if (Context.Officer.TurnToLastPoint())
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
