public class IsSleepingNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.isSleeping)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
