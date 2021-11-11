using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBetweenSoundNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.WallBetweenSound())
        {
            return State.Success;
        }
        return State.Failure;
    }
}
