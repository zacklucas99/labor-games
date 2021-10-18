using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsLastPointNode : ActionNode
{
    public float duration = 1;
    float startTime;
    protected override void OnStart()
    {
        startTime = Time.time;
    }

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
}
