using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfPlayerInView : CompositeNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.FollowingPlayer)
        {
            if (Children.Count > 0)
            {
                return Children[0].Update();
            }
        }
        else
        {
            if(Children.Count > 1)
            {
                return Children[1].Update();
            }
        }
        return State.Failure;
    }
}
