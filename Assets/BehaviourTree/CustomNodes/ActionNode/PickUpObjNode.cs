using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObjNode : ActionNode
{
    public bool cleanUp;
    protected override void OnStart()
    {
        base.OnStart();
        if (!cleanUp)
        {
            Context.Officer.PickUpObj();
        }
        else
        {
            Context.Officer.CleanUpObj();
        }
    }
    protected override State OnUpdate()
    {
        if (Context.Officer.IsPickingUp)
        {
            return State.Running;
        }
        return State.Success;
    }
}
