using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class playerSelectedFilter : MonoBehaviour, IXRSelectFilter
{
    public bool canProcess => true;

    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        List<IXRSelectInteractor> selectors = interactable.interactorsSelecting;

        bool playerIsSelecting = playerIsOneOfSelectors(selectors);

        return playerIsSelecting;
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
