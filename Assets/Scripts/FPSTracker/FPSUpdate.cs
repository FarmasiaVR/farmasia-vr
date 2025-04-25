using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public float updateDelay = 0f;
    private float _currentFPS = 0f;
    private float _deltatime = 0f;
    private TextMeshProUGUI _textFPS;

    // Start is called before the first frame update
    void Start()
    {
        _textFPS = GetComponent<TextMeshProUGUI>();
        StartCoroutine(DisplayFPS());
    }

    // Update is called once per frame
    void Update()
    {
        GenerateFPS();
    }


    private void GenerateFPS()
    {
        _deltatime += (Time.unscaledDeltaTime - _deltatime) * 0.1f;
        _currentFPS = 1.0f / _deltatime;
    }

    private IEnumerator DisplayFPS()
    {
        while (true)
        {
            _textFPS.text = "FPS: " + _currentFPS.ToString("0");
            yield return new WaitForSeconds(updateDelay);
        }
    }
}
