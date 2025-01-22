using UnityEngine;

public class FireExtinguisherCollision : MonoBehaviour
{
    /// <summary>
    /// Detects if foam particle system collides with ColliderCube situated inside FireGridObject. 
    /// ColliderCube has a tag "FireGrid" attached to it.
    /// On collision with colliderCube fetches the object and calls its Extinguish script
    /// </summary>
    /// <param name="collision"></param>
    private void OnParticleCollision(GameObject collision)
    {
        if (collision.CompareTag("FireGrid"))
        {
            collision.GetComponentInParent<FireGrid>().Extinguish();
        }
    }
}
