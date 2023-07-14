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

    //time that is allowed to pass since the last successfull check
    public float allowCycleTime;

    Dictionary<IXRSelectInteractor, float> allowedInteractors = new Dictionary<IXRSelectInteractor, float>();
    //This Process is for when interactor is selecting an interactable
    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        Debug.Log(allowedInteractors.Count);
        bool wasAllowed = allowedInteractors.TryGetValue(interactor, out float lastSelectedTime);
        
        if(wasAllowed)
        {
            float timeNow = Time.time;

            //allow select if there is still time left
            if(timeNow - lastSelectedTime < allowCycleTime)
            {
                return true;
            }
            
            //there is no time left so remove the interactor from the allowed list
            allowedInteractors.Remove(interactor);
        }



        if (preventSelect)
        {
            List<IXRSelectInteractor> selectors = interactable.interactorsSelecting;

            bool playerIsSelecting = playerIsOneOfSelectors(selectors);
            if (playerIsSelecting)
            {
                allowedInteractors[interactor] = Time.time;
            }
            Debug.Log(allowedInteractors.Count);
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
