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

    [HideInInspector] public bool stopDebugging = false;

    [HideInInspector]  public State NodeState = State.Running;
    private bool started = false;

    public Context Context { get; set; }

    public bool Started => started;

    public string description;

    [HideInInspector]public string guid;

    [HideInInspector] public Vector2 Position;

    public State Update()
    {
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
    public virtual void Reset()
    {
        started = false;
    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }
}
