using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple;

// Original movement tutorial from https://www.youtube.com/watch?v=9n1NrP8bpyA
public class FireSpread : MonoBehaviour
{
    private FireGrid fireGrid;
    private GameObject[] wallStructure;
    private GameObject floor;

    private Vector3 up = Vector3.zero,
        right = new Vector3(0, 90, 0),
        down = new Vector3(0, 180, 0),
        left = new Vector3(0, 270, 0),
        currentDir = Vector3.zero;

    private Vector3 nextPos, destination, direction;

    private float speed = 5f;
    private float rayLength = 1f;
    private bool canMove;
    private bool canJump = false;

    [SerializeField]
    GameObject objectToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        fireGrid = FindObjectOfType<FireGrid>();
        wallStructure = GameObject.FindGameObjectsWithTag("Structure");
        floor = GameObject.FindGameObjectWithTag("Floor");
        currentDir = up;
        nextPos = Vector3.forward;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        // Moves to new position and sets it as current position
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);


        moveUp();
        moveDown();
        moveRight();
        moveLeft();

    }

    private void moveUp()
    {
        // Z+ movement
        nextPos = Vector3.forward;
        currentDir = up;
        canMove = true;
        setCheckMovement(nextPos, currentDir, canMove);
    }

    private void moveDown()
    {
        // Z- movement
        nextPos = Vector3.back;
        currentDir = down;
        canMove = true;
        setCheckMovement(nextPos, currentDir, canMove);
        
    }

    private void moveRight()
    {
        // X+ movement
        nextPos = Vector3.right;
        currentDir = right;
        canMove = true;
        setCheckMovement(nextPos, currentDir, canMove);
    }

    private void moveLeft()
    {
        // X- movement
        nextPos = Vector3.left;
        currentDir = left;
        canMove = true;
        setCheckMovement(nextPos, currentDir, canMove);
    }

    private void setCheckMovement(Vector3 nextPos, Vector3 currentDir, bool canMove)
    {
        // Weird tutorial if-condition between current pos and destination
        if (Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            transform.localEulerAngles = currentDir;
            if (canMove)
            {
                if (checkMovementObstacles())
                {
                    destination = transform.position + nextPos;
                    direction = nextPos;
                    canMove = false;
                    spawnFireGridObject(destination, transform.rotation);
                }
                /*else if (!canJump)
                {
                    canJump = true;
                    destination = transform.position + 2 * nextPos;
                    direction = nextPos;
                    canMove = false;
                }
                else
                {
                    canJump = false;
                }*/

            }
        }
    }

    private bool checkMovementObstacles()
    {
        // Note the y-axis position for the ray as the ColliderCube of FireGridObject is of scale 0.01
        Ray oneRay = new Ray(transform.position + new Vector3(0, 0.01f, 0), transform.forward);
        RaycastHit hit;
        //Debug.DrawRay(myRay.origin, myRay.direction, Color.red);

        if (Physics.Raycast(oneRay, out hit, rayLength))
        {
            if (hit.collider.tag == "Structure" || hit.collider.CompareTag("FireGrid"))
            {
                return false;
            }
        }

        /*
        foreach (GameObject wall in wallStructure)
        {
            if (destination.x < wall.transform.position.x - 0.5f || destination.x > wall.transform.position.x + floor.transform.localScale.x - 0.5f ||
                destination.z < wall.transform.position.z - 0.5f || destination.z > wall.transform.position.z + floor.transform.localScale.z - 0.5f)
            {
                return false;
            }
        }*/
        return true;
    }

    private void spawnFireGridObject(Vector3 position, Quaternion rotation)
    {
        Instantiate(objectToSpawn, position, rotation);
    }
}
