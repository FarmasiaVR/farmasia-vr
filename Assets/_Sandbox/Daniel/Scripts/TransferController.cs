using UnityEngine;
using UnityEngine.Assertions;

public class TransferController : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject source;
    [SerializeField]
    private GameObject sink;
    [SerializeField]
    private KeyCode transferKey = KeyCode.F;
    [SerializeField]
    private KeyCode switchKey = KeyCode.T;
    #endregion

    private void FixedUpdate() {
        if (Input.GetKey(transferKey)) {
            LiquidContainer src = LiquidContainer.FindLiquidContainer(source.transform);
            LiquidContainer dst = LiquidContainer.FindLiquidContainer(sink.transform);
            src?.TransferTo(dst, 1);
        }
        if (Input.GetKeyDown(switchKey)) {
            GameObject tmp = source;
            source = sink;
            sink = tmp;
        }
    }
}
