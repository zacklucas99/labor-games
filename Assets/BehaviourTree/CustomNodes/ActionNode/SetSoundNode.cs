using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSoundNode : ActionNode
{
    public SoundState soundState;

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override State OnUpdate()
    {
        Context.Officer.gameObject.GetComponent<SoundObject>().turnedOn = soundState == SoundState.Start;
        return State.Success;
    }

    protected override void OnStop()
    {
        base.OnStop();
    }
}

public enum SoundState
{
    Start,
    Stop
}
