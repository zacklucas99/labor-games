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
        Undo.RecordObject(this, "Behaviour Tree (Add Child)");

        if (!Application.isPlaying)
        {
            AssetDatabase.AddObjectToAsset(node, this);
        }
        Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (Add Child)");
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(Node node)
    {
        Undo.RecordObject(this, "Behaviour Tree (Delece Child)");
        Nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }


    public void AddChild(Node parent, Node child)
    {
        DecoratorNode decoratorNode = parent as DecoratorNode;
        if (decoratorNode)
        {
            Undo.RecordObject(decoratorNode, "Behaviour Tree (Add Child)");
            decoratorNode.Child = child;
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode) {
            Undo.RecordObject(rootNode, "Behaviour Tree (Add Child)");
            rootNode.Child = child;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            Undo.RecordObject(composite, "Behaviour Tree (Add Child)");
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
            Undo.RecordObject(decoratorNode, "Behaviour Tree (Remove Child)");
            decoratorNode.Child = null;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            Undo.RecordObject(composite, "Behaviour Tree (Remove Child)");
            composite.Children.Remove(child);
        }

        RootNode root = parent as RootNode;
        if (root)
        {
            Undo.RecordObject(root, "Behaviour Tree (Remove Child)");
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

    public void Traverse(Node node, ref List<Node>nodes)
    {
        if (node)
        {
            nodes.Add(node);
            if(node is RootNode root)
            {
                Traverse(root.Child, ref nodes);
            }
            else if(node is DecoratorNode decorator)
            {
                Traverse(decorator.Child, ref nodes);
            }
            else if(node is CompositeNode composite)
            {
                foreach (var child in composite.Children)
                {
                    Traverse(child, ref nodes);
                }
            }
        }
    }

    public BehaviourTree Clone()
    {
        Debug.Log("Clone");
        BehaviourTree tree = Instantiate(this);
        tree.RootNode = RootNode.Clone();
        tree.Nodes = new List<Node>();
        Traverse(tree.RootNode, ref tree.Nodes);
        return tree;
    }
}
