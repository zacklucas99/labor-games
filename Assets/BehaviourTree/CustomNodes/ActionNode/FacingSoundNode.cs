using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingSoundNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.FacingSoundObj())
        {
            // Check whether player is looking in the directory of the sound
            return State.Success;
        }
        return State.Failure;
    }
}
