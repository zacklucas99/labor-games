public class IsNotifiedNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.Notified)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
