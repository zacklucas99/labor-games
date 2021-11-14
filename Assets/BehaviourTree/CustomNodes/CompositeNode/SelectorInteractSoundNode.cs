using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorInteractSoundNode: CompositeNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.CanPickUpObj())
        {
            return Children[0].Update();
        }

        else if (Context.Officer.CanTurnSoundOff())
        {
            return Children[1].Update();
        }

        return State.Failure;
    }

}
