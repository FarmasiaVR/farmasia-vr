using UnityEngine;

// This will add a new particle system to FireGridObject, not necessary now
//[RequireComponent(typeof(ParticleSystem))]

public class FireGrid : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.Space;

    // Different particle effect fields
    [SerializeField]
    private ParticleSystem fireParticle;
    [SerializeField]
    private ParticleSystem igniteParticle;
    [SerializeField]
    private ParticleSystem extinguishParticle;
    // Light source of the fire
    [SerializeField]
    private GameObject pointLight;

    //bool isPlaying = true;
    [SerializeField]
    private int degrees;

    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        //fireParticle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Hit spacebar to lower degrees by ten
        if (Input.GetKeyDown(toggleKey))
        {
            if (degrees >= 10)
            {
                degrees -= 10;
                Debug.Log("degrees should go down by -10: " + degrees);
            }
        }
        // If at 0 degrees stop the fire particle effect and play the extinguish particle once
        if (degrees == 0)
        {
            Extinguish();
        }
        else if (degrees > 0)
        {
            Ignite();
        }

    }

    public void Extinguish()
    {
        fireParticle.Stop();
        pointLight.SetActive(false);
        if (extinguishParticle != null && count == 0)
        {
            count++;
            extinguishParticle.Play();
        }
        //isPlaying = false;
        Debug.Log("degrees should be zero: " + degrees);
    }

    public void Ignite()
    {
        fireParticle.Play();
        pointLight.SetActive(true);
        if (igniteParticle != null)
            igniteParticle.Play();
        //isPlaying = true;
    }
}
