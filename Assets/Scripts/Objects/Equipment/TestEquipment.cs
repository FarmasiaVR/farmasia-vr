using UnityEngine;
using UnityEngine.Assertions;

public class TestEquipment : GeneralItem {

    [SerializeField]
    private KeyCode transferButton;
    [SerializeField]
    private LiquidContainer src;
    [SerializeField]
    private LiquidContainer dest;

    protected override void Start() {
        base.Start();
        Assert.IsNotNull(src);
    }

    public void Update() {
        if (Input.GetKey(transferButton)) {
            int transferRate = 1; // 1 unit / update
            src.TransferTo(dest, transferRate);
        }
    }
}
