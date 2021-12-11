using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjStillExitsNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.SoundObj != null && Context.Officer.SoundObj.turnedOn)
        {
            return State.Success;
        }
        return State.Failure;
    }

    public override void Reset()
    {
        base.Reset();
        Context.Officer.isFollowingSound = false;
    }
}
