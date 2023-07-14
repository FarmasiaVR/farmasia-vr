using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class playerSelectedFilter : MonoBehaviour, IXRSelectFilter
{

    public bool preventSelect;
    public bool canProcess => true;

    //This Process is for when interactor is selecting an interactable
    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        if (preventSelect)
        {
            List<IXRSelectInteractor> selectors = interactable.interactorsSelecting;

            bool playerIsSelecting = playerIsOneOfSelectors(selectors);

            return playerIsSelecting;
        }
        else
        {
            return true;
        }
    }

    bool playerIsOneOfSelectors(List<IXRSelectInteractor> selectors)
    {
        bool playerIsSelecting = false;

        selectors.ForEach(selector =>
        {
            XRController controller = selector.transform.GetComponent<XRController>();
            if (controller)
            {
                playerIsSelecting = true;
            }
        });

        return playerIsSelecting;
    }
}
