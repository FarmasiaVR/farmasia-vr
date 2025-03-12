using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SpreadStick : MonoBehaviour {
    public LiquidType liquidType;
    public bool colliding = false;
    public int spreadingValue;
    public float movementSensitivity = 10f;
    private float lastXPosition;
    private float lastZPosition;
    

    void Update() {
        whileColliding(colliding);
    }
    public void OnTriggerEnter(Collider collision){
        GameObject orig = collision.gameObject;
        Bottle item = orig.GetComponent<Bottle>();
        if (item != null && item.ObjectType == ObjectType.AgarPlate){
            LiquidType collidedLiquid = item.Container.LiquidType;
            Debug.Log($"{item}, {collidedLiquid}");
            changeLiquidType(collidedLiquid);
            colliding = true;
        }
    }

    private void changeLiquidType(LiquidType incoming) {
        if ( liquidType != LiquidType.None ) {
            liquidType = incoming;
        }
    }

    private void whileColliding(bool collision) {
        float deltaX = transform.position.x - lastXPosition;
        float deltaZ = transform.position.z - lastZPosition;
        if (Mathf.Abs(deltaX) > 0.0001f) {
            int changeAmount = Mathf.RoundToInt(deltaX * movementSensitivity);
            spreadingValue+=Mathf.Abs(changeAmount)*200;
        }
        if (Mathf.Abs(deltaZ) > 0.0001f) {
            int changeAmount = Mathf.RoundToInt(deltaZ * movementSensitivity);
            spreadingValue+=Mathf.Abs(changeAmount)*200;
        }
        lastXPosition = transform.position.x;
        lastZPosition = transform.position.z;
    }

    private void OnTriggerExit(Collider collision) {
        GameObject orig = collision.gameObject;
        Bottle item = orig.GetComponent<Bottle>();
        if (item != null && item.ObjectType == ObjectType.AgarPlate){
            colliding = false;
        }
    }

}