using UnityEngine;
using UnityEngine.Events;

public class AcidExtinguisherShower : MonoBehaviour 
{
    [Tooltip("Time in seconds the player must use the shower to trigger the end screen.")]
    public float requiredShowerTime = 5f;
    private float showerUsageTimer = 0f;
    private bool isPlayerInShower = false;
    private bool isPlayerShowering = false;

    // Reference to the ShowerToggler script
    public ShowerToggler showerToggler;

    // Event to be invoked when the end screen should be shown
    public UnityEvent onEndScreenTrigger;

    private void Update() {

        if (!isPlayerShowering && isPlayerInShower && showerToggler.open) {
            StartShowerUsage();
        }

        if (isPlayerShowering) {
            showerUsageTimer += Time.deltaTime;

            if (showerUsageTimer >= requiredShowerTime) {
                onEndScreenTrigger.Invoke();
                ResetShowerUsage();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerCollider")) {
            isPlayerInShower = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("PlayerCollider")) {
            StopShowerUsage();
        }
    }

    public void StartShowerUsage() {
        Debug.Log("Started shower usage");
        isPlayerShowering = true;
        showerUsageTimer = 0f;
    }

    public void StopShowerUsage() {
        ResetShowerUsage();
    }

    private void ResetShowerUsage() {
        isPlayerInShower = false;
        isPlayerShowering = false;
        showerUsageTimer = 0f;
    }
}
