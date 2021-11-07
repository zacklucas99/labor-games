using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAlarmNode : ActionNode
{
    protected override State OnUpdate()
    {
        var erg = Context.Officer.RunToAlarm();
        if (!erg)
        {
            return State.Running;
        }
        return State.Success;
    }

    protected override void OnStop()
    {
        Context.Officer.ResetNotification();
    }
}
