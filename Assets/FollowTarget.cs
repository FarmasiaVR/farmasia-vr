using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target; // Assign the target object you want to focus on in the Inspector.

    private int selectExitCount = 0;
    private float timer = 0.0f;

    public void startFollowing() {
        selectExitCount += 1;
    }

    void Update() {
        if (selectExitCount >= 2)
            timer += Time.deltaTime;

        if (target != null && timer >= 5.25f) {
            // Set the position of the CameraFocus object to the target's position.
            Vector3 newPos = target.position;
            newPos.x = transform.position.x;
            newPos.z = transform.position.z;
            newPos.y -= 0.6f;
            transform.position = newPos;
        }
    }
}