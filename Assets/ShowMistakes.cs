using UnityEngine;
using UnityEngine.UI;  
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;  

public class ShowMistakes : MonoBehaviour
{
    public TextMeshProUGUI textMesh;  


    void Start()
    {
       
        Button button = GetComponent<Button>();  
        XRSimpleInteractable interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnButtonPressed);
    }

    void OnButtonPressed(SelectEnterEventArgs args)
    {
        
        if (textMesh != null)
        {
            textMesh.enabled = true;
            this.gameObject.SetActive(false);  
        }


    }
}