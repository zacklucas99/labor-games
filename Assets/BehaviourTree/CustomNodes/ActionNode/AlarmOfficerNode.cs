using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmOfficerNode : ActionNode
{
    public string message;
    protected override State OnUpdate()
    {
        Context.Officer.Alarm();
        return State.Success;
    }
}
