using UnityEngine;
using UnityEngine.Events;

public class EquipmentFoundTrigger : MonoBehaviour {
    [SerializeField] private GameObject targetHighlightBox;
    [SerializeField] private GameObject targetTutorialStartButton;
    [SerializeField] private Material highlightingMaterial;
    
    private Material startingMaterial;
    private bool enteredOnce;
    public UnityEvent onFirstEnter = new UnityEvent();

    private void Start() {
        startingMaterial = Instantiate(targetHighlightBox.GetComponent<MeshRenderer>().material);
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "PlayerCollider" && !enteredOnce) {
            enteredOnce = true;
            targetHighlightBox.GetComponent<MeshRenderer>().material = highlightingMaterial;
            targetTutorialStartButton.SetActive(true);
            onFirstEnter.Invoke();
        }
    }
}