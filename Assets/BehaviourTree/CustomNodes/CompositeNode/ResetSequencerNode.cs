public class ResetSequencerNode : CompositeNode
{
    private int current;
    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        var child = Children[current];
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                current = 0;
                foreach(var c in Children)
                {
                    if (child.Started)
                    {
                        child.Reset();
                    }
                }
                return State.Failure;
            case State.Success:
                current++;
                break;
        }

        return current == Children.Count ? State.Success : State.Running;
    }
}
