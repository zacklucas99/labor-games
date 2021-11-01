using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloseByNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.PlayerCloseBy)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
