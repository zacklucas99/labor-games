using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopNode : ActionNode
{
    public float duration = 1;
    float startTime;
    public bool failOnFollowingPlayer = true;
    public bool failOnHeardSound = true;
    protected override void OnStart()
    {
        startTime = Time.time;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(Context.Officer.isFollowingSound && failOnHeardSound)
        {
            return State.Failure;
        }


        if (Context.Officer.FollowingPlayer && failOnFollowingPlayer)
        {
            return State.Failure;
        }


        Context.Officer.StopMovement();
        
        if (Time.time - startTime > duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
