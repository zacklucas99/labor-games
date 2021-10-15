using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : Node
{
    public List<Node> Children= new List<Node>();

    public override Node Clone()
    {
       CompositeNode node = Instantiate(this);
        foreach(var child in Children)
        {
            node.Children.Add(child.Clone());
        }
        return node;
    }
}
