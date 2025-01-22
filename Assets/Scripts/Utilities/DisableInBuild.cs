using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInBuild : MonoBehaviour
{
    private void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            gameObject.SetActive(false);
        }
    }
}
