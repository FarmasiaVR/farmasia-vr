using UnityEditor;
using UnityEngine;

public class ShadowDisabler : MonoBehaviour
{
    [MenuItem("Tools/Disable All Shadows")]
    static void DisableAllShadows()
    {
        var renderers = FindObjectsOfType<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        Debug.Log($"Disabled shadows on {renderers.Length} renderers.");
    }
}