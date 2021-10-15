using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { };

    Editor editor;
    public InspectorView(){
        focusable = false;
    }

    internal void UpdateSelection(NodeView node)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);

        editor = Editor.CreateEditor(node.node);
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);

    }
}
