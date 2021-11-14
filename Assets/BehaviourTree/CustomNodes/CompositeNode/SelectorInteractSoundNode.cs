using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorInteractSoundNode: CompositeNode
{
    private bool pickingUp;
    private bool turningOff;
    protected override State OnUpdate()
    {
        if (Context.Officer.CanPickUpObj())
        {
            pickingUp = true;
            return Children[0].Update();
        }

        else if (Context.Officer.CanTurnSoundOff())
        {
            turningOff = true;
            return Children[1].Update();
        }

        else if (pickingUp)
        {
            return Children[0].Update();
        }

        else if (turningOff)
        {
            return Children[1].Update();
        }

        //Finished
        return State.Success;
    }

    protected override void OnStop()
    {
        base.OnStop();
        pickingUp = false;
        turningOff = false;
    }

    public override void Reset()
    {
        base.Reset();
        pickingUp = false;
        turningOff = false;
    }

}
