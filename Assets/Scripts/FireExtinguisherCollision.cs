using UnityEngine;

public class FireExtinguisherCollision : MonoBehaviour
{
    private FireGrid fireGrid;

    // Fetches the FireGrid script that's attached to the FireGridObject
    private void Start()
    {
        fireGrid = FindObjectOfType<FireGrid>();
    }

    // Detects if foam particle system collides with Collider Cube situated inside FireGridObject
    private void OnParticleCollision(GameObject collision)
    {
        if (collision.name == "ColliderCube")
        {
            fireGrid.Extinguish();
        }
    }
}
