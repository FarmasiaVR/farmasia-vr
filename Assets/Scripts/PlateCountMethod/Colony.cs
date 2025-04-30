using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

public class Colony : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites;

    // Chance for a colony to receive a single sprite
    [SerializeField]
    private List<int> threshold;
    private System.Random random = new System.Random();
    private Image image;
    private GameObject circle;
    private ColonyCanvasPCM canvas;
    private bool found = false;

    void Start()
    {
        image = GetComponent<Image>();
        circle = transform.parent.GetChild(1).gameObject;
        
        if (sprites.Count != threshold.Count)
        {
            Debug.LogWarning("Number of sprites and threshold values do not match! Could not initialize the image.");
        }
        else
        {
            int randomValue = random.Next(1, threshold.Last()+1);
            // Debug.Log("Got random value: " + randomValue);
            for (int i = 0; i < threshold.Count; i++)
            {
                if (randomValue <= threshold[i])
                {
                    image.sprite = sprites[i];
                    // Debug.Log("Assigning " + sprites[i]);
                    return;
                }
            }
        }
    }

    void Awake()
    {
        canvas = transform.parent.parent.parent.GetComponent<ColonyCanvasPCM>();
        if (!canvas)
        {
            Debug.LogWarning("Could not fetch ColonyCanvasPCM");
        }
    }

    public void MarkWithPen()
    {
        if (!found)
        {
            found = true;
            circle.SetActive(true);
            MarkDoneInCanvas();
        }
    }

    private void MarkDoneInCanvas()
    {
        if (!canvas) return;
        canvas.ColonyFound();
    }

}
