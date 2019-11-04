using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnableFactory : MonoBehaviour {
    private void Start() {
        GetComponent<Rigidbody>().isKinematic = true;
        gameObject.SetActive(false);

        GameObject factory = Instantiate(Resources.Load<GameObject>("Prefabs/Factory"));
        factory.transform.position = gameObject.transform.position;
        factory.transform.rotation = gameObject.transform.rotation;
        factory.transform.localScale = gameObject.transform.localScale;

        ObjectFactory f = factory.GetComponent<ObjectFactory>();
        f.CopyObject = gameObject;
        Assert.IsNotNull(f.CopyObject);

        Destroy(this);
    }
}
