using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FlameExtinguish : MonoBehaviour
{
    [SerializeField]
    private VisualEffect fireVFX;

    [SerializeField]
    private float x;
    [SerializeField]
    private float y;
    [SerializeField]
    private float z;

    // Summary
    // This method is called once when the GameObject is first enabled.
    // It fetches the VisualEffect component the "Flames" and it's scale.
    void Start()
    {
        fireVFX = transform.Find("Flames").gameObject.GetComponent<VisualEffect>();
        Vector3 scale = fireVFX.transform.localScale;
        x = scale.x;
        y = scale.y;
        z = scale.z;
    }

    // Summary
    // 
    void Update()
    {
        x -= 0.006f;
        y -= 0.006f;
        z -= 0.006f;
        if (x <= 0f && y <= 0f && z <= 0f)
        {
            x = 0f;
            y = 0f;
            z = 0f;
        }
        SetScale(x, y, z);
    }

    // Summary
    // This method sets the local scale of the "Flames" GameObject using float values x, y, z.
    private void SetScale(float x, float y, float z)
    {
        fireVFX.transform.localScale = new Vector3(x, y, z);
    }
}
