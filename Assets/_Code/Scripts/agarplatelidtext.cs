using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agarplatelidtext : MonoBehaviour
{

    public GameObject lidSelectedObject;

    private bool areLidsCombined;

    public void objectEnabled()
    {
        if (areLidsCombined)
        {
            lidSelectedObject.SetActive(true);
        }

    }

    public void objectDisabled()
    {
        lidSelectedObject.SetActive(false);
    }

    public void lidsCombined()
    {
        areLidsCombined = true;
    }

    public void lidsApart()
    {
        areLidsCombined = false;
    }
}
