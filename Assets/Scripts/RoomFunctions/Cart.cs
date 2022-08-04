using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    private GameObject cart;
    private float speed;
    private Vector3 pos = new Vector3(0, 0, 3);
     public void MoveCart()
    {
        while (cart.transform.position != pos)
        {
            cart.transform.position = Vector3.Lerp(cart.transform.position, pos, Time.deltaTime * speed);
        }
    }
}
