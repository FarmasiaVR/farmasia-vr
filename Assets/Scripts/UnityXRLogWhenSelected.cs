using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class UnityXRLogWhenSelected : MonoBehaviour {
    // Start is called before the first frame update
    float dmgTaken;
    float dmgRate;
    bool shouldTakeDmg;

    bool shouldScaleDown;

    [SerializeField]
    float scaleDownMultiplier;

    [SerializeField]
    float scaleUpMultiplier;

    [SerializeField]
    Vector3 initialScale;


    bool isPointed;
    void Start() {
        dmgTaken = 0;
        dmgRate = 0;
        shouldTakeDmg = false;
        shouldScaleDown = false;
        isPointed = false;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(shouldTakeDmg) {
            dmgTaken += dmgRate * Time.deltaTime;
            Debug.Log("Taking damage! current damage: " + dmgTaken);
        }


        if(isPointed) {
            //Debug.Log(Time.deltaTime);
            transform.localScale *= 1 - Time.deltaTime * scaleDownMultiplier;
        } else {
            if(transform.localScale.x / initialScale.x < 1) {
                transform.localScale *= 1 + (Time.deltaTime * scaleUpMultiplier);
            }
           
        }
    }

    public void setPointing(bool status) {
        isPointed = status;
    }




     public void takeDamageWhenSelected(float amount){
        dmgRate = amount;
        shouldTakeDmg = true;
        shouldScaleDown = true;
    }

    public void stopTakingDamage() {
        dmgRate = 0;
        shouldTakeDmg = false;
        shouldScaleDown = false;
    }




}
