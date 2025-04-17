using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColonyCanvasPCM : MonoBehaviour
{
    public int matrixSize = 5;
    private int[,] matrix;
    private int numColonies;
    private int flipped = 0;
    // Number of found colonies
    private int found = 0;
    public string dilutionType = "1:100";

    [SerializeField] private GameObject colonyImg;
    private RectTransform canvasRectTransform;

    private void Awake()
    {
        canvasRectTransform = transform.Find("Display").GetComponent<RectTransform>();

        matrix = new int[matrixSize, matrixSize];

        switch(dilutionType)
        {
            case "1:100":
            {
                numColonies = Random.Range(7, 16);
                break;
            }
        }

        // Debug
        //numColonies = 88;
        
        Debug.Log("Colonies " + numColonies);
        SetColonyPositions();
    }

    private void SetColonyPositions()
    {
        var corners = new HashSet<(int, int)>
        {
            (0, 0),
            (0, 1),
            (1, 0),
            (0, matrixSize-1),
            (1, matrixSize-1),
            (0, matrixSize-2),
            (matrixSize-1, 0),
            (matrixSize-2, 0),
            (matrixSize-1, 1),
            (matrixSize-1, matrixSize-1),
            (matrixSize-2, matrixSize-1),
            (matrixSize-1, matrixSize-2)
        };

        while (flipped < numColonies)
        {
            int i = Random.Range(0, matrixSize);
            int j = Random.Range(0, matrixSize);

            if (matrix[i, j] == 0 && !corners.Contains((i, j)))
            {
                matrix[i, j] = 1;
                flipped++;
            }
        }
    }

    public void PrintMatrix()
    {
        for (int i = 0; i < matrixSize; i++)
        {
            string row = "";
            for (int j = 0; j < matrixSize; j++)
            {
                row += matrix[i, j] + " ";
            }
            Debug.Log(row);
        }
    }

    public void ShowCanvas()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        SpawnImages();
    }

    public void ColonyFound()
    {
        found += 1;
        Debug.Log("Found colonies: " + found);
    }

    private void SpawnImages()
    {
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;
        float squareSize = Mathf.Min(canvasWidth, canvasHeight) / matrixSize;

        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                if (matrix[i, j] == 1)
                {
                    GameObject colony = Instantiate(colonyImg, canvasRectTransform);
                    BoxCollider collider = colony.GetComponentInChildren<BoxCollider>();
                    collider.size = new Vector3(0.1f, squareSize, squareSize);
                    
                    foreach (Transform child in colony.transform)
                    {
                        RectTransform Rt = child.GetComponent<RectTransform>();

                        // Resizing the colony image to fit the canvas
                        Rt.sizeDelta = new Vector2(squareSize, squareSize);

                        Vector2 anchoredPosition = new Vector2(
                        (-canvasWidth / 2) + (j + 0.5f) * squareSize,
                        (canvasHeight / 2 ) - (i + 0.5f) * squareSize);
                        Rt.anchoredPosition = anchoredPosition;
                    }
                }
            }
        }
    }
}