using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionNumberPrinter : MonoBehaviour
{
    public string versionPrefix;
    private TextMeshPro versionText;
    private string applicationVersion;  // The local variable for the application version

    void Start()
    {
        versionText = GetComponent<TextMeshPro>();
        applicationVersion = Application.version;  // Assign the application version to the local variable
        versionText.text += applicationVersion;    // Use the local variable here
    }
}
