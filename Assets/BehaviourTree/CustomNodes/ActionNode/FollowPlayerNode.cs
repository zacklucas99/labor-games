using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerNode : ActionNode
{
    protected override State OnUpdate()
    {

        Debug.Log("Following Player:"+Context.Officer.FollowingPlayer);
        if (Context.Officer.FollowingPlayer)
        {
            Context.Officer.FollowPlayer();
            return State.Running;
        }
        return State.Success;
    }
}
