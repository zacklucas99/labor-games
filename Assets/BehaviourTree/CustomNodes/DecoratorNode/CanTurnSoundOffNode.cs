using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanTurnSoundOffNode :DecoratorNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.SoundObj != null && Context.Officer.CanTurnSoundOff())
        {
            return Child.Update();
        }
        else
        {
            Context.Officer.ResetSoundToHandle();
        }
        return State.Success;
    }
}
