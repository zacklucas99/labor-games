using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNextPointNode : ActionNode
{
    public string message;
    protected override State OnUpdate()
    {
        Debug.Log("Find NextPoint");
        Context.Officer.FindNewPoint();
        return State.Success;
    }
}
