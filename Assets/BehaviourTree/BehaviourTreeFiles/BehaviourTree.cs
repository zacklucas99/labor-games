using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviourTree")]
public class BehaviourTree : ScriptableObject
{
    public Node RootNode;
    public Node.State TreeState;

    public List<Node> Nodes= new List<Node>();


    public void Awake()
    {
        // Cleaning up database from unnecessary assets
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath("Assets/OfficerTree.asset"))
        {
            Debug.Log("Asset:" + asset);
            if (asset.GetType() == typeof(ToDeleteNode))
            {
                DestroyImmediate(asset, true);
            }
        }
        AssetDatabase.SaveAssets();
    }

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

        EditorUtility.SetDirty(parent);
        AssetDatabase.SaveAssetIfDirty(this);
        AssetDatabase.SaveAssets();
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

        EditorUtility.SetDirty(parent);
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
}
