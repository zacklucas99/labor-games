using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BehaviourTreeView : GraphView
{

    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits> { };
    private BehaviourTree tree;
    public BehaviourTreeView() {
        
        Insert(0, new GridBackground());
        
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourTree/UI/BehaviourTreeEditor.uss");

        styleSheets.Add(styleSheet);

    }

    protected override void CollectCopyableGraphElements(IEnumerable<GraphElement> elements, HashSet<GraphElement> elementsToCopySet)
    {
        Debug.Log("CollectCopyableGraphElements");
        base.CollectCopyableGraphElements(elements, elementsToCopySet);
        foreach(var element in elements)
        {
            elementsToCopySet.Add(element);
        }
    }

    NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }


    internal void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements.ToList());
        graphViewChanged += OnGraphViewChanged;
        if (tree.RootNode == null)
        {
            tree.RootNode = tree.CreateNode(typeof(RootNode));
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }



        foreach (var node in tree.Nodes)
        {
            if(node != null)
            {
                CreateNodeView(node);
            }
        }

        foreach(Node n in tree.Nodes){
            foreach (var child in tree.GetChildren(n))
            {
                NodeView nodeView = FindNodeView(n);
                NodeView childView = FindNodeView(child);

                Edge edge = nodeView.output.ConnectTo(childView.input);
                AddElement(edge);
            }
        }
    }
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if(graphViewChange.elementsToRemove != null)
        {
            foreach(var elem in graphViewChange.elementsToRemove)
            {
                NodeView nodeView = elem as NodeView;
                if(nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;

                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    tree.RemoveChild(parentView.node, childView.node);
                }
                   
            }
        }

        if(graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                tree.AddChild(parentView.node, childView.node);
            });
        }
        return graphViewChange;
    }

    private void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        List<Type> nodes = new List<Type>();
        
        foreach (Type n in TypeCache.GetTypesDerivedFrom<ActionNode>()){
            nodes.Add(n);
        }

        foreach (Type n in TypeCache.GetTypesDerivedFrom<CompositeNode>())
        {
            nodes.Add(n);
        }

        foreach (Type n in TypeCache.GetTypesDerivedFrom<DecoratorNode>())
        {
            nodes.Add(n);
        }

        foreach (var node in nodes)
        {
            evt.menu.AppendAction($"[{node.BaseType.Name}] {node.Name}", (a) => { CreateNode(a, node); });
        }
        evt.menu.AppendAction("Delete Selection", (a) => { DeleteSelection(); });


    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }

    void CreateNode(DropdownMenuAction a, Type type)
    {
        Node node = tree.CreateNode(type);
        node.Position = a.eventInfo.localMousePosition;
        CreateNodeView(node);
    }
}
