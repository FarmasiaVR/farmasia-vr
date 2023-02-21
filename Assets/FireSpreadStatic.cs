using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;

public class FireSpreadStatic : MonoBehaviour
{
    private FireGrid fireGrid;
    private FirePositions firePositions;
    private GameObject[] wallStructure;
    private GameObject floor;
    private List<Vector3> positions = new List<Vector3>();

    // Sets the rotation variables
    private Vector3 up = Vector3.zero,
        right = new Vector3(0, 90, 0),
        down = new Vector3(0, 180, 0),
        left = new Vector3(0, 270, 0),
        currentDir = Vector3.zero;

    private Vector3 upRight = new Vector3(1, 0, 1),
        upLeft = new Vector3(-1, 0, 1),
        downRight = new Vector3(1, 0, -1),
        downLeft = new Vector3(-1, 0, -1);
    private Vector3 nextPos, destination, direction;

    private float rayLength = 1f;
    //private bool canMove;
    private bool upFire, downFire, rightFire, leftFire, upRightFire, upLeftFire, downRightFire, downLeftFire;

    [SerializeField]
    GameObject objectToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        fireGrid = FindObjectOfType<FireGrid>();
        firePositions = FindObjectOfType<FirePositions>();
        wallStructure = GameObject.FindGameObjectsWithTag("Structure");
        floor = GameObject.FindGameObjectWithTag("Floor");
        currentDir = up;
        nextPos = Vector3.forward;
        destination = transform.position;
        firePositions.addPosition(destination);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("isIgnited status: " + fireGrid.IsIgnited());
        // Press space bar to change isIgnited to true on all clones which creates more clones
        if (fireGrid.IsIgnited())
        {
            doChecks();
        }
    }

    private void doChecks()
    {
        // Check up
        if (!upFire)
        {
            if (checkDirection(Vector3.forward))
            {
                upFire = true;
            }
        }

        // Check down
        if (!downFire)
        {
            if (checkDirection(Vector3.back))
            {
                downFire = true;
            }
        }

        // Check right
        if (!rightFire)
        {
            if (checkDirection(Vector3.right))
            {
                rightFire = true;
            }
        }

        // Check left
        if (!leftFire)
        {
            if (checkDirection(Vector3.left))
            {
                leftFire = true;
            }
        }

        // Check up-right
        if (!upRightFire)
        {
            if (checkDirection(upRight))
            {
                upRightFire = true;
            }
        }

        // Check up-left
        if (!upLeftFire)
        {
            if (checkDirection(upLeft))
            {
                upLeftFire = true;
            }
        }

        // Check down-right
        if (!downRightFire)
        {
            if (checkDirection(downRight))
            {
                downRightFire = true;
            }
        }

        // Check down-left
        if (!downLeftFire)
        {
            if (checkDirection(downLeft))
            {
                downLeftFire = true;
            }
        }

    }

    private bool checkDirection(Vector3 direction)
    {
        // Calculate next position and rotation based on direction
        nextPos = direction;
        currentDir = Quaternion.LookRotation(direction).eulerAngles;

        // Check for obstacles in the direction
        if (checkMovementObstacles(direction) && !checkPositionAvailability(transform.position + nextPos))
        {

            // Calculate destination and spawn object
            destination = transform.position + nextPos;
            if (checkPositionAvailability(destination))
            {
                return false;
            }
            Debug.Log("Current destination: " + destination + " and list status: " + string.Join(",", firePositions.getList()) + " list length: " + firePositions.getList().Count);
            // Method with a timer, used in debugging
            StartCoroutine(timeOutCoroutine(destination, Quaternion.Euler(currentDir), fireGrid));

            return true;
        }

        return false;
    }

    private bool checkPositionAvailability(Vector3 position)
    {
        Debug.Log("vector3 position: " + position.ToString());
        return firePositions.checkContains(position);
    }

    /**
     * Method to check whether an object belonging to the tags Structure or FireGrid is in assigned direction. 
     * Objects with structure tag are walls in the scene and FireGrid tag refers to all FireGridObjects.
     */
    private bool checkMovementObstacles(Vector3 direction)
    {
        // Note the y-axis position for the ray as the ColliderCube of FireGridObject is of scale 0.01
        Ray oneRay = new Ray(transform.position + new Vector3(0, 0.01f, 0), direction);
        RaycastHit hit;

        if (Physics.Raycast(oneRay, out hit, rayLength))
        {
            if (hit.collider.tag == "Structure" || hit.collider.CompareTag("FireGrid"))
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator timeOutCoroutine(Vector3 destination, Quaternion rotation, FireGrid fireGrid)
    {
        yield return new WaitForSeconds(1);
        spawnFireGridObject(destination, rotation, fireGrid);
    }
    private void spawnFireGridObject(Vector3 position, Quaternion rotation, FireGrid fireGrid)
    {
        firePositions.addPosition(position);
        GameObject obj = Instantiate(objectToSpawn, position, rotation);
        obj.GetComponent<FireSpreadStatic>().fireGrid = fireGrid;
        obj.tag = "FireGrid";

    }

}
