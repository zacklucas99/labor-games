using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsSoundNode : ActionNode
{
    protected override void OnStart()
    {
        base.OnStart();
        Context.Officer.OverTurning = false;
    }
    protected override State OnUpdate()
    {
        if (Context.Officer.TurnToSound())
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
