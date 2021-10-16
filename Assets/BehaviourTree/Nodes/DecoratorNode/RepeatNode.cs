using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
{
    public bool loopInfinite = true;
    public int maxCounter;
    private int counter;
    protected override void OnStart()
    {
        counter = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (loopInfinite)
        {
            Child.Update();
            return State.Running;
        }
        else
        {
            if(counter < maxCounter)
            {
                counter++;
                Child.Update();
                return State.Running;
            }
            return State.Success;
        }
    }
}
