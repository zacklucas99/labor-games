using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAlarmNode: ActionNode
{
    protected override State OnUpdate()
    {
        Context.Officer.ResetNotification(); 
        return State.Success;
    }
}
