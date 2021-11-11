using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraNode : ActionNode
{
    protected override State OnUpdate()
    {
        Context.Camera.Rotate();
        return Context.Camera.RotatedToTarget ? State.Success : State.Running;
    }
}
