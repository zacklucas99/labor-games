using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetInteractSoundNode: ActionNode
{

    protected override State OnUpdate()
    {
        Context.Officer.ResetSoundInteractionAnimation();
        Context.Officer.ResetSoundToHandle();
        return State.Success;
    }

}
