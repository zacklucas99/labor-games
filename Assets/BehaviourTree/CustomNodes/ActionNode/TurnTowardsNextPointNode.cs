using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsNextPointNode : ActionNode
{
    public float duration = 1;
    float startTime;
    protected override void OnStart()
    {
        startTime = Time.time;
    }

    protected override State OnUpdate()
    {
        if (Context.Officer.TurnToNextPoint())
        {
            return State.Running;
        }
        return State.Success;
    }
}
