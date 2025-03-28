using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateBoxesManager : MonoBehaviour
{
    public BoxController box1;
    public BoxController box2; 

    public void AreAllBoxesReady()
    {
        if (box1 == null || box2 == null)
        {
            Logger.Print("One or both BoxControllers are missing!");
            return;
        }

        bool box1Ready = box1.AreAllPlatesReady();
        bool box2Ready = box2.AreAllPlatesReady();

        Logger.Print($"Box1 Ready: {box1Ready}, Box2 Ready: {box2Ready}");

        if(box1Ready && box2Ready){
            Logger.Print($"Task Complete ! Notify SceneManager");
        }
    }
}
