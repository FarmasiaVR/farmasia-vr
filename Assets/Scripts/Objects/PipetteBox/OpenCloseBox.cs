using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseBox : MonoBehaviour
{

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenBox()
    {
        if (animator == null) 
        {
            Debug.LogWarning("Animator component wasn't assigned to the object!");
            return;
        }
        animator.SetTrigger("TriggerOpen");
    }

    public void CloseBox()
    {
        if (animator == null) return;
        animator.SetTrigger("TriggerClose");
    }
}
