using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using Debug = UnityEngine.Debug;

public class FireSpreadStatic : MonoBehaviour
{
    private FireGrid fireGrid;
    private FirePositions firePositions;
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

    private float spawnTime = 0f;

    private bool diagonal;
    private int numInc;

    // Start is called before the first frame update
    void Start()
    {
        numInc = 0;
        fireGrid = GetComponent<FireGrid>();
        firePositions = FindObjectOfType<FirePositions>();
        currentDir = up;
        nextPos = Vector3.forward;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;

        if (fireGrid.IsIgnited())
        {
            timeSinceLast += Time.deltaTime;
            if (timeSinceLast > 2 && autonomous == true)
            {
                rngNum = RandomNumber();
                DoChecks(rngNum);
                timeSinceLast = 0;
            }
        }

    }

    private int RandomNumber()
    {
        randomNumber = UnityEngine.Random.Range(0, 7);
        if (randomNumber == lastNumber)
        {
            randomNumber = UnityEngine.Random.Range(0, 7);
        }
        lastNumber = randomNumber;
        return randomNumber;
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
            diagonal = false;
            CheckDirection(Vector3.forward, diagonal);
            upFire = true;

        }

        // Check down
        if (!downFire && rngNum == 1)
        {
            diagonal = false;
            CheckDirection(Vector3.back, diagonal);
            downFire = true;

        }

        // Check right
        if (!rightFire && rngNum == 2)
        {
            diagonal = false;
            CheckDirection(Vector3.right, diagonal);
            rightFire = true;

        }

        // Check left
        if (!leftFire && rngNum == 3)
        {
            diagonal = false;
            CheckDirection(Vector3.left, diagonal);
            leftFire = true;

        }

        // Check up-right
        if (!upRightFire && rngNum == 4)
        {
            diagonal = true;
            CheckDirection(upRight, diagonal);
            upRightFire = true;
        }

        // Check up-left
        if (!upLeftFire && rngNum == 5)
        {
            diagonal = true;
            CheckDirection(upLeft, diagonal);
            upLeftFire = true;
        }

        // Check down-right
        if (!downRightFire && rngNum == 6)
        {
            diagonal = true;
            CheckDirection(downRight, diagonal);
            downRightFire = true;

        }

        // Check down-left
        if (!downLeftFire && rngNum == 7)
        {
            diagonal = true;
            CheckDirection(downLeft, diagonal);
            downLeftFire = true;

        }

    }

    /// <summary>
    /// Sets and starts the check of the current direction. 
    /// Returns true, if passes CheckMovementObstacles(), else returns false.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private void CheckDirection(Vector3 direction, bool diagonal)
    {
        // Calculate next position and rotation based on direction
        nextPos = direction;
        
        // Get current position
        currentPos = transform.position;

        Debug.Log("Going to direction of: " + nextPos.ToString());

        //Debug.Log("Value of CheckMovementObstacles: " + CheckMovementObstacles(nextPos));
        // Check for obstacles in the direction
        if (CheckMovementObstacles(nextPos, diagonal, "Structure") && (CheckMovementObstacles(nextPos, diagonal, "FireGrid")))
        {
            // Calculate destination and spawn object
            // Spread distance is changed by using ternary operation. First 5 second spread is 0.1 units. From 5 to 7 seconds 0.2 units and from 7 to 10 seconds 0.4 units and so on.
            destination = currentPos + nextPos * ((spawnTime < 5f) ? 0.1f : (spawnTime < 7f) ? 0.2f : (spawnTime < 10f) ? 0.4f : (spawnTime < 14f) ? 0.7f : 1f);

            // Check from current position if we're higher than the floor. Floor should be at y = 0.
            /*if(currentPos.y > 0.001f){
                Debug.Log("Doing a skyraycast next");
                // Do a ray cast from sky downwards and position fire grid lower, if possible
                currentPos = SkyRayCast(currentPos + nextPos);
            }*/

            if (!CheckPositionAvailability(nextPos))
            {
                SpawnFireGridObject(destination, Quaternion.Euler(nextPos), fireGrid);
            }
            
            //SpawnFireGridObject(destination, Quaternion.Euler(nextPos), fireGrid);

            //return true;
        }

        //return false;
    }

    /// <summary>
    /// Checks if the position is in the List in the FirePositions script. Currently useless.
    /// Can maybe be later used to do HP status checks for grid positions. Who knows, dude.
    /// </summary>
    /// <param name="position">Vector3 parameter for spawn position.</param>
    /// <returns></returns>
    private bool CheckPositionAvailability(Vector3 position)
    {
        Debug.Log("vector3 position in CheckPositionAvailability: " + position.ToString());
        //Debug.Log("value of CheckFirePositionsContainsValue: " + firePositions.CheckFirePositionsContainsValue(position));
        Debug.Log("values in list of positions: " + firePositions.CheckContains(position));
        return firePositions.CheckContains(position);
        //return firePositions.CheckFirePositionsContainsValue(position);
    }


    /// <summary>
    /// Method to check whether an object belonging to the tags Structure or FireGrid is in assigned direction. 
    /// Objects with structure tag are walls in the scene and FireGrid tag refers to all FireGridObjects' ColliderCubes.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool CheckMovementObstacles(Vector3 direction, bool diagonal, string obstacle)
    {
        // Note that the diagonal length is according to Pythagoras' theorem
        float diagonalLength = Mathf.Sqrt(1 + 1);
        float length = diagonal ? diagonalLength : rayLength;

        Ray diagRay = new Ray(transform.position + new Vector3(0, 0.001f, 0), direction);

        if (Physics.Raycast(diagRay, out RaycastHit diagHit, length))
        {
            if (diagHit.collider.CompareTag(obstacle))
                {
                    Debug.Log("hit collider hit: " + diagHit.collider.name + " in the direction: " + direction);
                    return false;
                }
        }
        return true;
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
        if (CheckPositionAvailability(position))
        {
            Debug.Log("Position already occupied: " + position);
            return;
        }

        Debug.Log("Spawning object in position: " + position + " with rotation: " + rotation + " and name: " + objectToSpawn.name);
        firePositions.AddPosition(position);
        GameObject obj = Instantiate(objectToSpawn, position, rotation);
        obj.GetComponent<FireSpreadStatic>().fireGrid = fireGrid;

        //numInc++;
        //Debug.Log("incremented number is now: " + numInc);
        //objectToSpawn.name = objectToSpawn.name + numInc.ToString();
        //firePositions.AddFirePosition(objectToSpawn, position);
        //obj.tag = "FireGrid";
    }

    private Vector3 SkyRayCast(Vector3 checkPos)
    {
        Debug.Log("This is checkPos values: " + checkPos.ToString());

        Ray oneRay = new Ray(checkPos, new Vector3(checkPos.x, -3, checkPos.z));
        
        Debug.Log("This is raycast ray: " + oneRay.ToString());
        
        if (Physics.Raycast(oneRay, out RaycastHit hit, rayLength * 3))
        {
            Debug.Log("Raycast hit!");
            if (hit.collider.CompareTag("Furniture"))
            {
                Debug.Log("Hit furniture!");
                return hit.collider.transform.position;
            }
            if (hit.collider.CompareTag("Floor"))
            {
                Debug.Log("Hit floor!");
                return hit.collider.transform.position;
            }
        }
        return checkPos;
    }
}
