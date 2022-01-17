public class IsNotifierObjBone : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.NotifierObject && Context.Officer.NotifierObject.isBone)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
