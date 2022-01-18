using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSoundNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (!Context.Officer.SoundObj)
        {
            return State.Success;
        }
        if (Context.Officer.isFollowingSound &&!Context.Officer.NearSound() && CalculateDistToSoundObj() > Context.Officer.SoundObj.interactDist)
        {
            if (
            (Context.Officer.CollidesWithOfficersInAbortDistance() && 
            CalculateDistToSoundObj() <= Context.Officer.abortDistanceTheshold)) {
                return State.Success;
            }
            Context.Officer.FollowSound();
            return State.Running;
        }
        return State.Success;
    }

    public float CalculateDistToSoundObj()
    {
        return new Vector2(
            Context.Officer.transform.position.x - Context.Officer.SoundObj.transform.position.x,
            Context.Officer.transform.position.z - Context.Officer.SoundObj.transform.position.z
            ).magnitude;
    }
}
