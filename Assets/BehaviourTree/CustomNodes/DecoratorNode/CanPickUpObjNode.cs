using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPickUpObjNode :DecoratorNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.CanTurnSoundOff())
        {
            return Child.Update();
        }
        return State.Success;
    }
}
