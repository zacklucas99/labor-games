using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowExclamationMarkNode : ActionNode
{
    public SearchStateNode searchState;

    public bool hide = false;
    protected override State OnUpdate()
    {
        if (searchState == SearchStateNode.ExclamationMarkNode)
        {
            Context.Officer.questionMark.GetComponent<MeshRenderer>().enabled = false;
            Context.Officer.exclamationMark.GetComponent<MeshRenderer>().enabled = !hide;
        }

        if (searchState == SearchStateNode.QuestionMarkNode && Context.Officer.questionMark)
        {
            Context.Officer.exclamationMark.GetComponent<MeshRenderer>().enabled = false;
            Context.Officer.questionMark.GetComponent<MeshRenderer>().enabled = !hide;
        }
        return State.Success;
    }
}

public enum SearchStateNode
{
    QuestionMarkNode,
    ExclamationMarkNode
}
