public class IsSoundObjBone : ActionNode
{
    protected override State OnUpdate()
    {
        if (Context.Officer.SoundObj && Context.Officer.SoundObj.isBone)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
