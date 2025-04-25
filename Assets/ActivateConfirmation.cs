using UnityEngine;
using UnityEngine.UI;  
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;  

public class ActivateConfirmation : MonoBehaviour
{
    public GameObject ConfirmationPopup;
    public GameObject showMistakesButton;  
     


    void Start()
    {
       
        Button button = GetComponent<Button>();  
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnButtonPressed);
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        ConfirmationPopup.SetActive(true);
        this.gameObject.SetActive(false); 
        showMistakesButton.SetActive(false);
    }
}