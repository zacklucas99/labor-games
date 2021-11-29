using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorInteractNotificationNode: CompositeNode
{
    private bool pickingUp;
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
    }

    public override void Reset()
    {
        base.Reset();
        pickingUp = false;
    }

}
