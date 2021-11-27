using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private bool isStandingAtFront;
    public bool IsStandingAtFront { 
        get { return isStandingAtFront; }
        set {
            isStandingAtFront = value;
            animator.SetBool("opening_front", value);
        }
    }
    private bool isStandingAtBack;
    public bool IsStandingAtBack
    {
        get { return isStandingAtBack; }
        set
        {
            isStandingAtBack = value;
            animator.SetBool("opening_back", value);
        }
    }
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
}
