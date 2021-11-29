using System;
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
        }
    }
    private bool isStandingAtBack;

    public bool IsStandingAtBack
    {
        get { return isStandingAtBack; }
        set
        {
            isStandingAtBack = value;
        }
    }
    public Animator animator;

    private bool isOpen;
    private bool openedFront;
    private bool openedBack;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Open()
    {
        if (!isOpen) {
            animator.SetBool("opening_front", isStandingAtFront);
            animator.SetBool("opening_back", isStandingAtBack);
            openedFront = isStandingAtFront;
            openedBack = isStandingAtBack;
            isOpen = isStandingAtBack || isStandingAtFront;
        }

        else
        {
            Debug.Log(openedFront);
            animator.SetBool("closing_front", openedFront);
            animator.SetBool("closing_back", openedBack);
        }
    }

    internal void Reset()
    {
        openedFront = false;
        isOpen = false;
        openedBack = false;
        animator.SetBool("opening_front", false);
        animator.SetBool("opening_back", false);
        animator.SetBool("closing_front", false);
        animator.SetBool("closing_back", false);
    }

}
