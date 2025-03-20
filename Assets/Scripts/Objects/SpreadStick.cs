using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SpreadStick : MonoBehaviour {
    public LiquidType liquidType;
    public bool colliding = false;
    public float movementSensitivity = 10f;
    private float lastXPosition;
    private float lastZPosition;
    private AgarPlateBottom plateBottom;
    

    [Tooltip("This is called when Stick is contaminated")]
    public UnityEvent<string, int> contaminatedStickUsed;

    void Update() {
        if (plateBottom != null && colliding == true) whileColliding(colliding);
    }

    public void OnTriggerEnter(Collider collision){
        GameObject collidingObject = collision.gameObject;
        plateBottom = collidingObject.GetComponent<AgarPlateBottom>();
        if (plateBottom != null && plateBottom?.isOpen == true){
            LiquidType collidedLiquid = plateBottom.Container.LiquidType;
            changeLiquidType(collidedLiquid);
            colliding = true;
        }
    }

    private void OnTriggerExit(Collider collision) {
        if (colliding == true) {
            colliding = false;
            plateBottom = null;
        }
    }
    private void changeLiquidType(LiquidType incoming) {
        if ( liquidType == LiquidType.None ) {
            liquidType = incoming;
        }
        if ( liquidType != incoming ) {
            contaminatedStickUsed.Invoke("ContaminatedStick", 1);
        } //this would be a good place to add an event to see if spreadstick has been in another agar plate
    }

    private void whileColliding(bool collision) {
        if (plateBottom.Container.Amount == 0) { return; }
        if (plateBottom.spreadingStatus == false) {
            float deltaX = transform.position.x - lastXPosition;
            float deltaZ = transform.position.z - lastZPosition;
            int changeXAmount = Mathf.RoundToInt(deltaX * movementSensitivity);
            int changeZAmount = Mathf.RoundToInt(deltaZ * movementSensitivity);
            int changeAmountsSum = Mathf.Min(100,Mathf.Abs(changeXAmount)*100) + Mathf.Min(100,Mathf.Abs(changeZAmount)*100);
            plateBottom.spreadValue+=changeAmountsSum;
            lastXPosition = transform.position.x;
            lastZPosition = transform.position.z;
        }
    }

}