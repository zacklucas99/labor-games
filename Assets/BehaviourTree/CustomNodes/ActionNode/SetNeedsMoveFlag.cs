using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNeedsMoveFlag : ActionNode
{
    protected override State OnUpdate()
    {
        Context.Officer.SetNeedsMoveFlag();
        return State.Success;
    }
}
