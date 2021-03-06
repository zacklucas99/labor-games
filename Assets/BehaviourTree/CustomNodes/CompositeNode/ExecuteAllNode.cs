using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteAllNode: CompositeNode
{
    public bool canFail;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var state = Node.State.Success;
        for (int i = 0; i < Children.Count; ++i)
        {
            var child = Children[i];

            switch (child.Update())
            {
                case State.Running:
                    // Resetting all the nodes, if they arent't currently run
                    for(int j = i+1; j< Children.Count; j++)
                    {
                        if (Children[j].Started)
                        {
                            Children[j].Reset();
                        }
                    }
                    return State.Running;
                case State.Success:
                    continue;
                case State.Failure:
                    if (canFail)
                    {
                        return State.Failure;
                    }
                    continue;
            }
        }

        return state;
    }

}
