using UnityEngine;

public class FireExtinguisherCollision : MonoBehaviour
{
    private FireGrid fireGrid;

    // Fetches the FireGrid script that's attached to the FireGridObject
    private void Start()
    {
        fireGrid = FindObjectOfType<FireGrid>();
    }

    /// <summary>
    /// Detects if foam particle system collides with ColliderCube situated inside FireGridObject. 
    /// ColliderCube has a tag "FireGrid" attached to it.
    /// </summary>
    /// <param name="collision"></param>
    private void OnParticleCollision(GameObject collision)
    {
        if (collision.tag == "FireGrid")
        {
            fireGrid.Extinguish();
        }
    }
}
