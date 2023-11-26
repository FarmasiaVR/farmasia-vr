using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class SpreadableFire : MonoBehaviour
{
    //grid configuration
    [SerializeField] float spreadingSpeed=1f, gridSquarewidth=1f,flameCollisionHeight=1f,meshUpdateDelay=0.4f;
    [SerializeField] int gridCols=4, gridRows=4;
    [SerializeField] BoxCollider bCollider;
    [SerializeField] Transform presetIgnitionPoint;

    //Things that use the generated mesh
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;
    public VisualEffect fireEffect;

    //events
    public UnityEvent fireExtinguished;
    public UnityEvent fireIgnited;

    //fire propagation
    private float spreadTimer = 0;
    private bool burning;
    private HashSet<Vector2> spreadableVerticies = new HashSet<Vector2>();
    private HashSet<Vector2> ignitedVerticies = new HashSet<Vector2>();
    private HashSet<Vector2> noneSpreadableVerticies = new HashSet<Vector2>();
    private Dictionary<Vector2, FireVertex> fireVerticies = new Dictionary<Vector2, FireVertex>();
    
    //mesh generatrion
    Mesh mesh;
    Dictionary<Vector3, int> vertexIndex = new Dictionary<Vector3, int>();
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    bool delayedGeneration;

    private void OnValidate()
    {
        if (!bCollider)
            bCollider = GetComponent<BoxCollider>();
        UpdateBoxCollider();
    }
    private void OnEnable()
    {
        PopulateGrid();
        mesh = new Mesh();
        UpdateBoxCollider();
        if (fireEffect)
        {
            fireEffect.Stop();
            fireEffect.SetMesh("fireMesh", GenerateTemporaryMeshForVFX());
            fireEffect.SetVector3("firePos", transform.localPosition);
            fireEffect.SetFloat("FlameHeight", flameCollisionHeight);
        }
    }

    private void Update()
    {
        //Debug.Log("burning" + noneSpreadableVerticies.Count);
        if (ignitedVerticies.Count < 1)
            return;

        if(spreadTimer >= spreadingSpeed)
        {
            spreadTimer = 0;
            Propagate();
            GenerateFireMesh();
            UpdateMeshUsage();
        }
        spreadTimer += Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        //Shows the grid when game object is selected
        for (int i = 0; i < gridRows + 1; i++)
        {
            for (int j = 0; j < gridCols + 1; j++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.TransformPoint(new Vector3((i - (gridRows / 2)) * gridSquarewidth, 0, (j - (gridCols / 2)) * gridSquarewidth)), 0.05f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("FireGrid"))
            IgniteClosestVertex(transform.InverseTransformPoint(collision.GetContact(0).point));
    }

    //Ignites closest Vertex to (local) positon
    private void IgniteClosestVertex(Vector3 point)
    {
        Vector2 flatenedPoint = WorldToGridPosition(point);
        if (spreadableVerticies.Contains(flatenedPoint))
        {
            ignitedVerticies.Add(flatenedPoint);
            spreadableVerticies.Remove(flatenedPoint);
            if (!burning)
            {
                burning = true;
                fireIgnited.Invoke();
            }
        }
    }
    //Ignition using function call from outside (world) position
    public void IgniteClosestVertexNonCollision(Vector3 point)
    {
        Vector3 p = transform.InverseTransformPoint(point);
        Vector2 flatenedPoint = WorldToGridPosition(p);
        if (spreadableVerticies.Contains(flatenedPoint))
        {
            ignitedVerticies.Add(flatenedPoint);
            spreadableVerticies.Remove(flatenedPoint);
            if (!burning)
            {
                burning = true;
                fireIgnited.Invoke();
            }
        }
    }

    public void IgniteAtPresetStartPoint()
    {
        if(presetIgnitionPoint)
            IgniteClosestVertexNonCollision(presetIgnitionPoint.transform.position);
    }
    //Updates the mesh used for collision and particle emission
    void UpdateMeshUsage()
    {
        if (meshFilter)
            meshFilter.mesh = mesh;
        if (meshCollider)
            meshCollider.sharedMesh = mesh;
        if (fireEffect && vertices.Count >= 3 && triangles.Count >= 3)
        {
            if (!fireEffect.HasAnySystemAwake())
                fireEffect.Play();
            fireEffect.SetMesh("fireMesh", mesh);
            fireEffect.SetVector3("firePos", transform.localPosition);
            return;
        }
        fireEffect.SetMesh("fireMesh", GenerateTemporaryMeshForVFX());
        if (burning)
        {
            if (ignitedVerticies.Count < 1)
            {
                burning = false;
                fireExtinguished.Invoke();
            }
        }
        
    }
    //Ignites closest Vertex to (local) positon
    private void ExtinguishClosestVertex(Vector3 point)
    {
        Vector2 flatenedPoint = WorldToGridPosition(point);
        if (spreadableVerticies.Contains(flatenedPoint))
        {
            noneSpreadableVerticies.Add(flatenedPoint);
            spreadableVerticies.Remove(flatenedPoint);
        }
        if (ignitedVerticies.Contains(flatenedPoint))
        {
            noneSpreadableVerticies.Add(flatenedPoint);
            ignitedVerticies.Remove(flatenedPoint);
        }
        if (fireVerticies.ContainsKey(flatenedPoint))
            fireVerticies[flatenedPoint].status = FireVertex.VertexStatus.extingushed;
        if (!delayedGeneration)
            StartCoroutine("DelayedMeshGenerationCall");
    }
    //Extinguish verticies in an radius around the closest vertex
    public void ExtinguishVerticesInRaidus(Vector3 point,float r=1)
    {
        Vector3 p = transform.InverseTransformPoint(point);
        Vector2 center = WorldToGridPosition(p);
        float radius = r / gridSquarewidth;
        int rowS, rowE, colS, colE;
        rowS = (int)(center.x - radius);
        rowE = (int)(center.x + radius);
        colS = (int)(center.y - radius);
        colE = (int)(center.y + radius);
        Debug.Log(center);
        Debug.Log(rowS.ToString() + " " + rowE.ToString() + " " + colS.ToString() + " " + colE.ToString());
        if (rowE > gridRows + 1)
            rowE = gridRows + 1;
        if (rowS < 0)
            rowS = 0;
        if (colE > gridCols + 1)
            colE = gridCols + 1;
        if (colS < 0)
            colS = 0;
        for(int i= rowS; i <= rowE; i++)
        {
            for (int j = colS; j <= colE; j++)
            {
                Vector2 vert = new Vector2(i, j);
                if (Vector2.SqrMagnitude(vert - center) <= radius * radius)
                {
                    if (fireVerticies.ContainsKey(vert))
                    {
                        fireVerticies[vert].status = FireVertex.VertexStatus.extingushed;
                        if (noneSpreadableVerticies.Contains(vert))
                            continue;
                        if (spreadableVerticies.Contains(vert))
                            spreadableVerticies.Remove(vert);
                        if (ignitedVerticies.Contains(vert))
                            ignitedVerticies.Remove(vert);
                        noneSpreadableVerticies.Add(vert);
                    }
                }
            }
        }
        if (!delayedGeneration)
            StartCoroutine("DelayedMeshGenerationCall");
    }

    private void Propagate()
    {
        //spreads the fire to verticies surroungding the ignited ones, and updates the buring verticies
        Debug.Log("calculating propagation");
        List<Vector2> removeList = new List<Vector2>();//since we cant update lists when they are iterated
        HashSet<Vector2> addList = new HashSet<Vector2>();//^^^^^^
        foreach(Vector2 vert in ignitedVerticies.ToList())
        {
            foreach(Vector2 newVert in GetSurroundingVerticies(vert))
            {
                if (spreadableVerticies.Contains(newVert))
                {
                    addList.Add(newVert);
                    spreadableVerticies.Remove(newVert);
                }
                    
            }
            removeList.Add(vert);
            noneSpreadableVerticies.Add(vert);
        }
        foreach(Vector2 vert in removeList)
        {
            if (fireVerticies.ContainsKey(vert))
                fireVerticies[vert].status = FireVertex.VertexStatus.burning;
            ignitedVerticies.Remove(vert);
        }
        foreach (Vector2 vert in addList.ToList())
        {
            ignitedVerticies.Add(vert);
        }
    }

    void UpdateBoxCollider()
    {
        if (bCollider)
            bCollider.size = new Vector3(gridSquarewidth * gridRows, bCollider.size.y, gridSquarewidth * gridCols);
    }

    //Goes through the grid and generates a mesh of the burning vericies
    private void GenerateFireMesh()
    {
        //Goes through the grid and generates a mesh of the burning vericies
        Debug.Log("generating mesh");
        vertexIndex.Clear();
        vertices.Clear();
        triangles.Clear();
        //breakes down burning verticies in the grid into sqares that canbe used to gernerate the mesh
        for (int i = 0; i < gridRows; i++)
        {
            int top =-1;
            int bot = -1;
            for (int j = 0; j < gridCols + 1; j++)
            {
                bool notBurning = !CheckIfBuring(new Vector2(i, j), new Vector2(i + 1, j));
                if (notBurning || j== gridCols)
                {
                    if (top == -1)
                        continue;
                    if (bot == -1 && notBurning)
                    {
                        top = -1;
                        continue;
                    }
                    else
                        bot = j;

                    SetVertexPostionAndTriangleIndex(i, j, top, bot);
                    top = -1;
                    bot =-1;
                }
                if (top == -1)
                    top = j;
                else
                    bot = j;
            }
        }
        Debug.Log("Baking");
        mesh.Clear();
        Debug.Log(triangles.ToArray());
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    //Checks if grid square edge is burning
    bool CheckIfBuring(Vector2 v1, Vector2 v2)
    {
        if (!fireVerticies.ContainsKey(v1))
            return false;
        if (!fireVerticies.ContainsKey(v2))
            return false;
        if (fireVerticies[v1].status == FireVertex.VertexStatus.burning && fireVerticies[v2].status == FireVertex.VertexStatus.burning)
            return true;
        return false;
    }

    private void PopulateGrid()
    {
        for (int i = 0; i < gridRows + 1; i++)
        {
            for (int j = 0; j < gridCols + 1; j++)
            {
                Vector2 pos = new Vector2(i, j);
                spreadableVerticies.Add(pos);
                fireVerticies.Add(pos, new FireVertex(pos));
            }
        }
    }

    //Makes sure that a vertex only has a single index that can be retrived via grid positon
    void AddVertexAndSetVertexIndex(int x, int y, int z)
    {
        if (!vertexIndex.ContainsKey(new Vector3(x, y, z)))
        {
            vertexIndex.Add(new Vector3(x, y, z), vertices.Count);
            vertices.Add(new Vector3((x - (gridRows / 2)) * gridSquarewidth, y * flameCollisionHeight, (z - (gridCols / 2)) * gridSquarewidth));
        }
    }

    //gets Surrounding vericies in grid
    public List<Vector2> GetSurroundingVerticies(Vector2 point)
    {
        List<Vector2> surroundingPoints = new List<Vector2>();
        for (int i = -1; i < 2; i++)
            for (int j = -1; j < 2; j++)
                surroundingPoints.Add(point + new Vector2(i, j));

        return surroundingPoints;
    }

    IEnumerator DelayedMeshGenerationCall()
    {
        delayedGeneration = true;
        yield return new WaitForSeconds(meshUpdateDelay);
        GenerateFireMesh();
        UpdateMeshUsage();
        delayedGeneration = false;
    }

    //converts for world space to gridspace
    Vector2 WorldToGridPosition(Vector3 point)
    {
        point = point / gridSquarewidth + new Vector3(gridRows / 2, 0, gridCols / 2);//converts for world space to gridspace
        Vector2 flatenedPoint = new Vector2((int)point.x, (int)point.z);
        if (flatenedPoint.x > gridRows + 1)
            flatenedPoint.x = gridRows + 1;
        if (flatenedPoint.x < 0)
            flatenedPoint.x = 0;
        if (flatenedPoint.y > gridCols + 1)
            flatenedPoint.y = gridCols + 1;
        if (flatenedPoint.y < 0)
            flatenedPoint.y = 0;
        return flatenedPoint;
    }

    void SetVertexPostionAndTriangleIndex(int i,int j,int top, int bot)
    {
        //Bottom plane Vertices  -- adds index if it does not have one
        AddVertexAndSetVertexIndex(i, 0, top);
        AddVertexAndSetVertexIndex(i, 1, top);
        AddVertexAndSetVertexIndex(i+1, 0, top);
        AddVertexAndSetVertexIndex(i+1, 1, top);

        //top plane Vertices
        AddVertexAndSetVertexIndex(i, 0, bot);
        AddVertexAndSetVertexIndex(i, 1, bot);
        AddVertexAndSetVertexIndex(i + 1, 0, bot);
        AddVertexAndSetVertexIndex(i + 1, 1, bot);

        //--Triangle Indexes-- 
        //clockwise triangle index and counterclockwise indexes give different normals 
        //hence the index order being different between front and back,left and right ect...
        //bottom triangles
        triangles.Add(vertexIndex[new Vector3(i,0, top)]);
        triangles.Add(vertexIndex[new Vector3(i + 1, 0, top)]);
        triangles.Add(vertexIndex[new Vector3(i, 0, bot)]);

        triangles.Add(vertexIndex[new Vector3(i + 1, 0, top)]);
        triangles.Add(vertexIndex[new Vector3(i + 1, 0, bot)]);
        triangles.Add(vertexIndex[new Vector3(i, 0, bot)]);

        //top triangles
        triangles.Add(vertexIndex[new Vector3(i, 1, bot)]);
        triangles.Add(vertexIndex[new Vector3(i + 1, 1, top)]);
        triangles.Add(vertexIndex[new Vector3(i, 1, top)]);

        triangles.Add(vertexIndex[new Vector3(i, 1, bot)]);
        triangles.Add(vertexIndex[new Vector3(i + 1, 1, bot)]);
        triangles.Add(vertexIndex[new Vector3(i + 1, 1, top)]);

        //left triangles
        triangles.Add(vertexIndex[new Vector3(i, 0, bot)]);
        triangles.Add(vertexIndex[new Vector3(i, 1, top)]);
        triangles.Add(vertexIndex[new Vector3(i, 0, top)]);

        triangles.Add(vertexIndex[new Vector3(i, 0, bot)]);
        triangles.Add(vertexIndex[new Vector3(i, 1, bot)]);
        triangles.Add(vertexIndex[new Vector3(i, 1, top)]);

        //right triangles
        triangles.Add(vertexIndex[new Vector3(i+1, 0, top)]);
        triangles.Add(vertexIndex[new Vector3(i+1, 1, top)]);
        triangles.Add(vertexIndex[new Vector3(i+1, 0, bot)]);

        triangles.Add(vertexIndex[new Vector3(i+1, 1, top)]);
        triangles.Add(vertexIndex[new Vector3(i+1, 1, bot)]);
        triangles.Add(vertexIndex[new Vector3(i+1, 0, bot)]);

        //front triangles
        triangles.Add(vertexIndex[new Vector3(i, 0, top)]);
        triangles.Add(vertexIndex[new Vector3(i , 1, top)]);
        triangles.Add(vertexIndex[new Vector3(i+1, 0, top)]);

        triangles.Add(vertexIndex[new Vector3(i+1, 0, top)]);
        triangles.Add(vertexIndex[new Vector3(i, 1, top)]);
        triangles.Add(vertexIndex[new Vector3(i + 1, 1, top)]);

        //back triangles
        triangles.Add(vertexIndex[new Vector3(i + 1, 0, bot)]);
        triangles.Add(vertexIndex[new Vector3(i, 1, bot)]);
        triangles.Add(vertexIndex[new Vector3(i, 0, bot)]);

        triangles.Add(vertexIndex[new Vector3(i + 1, 1, bot)]);
        triangles.Add(vertexIndex[new Vector3(i, 1, bot)]);
        triangles.Add(vertexIndex[new Vector3(i + 1, 0, bot)]);
    }

    Mesh GenerateTemporaryMeshForVFX()
    {
        Mesh temp = new Mesh();
        Vector3[] Vcube = {
        new Vector3 (0, -1000, 0),
        new Vector3 (1, -1000, 0),
        new Vector3 (1, -1001, 0),
        new Vector3 (0, -1001, 0),
        new Vector3 (0, -1001, 1),
        new Vector3 (1, -1001, 1),
        new Vector3 (1, -1000, 1),
        new Vector3 (0, -1000, 1),
    };
        temp.vertices = Vcube;

        //This assignement workaround the crash
        temp.triangles = new int[] {
        0, 2, 1,
        0, 3, 2,
        2, 3, 4,
        2, 4, 5,
        1, 2, 5,
        1, 5, 6,
        0, 7, 4,
        0, 4, 3,
        5, 4, 7,
        5, 7, 6,
        0, 6, 7,
        0, 1, 6
    };
        return temp;
    }

    //used to se if a point in the grid is buning,extinguished ect..
    class FireVertex
    {
        public enum VertexStatus
        {
            normal = 0,
            spreading = 1,
            burning = 2,
            extingushed =3
        }
        public Vector2 gridPos = new Vector2();

        public VertexStatus status = VertexStatus.normal;

        public FireVertex(Vector2 pos,VertexStatus vStatus = VertexStatus.normal)
        {
            gridPos = pos;
            status = vStatus;
        }
    }
}
