using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorInteractNotificationNode: CompositeNode
{
    private bool pickingUp;
    private bool cleaningUp;
    protected override State OnUpdate()
    {
        if (Context.Officer.CanPickUpObj())
        {
            pickingUp = true;
            return Children[0].Update();
        }

        else if (pickingUp)
        {
            return Children[0].Update();
        }
        else if (Context.Officer.CanCleanUpObj())
        {
            cleaningUp = true;
            return Children[2].Update();
        }
        else if (cleaningUp)
        {
            return Children[2].Update();
        }

        else
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
        cleaningUp = false;
    }

    public override void Reset()
    {
        base.Reset();
        pickingUp = false;
        cleaningUp = false;
    }

}
