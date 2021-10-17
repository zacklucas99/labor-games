using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Node node;

    public Port input;
    public Port output;

    public Action<NodeView> OnNodeSelected;
    public NodeView(Node node):base("Assets/BehaviourTree/UI/NodeView.uxml")
    {
        this.node = node;
        this.title = node.name.Replace("(Clone)","");
        this.viewDataKey = node.guid;

        style.left = node.Position.x;
        style.top = node.Position.y;

        CreateInputPorts();
        CreateOutputPorts();

        SetupClasses();
    }

    private void SetupClasses()
    {
        if (node is ActionNode) {
            this.AddToClassList("action");
                }
        else if (node is CompositeNode)
        {

            this.AddToClassList("composite");
        }
        else if (node is DecoratorNode)
        {

            this.AddToClassList("decorator");
        }

        else if (node is RootNode)
        {

            this.AddToClassList("root");
        }
    }

    private void CreateOutputPorts()
    {
        if (node is ActionNode) { }
        else if (node is CompositeNode) {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is DecoratorNode) {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        else if (node is RootNode)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        if (output != null)
        {
            output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse;
        }

        outputContainer.Add(output);
    }

    private void CreateInputPorts()
    {
        if(node is RootNode)
        {
            return;
        }
        input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        input.portName = "";
        input.style.flexDirection = FlexDirection.Column;
        inputContainer.Add(input);
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Undo.RecordObject(node, "BehaviourTree (Set Position)");
        node.Position = new Vector2(newPos.xMin, newPos.yMin);
        EditorUtility.SetDirty(node);
    }

    public void SortChildren()
    {
        if(node is CompositeNode compositeNode)
        {
            compositeNode.Children.Sort(SortByHorizontalPosition);
        }
    }

    private int SortByHorizontalPosition(Node left, Node right)
    {
        return (int)(left.Position.x - right.Position.x);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if(OnNodeSelected != null)
        {
            OnNodeSelected(this);
        }
    }

    public void UpdateState()
    {
        RemoveFromClassList("running");
        RemoveFromClassList("failure");
        RemoveFromClassList("success");
        if (Application.isPlaying)
        {
                switch (node.NodeState)
                {
                    case Node.State.Running:
                        if (node.Started)
                        {
                            AddToClassList("running");
                        }
                        break;
                    case Node.State.Failure:
                        AddToClassList("failure");
                        break;
                    case Node.State.Success:
                        AddToClassList("success");
                        break;
                }
        }
    }
}
