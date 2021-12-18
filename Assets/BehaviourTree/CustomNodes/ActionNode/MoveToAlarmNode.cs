using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAlarmNode : ActionNode
{
    protected override void OnStart()
    {
        base.OnStart();
        Context.Officer.SetNotification();
    }
    protected override State OnUpdate()
    {
        var erg = Context.Officer.RunToAlarm();
        if (erg)
        {
            return State.Running;
        }
        return State.Success;
    }

    protected override void OnStop()
    {
        Context.Officer.ResetNotification();
    }

    public override void Reset()
    {
        base.Reset();
        Context.Officer.ResetNotification();
    }
}
