using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterDelay : MonoBehaviour
{
    [SerializeField] private bool startDelayWhenEnabled;
    [SerializeField] private float delay=5f;

    void OnEnable()
    {
        if (startDelayWhenEnabled)
            StartCoroutine("DisableDelay");
    }

    public void startDisableDelay()
    {
        StartCoroutine("DisableDelay");
    }

    IEnumerator DisableDelay()
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
