using UnityEngine;

public class ColonyCanvasPCM : MonoBehaviour
{
    private int matrixSize = 5;
    private int squareSize = 400;
    private int[,] matrix;
    private int numColonies;
    private int flipped = 0;
    public string dilutionType = "1:100";

    private void Awake()
    {
        matrix = new int[matrixSize, matrixSize];

        switch(dilutionType)
        {
            case "1:100":
            {
                numColonies = Random.Range(7, 15);
                break;
            }
        }
        
        Debug.Log("Colonies " + numColonies);
        while (flipped < numColonies)
        {
            int i = Random.Range(0, matrixSize);
            int j = Random.Range(0, matrixSize);

            if (matrix[i, j] == 0 )
            {
                matrix[i, j] = 1;
                flipped++;
            }
        }
    }

    public void print()
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
}