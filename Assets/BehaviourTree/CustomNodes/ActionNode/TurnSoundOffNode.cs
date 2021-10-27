using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSoundOffNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (!Context.Officer.TurnSoundOff())
        {
            return State.Running;
        }
        return State.Success;
    }
}
