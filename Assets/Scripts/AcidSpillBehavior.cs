using UnityEngine;
using System.Collections;

public class WaterEffectController : MonoBehaviour
{
    public ParticleSystem waterEffect;

    private void OnEnable()
    {
        StartWaterEffectForDuration(5f); // Start the effect for 5 seconds
    }

    // Method to start the water effect for a fixed duration
    private void StartWaterEffectForDuration(float duration)
    {
        if (waterEffect != null)
        {
            waterEffect.Play();
            StartCoroutine(StopEffectAfterDuration(duration));
        }
    }

    // Coroutine to stop the particle effect after a specified duration
    private IEnumerator StopEffectAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        waterEffect.Stop();
    }
}
