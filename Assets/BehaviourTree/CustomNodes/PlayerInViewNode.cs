using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInViewNode : DecoratorNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.FollowingPlayer)
        {
            //return Child.Update();
        }
        return State.Failure;
    }
}
