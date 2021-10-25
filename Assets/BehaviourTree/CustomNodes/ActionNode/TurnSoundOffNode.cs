using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSoundOffNode : ActionNode
{

    protected override State OnUpdate()
    {
        Context.Officer.TurnSoundOff();
        return State.Success;
    }
}
