using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorInteractSoundNode: CompositeNode
{
    private bool pickingUp;
    private bool turningOff;
    private bool cleaninUp;
    protected override State OnUpdate()
    {
        if(Context.Officer.SoundObj && !Context.Officer.SoundObj.turnedOn)
        {
            // Abort action, if sound object already turned off
            return State.Success;
        }

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
        else if (Context.Officer.CanCleanUpObj())
        {
            cleaninUp = true;
            return Children[2].Update();
        }

        else if (pickingUp)
        {
            return Children[0].Update();
        }

        else if (turningOff)
        {
            return Children[1].Update();
        }

        else if (cleaninUp)
        {
            return Children[2].Update();
        }

        //Finished
        return State.Success;
    }

    protected override void OnStop()
    {
        base.OnStop();
        pickingUp = false;
        turningOff = false;
        cleaninUp = false;
    }

    public override void Reset()
    {
        base.Reset();
        pickingUp = false;
        turningOff = false;
        cleaninUp = false;
    }

}
