using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlateBoxesManager : MonoBehaviour
{
    public BoxController box1;
    public BoxController box2;
    public UnityEvent<bool> onBoxesReady;

    public void AreAllBoxesReady()
    {
        if (box1 == null || box2 == null)
        {
            Logger.Print("One or both BoxControllers are missing!");
            return;
        }

        bool box1Ready = box1.AreAllPlatesReady();
        bool box2Ready = box2.AreAllPlatesReady();

        // Logger.Print($"Box1 Ready: {box1Ready}, Box2 Ready: {box2Ready}");

        onBoxesReady?.Invoke(box1Ready && box2Ready);
    }
}
