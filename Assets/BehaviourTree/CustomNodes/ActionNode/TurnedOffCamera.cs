using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnedOffCamera : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Camera.TurnedOff)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
