using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayDownObjNode : ActionNode
{
    public bool cleanUp;
    protected override void OnStart()
    {
        base.OnStart();
    }
    protected override State OnUpdate()
    {
        return State.Success;
    }
}
