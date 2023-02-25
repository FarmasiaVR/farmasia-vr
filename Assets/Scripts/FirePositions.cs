using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePositions : MonoBehaviour
{

    List<Vector3> positions = new List<Vector3>();
    Dictionary<GameObject, int> objectHPs = new Dictionary<GameObject, int>();
    Dictionary<GameObject, Vector3> objectFirePosition = new Dictionary<GameObject, Vector3>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddObject(GameObject objectG, int hp)
    {
        objectHPs.Add(objectG, hp);
    }

    public void RemoveObject(GameObject objectG)
    {
        objectHPs.Remove(objectG);
    }

    public int GetHP(GameObject objectG)
    {
        try
        {
            return objectHPs[objectG];
        }
        catch (KeyNotFoundException)
        {
            return 0;
        }
    }

    public void AddFirePosition(GameObject objectG, Vector3 position)
    {
        objectFirePosition.Add(objectG, position);
    }

    public void RemoveFirePosition(GameObject objectG)
    {
        objectFirePosition.Remove(objectG);
    }

    public Vector3 GetFirePosition(GameObject objectG)
    {
        try
        {

        
        return objectFirePosition[objectG];
        }
        catch (KeyNotFoundException)
        {
            return Vector3.zero;
        }
     }

    public void ClearPositions()
    {
        positions.Clear();
    }

    public void AddPosition(Vector3 position)
    {
        positions.Add(position);
    }

    public bool CheckContains(Vector3 position)
    {
        return positions.Contains(position);
    }

    public List<Vector3> GetList()
    {
        return positions;
    }
}
