using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteAllNode: CompositeNode
{

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        for (int i = 0; i < Children.Count; ++i)
        {
            var child = Children[i];

            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return State.Success;
                case State.Failure:
                    continue;
            }
        }

        return State.Failure;
    }

}
