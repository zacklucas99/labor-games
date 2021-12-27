using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetInteractSoundNode: ActionNode
{
    public bool storeInMemory;

    protected override State OnUpdate()
    {
        if (storeInMemory)
        {
            Context.Officer.AddToMemory(Context.Officer.SoundObj);
        }
        Context.Officer.ResetSoundInteractionAnimation();
        Context.Officer.ResetSoundToHandle();
        return State.Success;
    }

}
