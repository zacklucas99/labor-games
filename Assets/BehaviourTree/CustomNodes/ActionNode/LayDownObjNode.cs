using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayDownObjNode : ActionNode
{
    public bool cleanUp;
    protected override void OnStart()
    {
        base.OnStart();
        Context.Officer.PutDownObj();
    }
    protected override State OnUpdate()
    {
        if (!Context.Officer.isLayingDown)
        {
            return State.Success;
        }
        return State.Running;
    }
}
