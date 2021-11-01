using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : Node
{
    public List<Node> Children= new List<Node>();

    public override Node Clone()
    {
       CompositeNode node = Instantiate(this);
        node.Children = new List<Node>();
        foreach(var child in Children)
        {
            node.Children.Add(child.Clone());
        }
        return node;
    }

    public override void Reset()
    {
        if (Started)
        {
            base.Reset();
            foreach (var child in Children)
            {
                child.Reset();
            }
        }
    }
}
