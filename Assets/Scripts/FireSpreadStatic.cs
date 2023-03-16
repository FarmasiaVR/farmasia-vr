using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpreadStatic : MonoBehaviour
{
    private FireGrid fireGrid;
    //private FirePositions firePositions;
    private GameObject[] wallStructure;
    private GameObject floor;

    private int randomNumber, lastNumber, rngNum;
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
    private Vector3 nextPos, destination, currentPos;

    private float rayLength = 1f;
    private float timeSinceLast;

    //private bool canMove;
    private bool upFire, downFire, rightFire,
        leftFire, upRightFire, upLeftFire,
        downRightFire, downLeftFire;

    [SerializeField]
    GameObject objectToSpawn;
    [SerializeField]
    bool autonomous;

    // Start is called before the first frame update
    void Start()
    {
        fireGrid = FindObjectOfType<FireGrid>();
        //firePositions = FindObjectOfType<FirePositions>();
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
        timeSinceLast += Time.deltaTime;
        if(timeSinceLast > 2 && autonomous == true)
        {
            rngNum = RandomNumber();
            DoChecks(rngNum);
            timeSinceLast = 0;
        }
        
        // Press F to pay respects and create more clones
        // fireGrid.IsIgnited()
        //if (Input.GetKeyDown(KeyCode.F))
        //{
            
        //}
    }

    /// <summary>
    /// Beginning of the method calls. If that direction hasn't been set to true, starts the checks and possibly 
    /// spawns a new object later in the methods. Sets true to direction boolean either way. 
    /// </summary>
    private void DoChecks(int rngNum)
    {
        // Check up
        if (!upFire && rngNum == 0)
        {
            if (CheckDirection(Vector3.forward))
            {
                upFire = true;
            }
        }

        // Check down
        if (!downFire && rngNum == 1)
        {
            if (CheckDirection(Vector3.back))
            {
                downFire = true;
            }
        }

        // Check right
        if (!rightFire && rngNum == 2)
        {
            if (CheckDirection(Vector3.right))
            {
                rightFire = true;
            }
        }

        // Check left
        if (!leftFire && rngNum == 3)
        {
            if (CheckDirection(Vector3.left))
            {
                leftFire = true;
            }
        }

        // Check up-right
        if (!upRightFire && rngNum == 4)
        {
            if (CheckDirection(upRight))
            {
                upRightFire = true;
            }
        }

        // Check up-left
        if (!upLeftFire && rngNum == 5)
        {
            if (CheckDirection(upLeft))
            {
                upLeftFire = true;
            }
        }

        // Check down-right
        if (!downRightFire && rngNum == 6)
        {
            if (CheckDirection(downRight))
            {
                downRightFire = true;
            }
        }

        // Check down-left
        if (!downLeftFire && rngNum == 7)
        {
            if (CheckDirection(downLeft))
            {
                downLeftFire = true;
            }
        }

    }

    /// <summary>
    /// Sets and starts the check of the current direction. 
    /// Returns true, if passes CheckMovementObstacles(), else returns false.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool CheckDirection(Vector3 direction)
    {
        // Calculate next position and rotation based on direction
        nextPos = direction;
        // Get current position
        currentPos = transform.position;
        // Check for obstacles in the direction
        if (CheckMovementObstacles(nextPos))
        {
            // Check from current position if we're higher than the floor. Floor should be at y = 0.
            /*if(currentPos.y > 0.001f){
                Debug.Log("Doing a skyraycast next");
                // Do a ray cast from sky downwards and position fire grid lower, if possible
                currentPos = SkyRayCast(currentPos + nextPos);
            }*/
            // Calculate destination and spawn object
            destination = currentPos + nextPos;

            // Method with a timer, used in debugging, can be used later to time spawns
            //StartCoroutine(TimeOutCoroutine(destination, Quaternion.Euler(nextPos), fireGrid));
            SpawnFireGridObject(destination, Quaternion.Euler(nextPos), fireGrid);

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
    /*private bool CheckPositionAvailability(Vector3 position)
    {
        Debug.Log("vector3 position: " + position.ToString());
        return firePositions.CheckContains(position);
    }*/


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

        if (Physics.Raycast(oneRay, out RaycastHit hit, rayLength))
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
    /*IEnumerator TimeOutCoroutine(Vector3 destination, Quaternion rotation, FireGrid fireGrid)
    {
        yield return new WaitForSeconds(1);
    }*/

    /// <summary>
    /// Spawn a FireGridObject in the wanted position, with the wanted rotation. Requires fireGrid
    /// object in the parameter to copy this script to the new clone FireGridObject.
    /// </summary>
    /// <param name="position">the object spawn position.</param>
    /// <param name="rotation">the object spawn rotation.</param>
    /// <param name="fireGrid">fireGrid object reference required for copying this script.</param>
    private void SpawnFireGridObject(Vector3 position, Quaternion rotation, FireGrid fireGrid)
    {
        //firePositions.AddPosition(position);
        GameObject obj = Instantiate(objectToSpawn, position, rotation);
        obj.GetComponent<FireSpreadStatic>().fireGrid = fireGrid;
        //obj.tag = "FireGrid";

    }

   private int RandomNumber()
   {
    randomNumber = UnityEngine.Random.Range(0,7);
    if(randomNumber == lastNumber)
    {
        randomNumber = UnityEngine.Random.Range(0,7);
    }
    lastNumber = randomNumber;
    return randomNumber;
   }

   private Vector3 SkyRayCast(Vector3 checkPos)
   {
    Debug.Log("This is checkPos values: " + checkPos.ToString());
    Ray oneRay = new Ray(checkPos, new Vector3(checkPos.x, -3, checkPos.z));
    Debug.Log("This is raycast ray: " + oneRay.ToString());
    if (Physics.Raycast(oneRay, out RaycastHit hit, rayLength*3))
    {
        Debug.Log("Raycast hit!");
        if(hit.collider.CompareTag("Furniture"))
        {
            Debug.Log("Hit furniture!");
            return hit.collider.transform.position;
        }
        if(hit.collider.CompareTag("Floor"))
        {
            Debug.Log("Hit floor!");
            return hit.collider.transform.position;
        }
    }
    return checkPos;

   }

}
