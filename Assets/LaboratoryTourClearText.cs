using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaboratoryTourClearText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI endtext = GetComponent<TextMeshProUGUI>();
        TaskDescriptionManager taskDescriptionManager = FindObjectOfType<TaskDescriptionManager>();

        if (taskDescriptionManager != null)
        {
            int points = taskDescriptionManager.getValue();
            endtext.text = Translator.Translate("LaboratoryTour", "MissionCompleted") + " " + points + "/25";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
