using System.Collections;
using UnityEngine;

public class WritingForwarder: WritingTarget {

    [SerializeField]
    private Writable writable;

    public override Writable GetWritable() {
        return writable;
    }
}
