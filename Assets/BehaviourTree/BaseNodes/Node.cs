using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running, Failure, Success
    }

    public bool stopDebugging = false;

    public State NodeState { get; set; } = State.Running;
    private bool started = false;

    //public Context Context { get; set; }

    [HideInInspector]public string guid;

    public Vector2 Position { get; set; }

    public State Update()
    {
        Debugger.Break();
        if (!started)
        {
            OnStart();
            started = true;
        }

        NodeState = OnUpdate();

        if(NodeState != State.Running)
        {
            OnStop();
            started = false;
        }
        return NodeState;
    }

    protected virtual void OnStart() { }
    protected virtual void OnStop() { }
    protected virtual State OnUpdate() { return State.Success; }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }
}
