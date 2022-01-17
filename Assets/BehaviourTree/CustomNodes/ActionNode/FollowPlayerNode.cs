using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerNode : ActionNode
{
    private Vector3 lastPos;
    protected override void OnStart()
    {
        base.OnStart();
        lastPos = Vector3.zero;
    }
    protected override State OnUpdate()
    {

        if (Context.Officer.FollowingPlayer)
        {
            Context.Officer.FollowPlayer();
            if(Context.Officer.transform.position == lastPos)
            {
                return State.Success;
            }
            lastPos = Context.Officer.transform.position;
            return State.Running;
        }
        return State.Success;
    }

    protected override void OnStop()
    {
        base.OnStop();
        Context.Officer.LostPlayer();
    }
}
