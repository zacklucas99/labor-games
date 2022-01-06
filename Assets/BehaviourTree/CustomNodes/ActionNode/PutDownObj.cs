using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownObj : ActionNode
{
    public bool cleanUp;
    protected override void OnStart()
    {
        base.OnStart();
        Context.Officer.PutDownObj();
    }
    protected override State OnUpdate()
    {
        return State.Success;
    }
}
