using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInViewNodeCamera : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Camera.FoundPlayer)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
