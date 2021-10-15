using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public Node RootNode { get; set; }
    public Node.State TreeState { get; set; }

    public List<Node> Nodes { get; set; } = new List<Node>();

    public Node.State Update()
    {
        if (RootNode.NodeState == Node.State.Running)
        {
            return RootNode.Update();
        }
        return TreeState;
    }

    public Node CreateNode(System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        Nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(Node node)
    {
        Nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }


    public void AddChild(Node parent, Node child)
    {
        DecoratorNode decoratorNode = parent as DecoratorNode;
        if (decoratorNode)
        {
            decoratorNode.Child = child;
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode) {
            rootNode.Child = child;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.Children.Add(child);
        }
    }

    public void RemoveChild(Node parent, Node child)
    {
        DecoratorNode decoratorNode = parent as DecoratorNode;
        if (decoratorNode)
        {
            decoratorNode.Child = null;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.Children.Remove(child);
        }

        RootNode root = parent as RootNode;
        if (root)
        {
            root.Child = null;
        }
    }

    public List<Node> GetChildren(Node parent)
    {

        DecoratorNode decorator = parent as DecoratorNode;
        if(decorator && decorator.Child != null)
        {
            return new List<Node> { decorator.Child };
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode && rootNode.Child != null)
        {
            return new List<Node> { rootNode.Child };
        }


        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            return composite.Children;
        }

        return new List<Node>();
    }

    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.RootNode = RootNode.Clone();
        return tree;
    }
    /*
    public void SetContext(Context c)
    {
        foreach (Node n in Nodes)
        {
            Debug.Log(n);
            n.Context = c;
        }
    }
    */
}
