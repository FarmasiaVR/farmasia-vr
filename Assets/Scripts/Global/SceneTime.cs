using UnityEngine;

public class SceneTime : MonoBehaviour {
    int[] value;
    public int[] Value {
        get { return value; } 
    }

    private void Awake() {
        value = new int[] { (int)Random.Range(0, 11), (int)Random.Range(0, 59) };
    }
}
