using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverterNode :DecoratorNode
{
    protected override State OnUpdate()
    {
        var state = Child.Update();
        if (state == Node.State.Success) {
            return State.Failure;
        }
        if (state == Node.State.Failure)
        {
            return State.Success;
        }

        return state;
    }
}
