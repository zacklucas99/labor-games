using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Node node;

    public Port input;
    public Port output;

    public Action<NodeView> OnNodeSelected;
    public NodeView(Node node)
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;

        style.left = node.Position.x;
        style.top = node.Position.y;

        CreateInputPorts();
        CreateOutputPorts();
    }

    private void CreateOutputPorts()
    {
        if (node is ActionNode) { }
        else if (node is CompositeNode) {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is DecoratorNode) {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        else if (node is RootNode)
        {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        if (output != null)
        {
            output.portName = "output";
        }
        outputContainer.Add(output);
    }

    private void CreateInputPorts()
    {
        if(node is RootNode)
        {
            return;
        }
        input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        input.portName = "input";
        inputContainer.Add(input);
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        node.Position = new Vector2(newPos.xMin, newPos.yMin);

    }

    public override void OnSelected()
    {
        base.OnSelected();
        if(OnNodeSelected != null)
        {
            OnNodeSelected(this);
        }
    }
}
