using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionNumberPrinter : MonoBehaviour
{
    public string versionPrefix;
    private TextMeshPro versionText;

    void Start()
    {
        versionText = GetComponent<TextMeshPro>();
        versionText.text = versionPrefix + Application.version;
    }
}
