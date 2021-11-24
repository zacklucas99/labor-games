using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowExclamationMarkNode : ActionNode
{
    protected override State OnUpdate()
    {
        Context.Officer.exclamationMark.GetComponent<MeshRenderer>().enabled = true;
        return State.Success;
    }
}
