using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FlameExtinguish : MonoBehaviour
{
    [SerializeField]
    private VisualEffect fireVFX;

    [SerializeField]
    private VisualEffect extinguishParticles;

    [SerializeField]
    private float x;
    [SerializeField]
    private float y;
    [SerializeField]
    private float z;
    [SerializeField]

    // Multiplier modifies the 
    private float multiplier;

    // Ensures extinguishParticlesVFX plays only once.
    private bool playedOnce = false;

    // <summary>
    // This method is called once when the GameObject is first enabled.
    // It fetches the VisualEffect component the "Flames" and it's scale.
    // <summary>
    void Start()
    {
        fireVFX = transform.Find("Flames").gameObject.GetComponent<VisualEffect>();

        extinguishParticles = transform.Find("ExtinguishParticlesVFX").gameObject.GetComponent<VisualEffect>();

        extinguishParticles.Stop();

        // Fetch the current scale of the Flames VFX
        Vector3 scale = fireVFX.transform.localScale;
        x = scale.x;
        y = scale.y;
        z = scale.z;
    }

    // <summary>
    // This method reduces the scale of the flame and triggers the extinguishParticlesVFX.
    // Multiplier is used to control the rate of the flame's size reduction
    // One can use the multiplier from inspector mode to easily change the rate of the reduction
    // <summary>
    void Update()
    {
        x -= 0.006f * multiplier;
        y -= 0.006f * multiplier;
        z -= 0.006f * multiplier;
        if (x <= 0f && y <= 0f && z <= 0f)
        {
            x = 0f;
            y = 0f;
            z = 0f;
            fireVFX.Stop();
        }
        if (!playedOnce)
        {
            if (x >= 0.1f && x <= 0.2f && y >= 0.1f && y <= 0.2f && z >= 0.1f && z <= 0.2f)
            {
                extinguishParticles.Play();
                playedOnce = true;
            }
        }


        SetScale(x, y, z);

    }

    // <summary>
    // This method sets the local scale of the "Flames" GameObject using float values x, y, z.
    // <summary>
    private void SetScale(float x, float y, float z)
    {
        fireVFX.transform.localScale = new Vector3(x, y, z);
    }
}
