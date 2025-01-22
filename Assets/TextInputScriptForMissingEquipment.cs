using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.SmartFormat.PersistentVariables;


public class TextInputScriptForMissingEquipment : MonoBehaviour
{
    public CabinetBase cabinet;
    public TMP_Text textObject;
    // Start is called before the first frame update

    public void GetMissingEquipmentAndUpdate()
    {
        if (cabinet.itemsNotInCabinet.Count > 0)
        {
            string shownText = Translator.Translate("XR MembraneFilteration 2.0", "MissingEquipmentsInTable") + System.Environment.NewLine + System.Environment.NewLine;

            foreach (string i in cabinet.itemsNotInCabinet)
            {
                shownText += i + System.Environment.NewLine;
            }
            textObject.text = shownText;
        } else
        {
            textObject.text = "";
        }
    }
}
