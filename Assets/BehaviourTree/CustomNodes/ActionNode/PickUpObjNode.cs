using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObjNode : ActionNode
{
    protected override void OnStart()
    {
        base.OnStart();
        Context.Officer.PickUpObj();
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
