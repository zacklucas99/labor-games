using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotNewSoundNode : ActionNode
{
    protected override State OnUpdate()
    {

        if (Context.Officer.GotNewSound)
        {
            return State.Success;
        }
        return State.Failure;
    }

    protected override void OnStop()
    {
        base.OnStop();
        Context.Officer.GotNewSound = false;
    }
}
