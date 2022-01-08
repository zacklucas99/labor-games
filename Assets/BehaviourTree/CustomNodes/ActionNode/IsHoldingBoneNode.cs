public class IsHoldingBoneNode : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.IsHoldingBone)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
