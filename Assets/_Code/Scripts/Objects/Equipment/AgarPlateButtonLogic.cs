using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AgarPlateButtonLogic : MonoBehaviour
{
    [SerializeField]
    public GameObject button;

    [SerializeField]
    public GameObject socket;

    private bool areLidsCombined;

    public XRGrabInteractable lid;

    XRInteractionManager manager;
    void Start()
    {
        manager = GetComponent<XRBaseInteractable>().interactionManager;
    }


    public void grabLid(SelectEnterEventArgs args)
    {
        socket.SetActive(false);

        IXRSelectInteractable interactable = GetComponent<IXRSelectInteractable>();
        IXRSelectInteractor interactorSelecting = null;

        if (interactable.isSelected)
        {
            interactorSelecting = interactable.firstInteractorSelecting;
            manager.SelectExit(interactable.firstInteractorSelecting, interactable);
        }

        manager.SelectEnter(interactorSelecting, lid);

        buttonDisabled();

        socket.SetActive(true);
    }
    public void buttonEnabled()
    {
        if (areLidsCombined)
        {
            button.SetActive(true);
        }

    }

    public void buttonDisabled()
    {
        button.SetActive(false);
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
