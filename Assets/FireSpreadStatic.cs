using System.Collections;
using System.Collections.Generic;
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
        //firePositions.addPosition(destination);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("isIgnited status: " + fireGrid.IsIgnited());

        // Press space bar to change isIgnited to true on all clones which creates more clones
        if (fireGrid.IsIgnited())
        {
            DoChecks();
        }
    }

    /// <summary>
    /// Beginning of the method calls. If that direction hasn't been set to true, starts the checks and possibly 
    /// spawns a new object later in the methods. Sets true to direction boolean either way. 
    /// </summary>
    private void DoChecks()
    {
        // Check up
        if (!upFire)
        {
            if (CheckDirection(Vector3.forward))
            {
                upFire = true;
            }
        }

        // Check down
        if (!downFire)
        {
            if (CheckDirection(Vector3.back))
            {
                downFire = true;
            }
        }

        // Check right
        if (!rightFire)
        {
            if (CheckDirection(Vector3.right))
            {
                rightFire = true;
            }
        }

        // Check left
        if (!leftFire)
        {
            if (CheckDirection(Vector3.left))
            {
                leftFire = true;
            }
        }

        // Check up-right
        if (!upRightFire)
        {
            if (CheckDirection(upRight))
            {
                upRightFire = true;
            }
        }

        // Check up-left
        if (!upLeftFire)
        {
            if (CheckDirection(upLeft))
            {
                upLeftFire = true;
            }
        }

        // Check down-right
        if (!downRightFire)
        {
            if (CheckDirection(downRight))
            {
                downRightFire = true;
            }
        }

        // Check down-left
        if (!downLeftFire)
        {
            if (CheckDirection(downLeft))
            {
                downLeftFire = true;
            }
        }

    }

    /// <summary>
    /// Sets and starts the check of the current direction. 
    /// Returns true, if passes CheckMovementObstacles() and not CheckPositionAvailability, else returns false.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool CheckDirection(Vector3 direction)
    {
        // Calculate next position and rotation based on direction
        nextPos = direction;
        //currentDir = Quaternion.LookRotation(direction).eulerAngles;
        //Debug.Log("is it in the list: " + CheckPositionAvailability(transform.position + nextPos));
        //&& !CheckPositionAvailability(transform.position + nextPos)

        // Check for obstacles in the direction
        if (CheckMovementObstacles(nextPos))
        {
            // Calculate destination and spawn object
            destination = transform.position + nextPos;

            Debug.Log("Current destination: " + destination + " and list status: " + string.Join(",", firePositions.getList()) + " list length: " + firePositions.getList().Count);
            // Method with a timer, used in debugging, can be used later to time spawns
            StartCoroutine(TimeOutCoroutine(destination, Quaternion.Euler(nextPos), fireGrid));

            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the position is in the List in the FirePositions script. Currently useless.
    /// Can maybe be later used to do HP status checks for grid positions. Who knows, dude.
    /// </summary>
    /// <param name="position">Vector3 parameter for spawn position.</param>
    /// <returns></returns>
    private bool CheckPositionAvailability(Vector3 position)
    {
        Debug.Log("vector3 position: " + position.ToString());
        return firePositions.checkContains(position);
    }

    /// <summary>
    /// Method to check whether an object belonging to the tags Structure or FireGrid is in assigned direction. 
    /// Objects with structure tag are walls in the scene and FireGrid tag refers to all FireGridObjects' ColliderCubes.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool CheckMovementObstacles(Vector3 direction)
    {
        // Note that the y-axis position doesn't need a "raise" i.e new Vector3(0, 0.001f, 0) as the hallway scene floor is set to -0.1 y-position.
        Ray oneRay = new Ray(transform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(oneRay, out hit, rayLength))
        {
            if (hit.collider.CompareTag("Structure") || hit.collider.CompareTag("FireGrid"))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Sets a time out before spawning a clone of the FireGridObject.
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="rotation"></param>
    /// <param name="fireGrid"></param>
    /// <returns></returns>
    IEnumerator TimeOutCoroutine(Vector3 destination, Quaternion rotation, FireGrid fireGrid)
    {
        yield return new WaitForSeconds(1);
        SpawnFireGridObject(destination, rotation, fireGrid);
    }

    /// <summary>
    /// Spawn a FireGridObject in the wanted position, with the wanted rotation. Requires fireGrid
    /// object in the parameter to copy this script to the new clone FireGridObject.
    /// </summary>
    /// <param name="position">the object spawn position.</param>
    /// <param name="rotation">the object spawn rotation.</param>
    /// <param name="fireGrid">fireGrid object reference required for copying this script.</param>
    private void SpawnFireGridObject(Vector3 position, Quaternion rotation, FireGrid fireGrid)
    {
        firePositions.addPosition(position);
        GameObject obj = Instantiate(objectToSpawn, position, rotation);
        obj.GetComponent<FireSpreadStatic>().fireGrid = fireGrid;
        //obj.tag = "FireGrid";

    }

}
