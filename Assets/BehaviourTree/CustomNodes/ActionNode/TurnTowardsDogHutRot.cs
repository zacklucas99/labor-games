using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsDogHutRots : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.NeedsMoveFlag)
        {
            return State.Failure;
        }

        if (Context.Officer.TurnTowardsDogHutPoint())
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
