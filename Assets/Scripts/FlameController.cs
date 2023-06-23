using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FlameController : MonoBehaviour
{
    [SerializeField]
    private VisualEffect fireVFX;

    [SerializeField]
    private float flameSize = 0.0f;

    private float x = 0.0f;
    private float y = 0.0f;
    private float z = 0.0f;

    // Summary
    // This method is called once when the GameObject is first enabled.
    // It fetches the VisualEffect component the "Flames" and sets its
    // initial scale using the SetScale method.
    void Start()
    {
        x = y = z = flameSize;
        fireVFX = transform.Find("Flames").gameObject.GetComponent<VisualEffect>();
        SetScale(x, y, z);
    }

    // Summary
    // Update method is called once per frame to update the scale of the GameObject "Flames".
    // The maximum size is set to 2. It will increase the value of all the axis by 0.03 when
    // scale is smaller that 0.5 and by 0.02 when the scale is smaller that 2.
    // Finally, the updated values are passed to the SetScale method.
    void Update()
    {
        if (x >= 2f && y >= 2f && z >= 2f)
        {
            x = 2f;
            y = 2f;
            z = 2f;
        }
        if (x < 0.5f && y < 0.5f && z < 0.5f)
        {
            x += 0.03f * Time.deltaTime;

            y += 0.03f * Time.deltaTime;

            z += 0.03f * Time.deltaTime;
        }

        if (x < 2f && y < 2f && z < 2f)
        {
            x += 0.02f * Time.deltaTime;
            y += 0.02f * Time.deltaTime;
            z += 0.02f * Time.deltaTime;
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