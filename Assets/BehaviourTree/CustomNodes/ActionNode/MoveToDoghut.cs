using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToDoghut : ActionNode
{
    protected override State OnUpdate()
    {
        var erg = Context.Officer.MoveToDogHut();
        if (!erg)
        {
            return State.Running;
        }
        else
        {
            return State.Success;
        }
    }
}
