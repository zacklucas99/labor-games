using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTurnNode: ActionNode
{
    public float duration = 1;
    float startTime;
    protected override void OnStart()
    {
        startTime = Time.time;
    }

    protected override State OnUpdate()
    {
        Context.Officer.ResetTurn();
        return State.Success;
    }
}
