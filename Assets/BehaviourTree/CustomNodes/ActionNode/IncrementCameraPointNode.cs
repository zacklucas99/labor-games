using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementCameraPointNode : ActionNode
{

    protected override State OnUpdate()
    {
        Context.Camera.IncrementIndex();
        return State.Success;
    }
}
