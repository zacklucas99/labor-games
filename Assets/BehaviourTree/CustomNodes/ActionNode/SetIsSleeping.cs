public class SetIsSleeping : ActionNode
{
    public bool sleepingVal = true;
    protected override State OnUpdate()
    {
        Context.Officer.isSleeping = sleepingVal;
        Context.Officer.SetSleeping();
        return State.Success;
    }
}
