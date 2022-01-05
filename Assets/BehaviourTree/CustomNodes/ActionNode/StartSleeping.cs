using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSleeping : ActionNode
{
    public bool cleanUp;
    protected override void OnStart()
    {
        base.OnStart();
        Context.Officer.StartSleeping();
    }
    protected override State OnUpdate()
    {
        return State.Success;
    }
}
