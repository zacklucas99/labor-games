using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreFailNode :DecoratorNode
{
    protected override State OnUpdate()
    {
        State state = Child.Update();
        if(state == State.Failure)
        {
            return State.Success;
        }
        return state;
    }
}
