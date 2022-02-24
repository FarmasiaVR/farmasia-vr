using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hose : MonoBehaviour
{
    [SerializeField]
    GameObject partPrefab, parentObject;

    [SerializeField]
    [Range(1, 1000)]
    int length = 1;

    [SerializeField]
    float partDistance = 0.21f;

    [SerializeField]
    bool reset, spawn, snapFirst, snapLast;

    #region spawn2 fields
    int fragmentCount;
    int activeFragmentCount;
    GameObject[] fragments;
    int splineFactor = 4;

    float[] xPositions;
    float[] yPositions;
    float[] zPositions;

    CatmullRomSpline splineX;
    CatmullRomSpline splineY;
    CatmullRomSpline splineZ;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Spawn();
        Spawn2();
        spawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (reset)
        {
            foreach(GameObject tmp in GameObject.FindGameObjectsWithTag("HosePart"))
            {
                Destroy(tmp);
            }
            reset = false;
        }
    }

    void LateUpdate()
    {
        // Copy rigidbody positions to the line renderer
        var lineRenderer = GetComponent<LineRenderer>();

        // No interpolation
        //for (var i = 0; i < fragmentNum; i++)
        //{
        //    renderer.SetPosition(i, fragments[i].transform.position);
        //}

        for (var i = 0; i < fragmentCount; i++)
        {
            var position = fragments[i].transform.position;
            xPositions[i] = position.x;
            yPositions[i] = position.y;
            zPositions[i] = position.z;
        }

        for (var i = 0; i < (fragmentCount - 1) * splineFactor + 1; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(
                splineX.GetValue(i / (float)splineFactor),
                splineY.GetValue(i / (float)splineFactor),
                splineZ.GetValue(i / (float)splineFactor)));
        }
    }

    public void Spawn()
    {
        int count = (int)(length / partDistance);

        for(int i = 0; i < count; i++)
        {
            GameObject tmp;

            tmp = Instantiate(partPrefab, new Vector3(transform.position.x, transform.position.y + partDistance * (i + 1), transform.position.z), Quaternion.identity, parentObject.transform);
            tmp.transform.eulerAngles = new Vector3(180, 0, 0);
            tmp.name = parentObject.transform.childCount.ToString();

            if(i == 0)
            {
                Destroy(tmp.GetComponent<CharacterJoint>());
            } else
            {
                tmp.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.Find((parentObject.transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
            }
        }
    }

    void Spawn2()
    {
        int fragmentCount = (int)(length / partDistance);

        activeFragmentCount = fragmentCount;

        fragments = new GameObject[fragmentCount];

        var position = transform.position;

        Vector3 interval = new Vector3(0, 0, partDistance);

        for (var i = 0; i < fragmentCount; i++)
        {
            fragments[i] = Instantiate(partPrefab, position, Quaternion.identity);
            fragments[i].transform.SetParent(transform);

            var joint = fragments[i].GetComponent<CharacterJoint>();
            if (i > 0)
            {
                joint.connectedBody = fragments[i - 1].GetComponent<Rigidbody>();
            }

            position += interval;
        }

        var lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = (fragmentCount - 1) * splineFactor + 1;

        xPositions = new float[fragmentCount];
        yPositions = new float[fragmentCount];
        zPositions = new float[fragmentCount];

        splineX = new CatmullRomSpline(xPositions);
        splineY = new CatmullRomSpline(yPositions);
        splineZ = new CatmullRomSpline(zPositions);
    }
}
