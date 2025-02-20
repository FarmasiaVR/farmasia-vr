using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseBox : MonoBehaviour
{

    private Animator animator;
    private bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OpenBox()
    {
        if (animator == null) 
        {
            Debug.LogWarning("Animator component wasn't assigned to the object!");
            return;
        }
        animator.SetTrigger("TriggerOpen");
        isOpen = true;
    }

    private void CloseBox()
    {
        if (animator == null) return;
        animator.SetTrigger("TriggerClose");
        isOpen = false;
    }

    public void OpenOrClose()
    {
        if (!isOpen)
        {
            OpenBox();
        }
        else CloseBox();
    }
}
