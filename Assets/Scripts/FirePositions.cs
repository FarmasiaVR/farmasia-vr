using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePositions : MonoBehaviour
{

    List<Vector3> positions = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPosition(Vector3 position)
    {
        positions.Add(position);
    }

    public bool checkContains(Vector3 position)
    {
        return positions.Contains(position);
    }

    public List<Vector3> getList()
    {
        return positions;
    }
}
