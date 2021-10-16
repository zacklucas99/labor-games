using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode : ActionNode
{
    public string message;
    protected override State OnUpdate()
    {
        if (Context.Officer.Move())
        {
            Debug.Log("Move");
            return State.Running;
        }
        return State.Success;
    }
}
