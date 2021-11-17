using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraNode : ActionNode
{
    public bool turnedOff;
    protected override State OnUpdate()
    {
        Context.Camera.Rotate(turnedOff);
        return Context.Camera.RotatedToTarget ? State.Success : State.Running;
        
    }

    protected override void OnStop()
    {
        base.OnStop();
        if (turnedOff)
        {
            Context.Camera.InvokeDisableEvent();
        }
    }
}
