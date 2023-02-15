using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple;

// Original movement tutorial from https://www.youtube.com/watch?v=9n1NrP8bpyA
public class FireSpread : MonoBehaviour
{
    private FireGrid fireGrid;

    private Vector3 up = Vector3.zero,
        right = new Vector3(0, 90, 0),
        down = new Vector3(0, 180, 0),
        left = new Vector3(0, 270, 0),
        currentDir = Vector3.zero;

    private Vector3 nextPos, destination, direction;

    private float speed = 5f;
    private float rayLength = 1f;
    private bool canMove;

    [SerializeField]
    GameObject objectToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        fireGrid = FindObjectOfType<FireGrid>();
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
        Movement(nextPos, currentDir, canMove);
    }

    private void moveDown()
    {
        // Z- movement
        nextPos = Vector3.back;
        currentDir = down;
        canMove = true;
        Movement(nextPos, currentDir, canMove);
    }

    private void moveRight()
    {
        // X+ movement
        nextPos = Vector3.right;
        currentDir = right;
        canMove = true;
        Movement(nextPos, currentDir, canMove);
    }

    private void moveLeft()
    {
        // X- movement
        nextPos = Vector3.left;
        currentDir = left;
        canMove = true;
        Movement(nextPos, currentDir, canMove);
    }

    private void Movement(Vector3 nextPos, Vector3 currentDir, bool canMove)
    {
        // Weird tutorial if-condition between current pos and destination
        if (Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            transform.localEulerAngles = currentDir;
            if (canMove)
            {
                if (checkSurroundings())
                {
                    destination = transform.position + nextPos;
                    direction = nextPos;
                    canMove = false;
                }

            }
        }
    }

    private bool checkSurroundings()
    {
        Ray myRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);
        RaycastHit hit;

        //Debug.DrawRay(myRay.origin, myRay.direction, Color.red);

        if (Physics.Raycast(myRay, out hit, rayLength))
        {
            if (hit.collider.tag == "Structure" || hit.collider.tag == "FireGrid")
            {
                return false;
            }
        }

        return true;
    }

    private void spawnObject(Vector3 position, Quaternion rotation)
    {
        Instantiate(objectToSpawn, position, rotation);
    }
}
