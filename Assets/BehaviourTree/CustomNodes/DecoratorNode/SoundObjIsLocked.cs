using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObjIsLocked : DecoratorNode
{
    protected override State OnUpdate()
    {
        if(Context.Officer.SoundObj && Context.Officer.SoundObj.isLocked)
        {
            return State.Running;
        }
        return State.Success;
    }
}
