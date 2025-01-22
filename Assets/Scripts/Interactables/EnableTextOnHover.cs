using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTextOnHover : MonoBehaviour
{
    public GameObject textObject;

    public void enableText()
    {
        textObject.SetActive(true);
    }

    public void disableText()
    {
        textObject.SetActive(false);
    }
}
