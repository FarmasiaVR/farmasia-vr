using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpreadableFire : MonoBehaviour
{
    [SerializeField] float spreadingSpeed=1f, gridSquarewidth=1f;
    [SerializeField] int gridCols=4, gridRows=4;
    [SerializeField] BoxCollider collider;

    private float spreadTimer = 0;
    private HashSet<Vector2> spreadableVerticies = new HashSet<Vector2>();
    private HashSet<Vector2> ignitedVerticies = new HashSet<Vector2>();
    private HashSet<Vector2> noneSpreadableVerticies = new HashSet<Vector2>();
    private Dictionary<Vector2, FireVertex> fireVerticies = new Dictionary<Vector2, FireVertex>();
    public MeshFilter meshFilter;
    public VisualEffect fireEffect;
    Mesh mesh;
    private void OnValidate()
    {
        if (!collider)
            collider = GetComponent<BoxCollider>();
        UpdateBoxCollider();
    }
    private void OnEnable()
    {
        PopulateGrid();
        mesh = new Mesh();
        UpdateBoxCollider();

        //fireEffect.SetMesh("fireMesh", mesh);
        //fireEffect.SetVector3("firePos", transform.position);
    }

    private void Update()
    {
        Debug.Log("burning" + noneSpreadableVerticies.Count);
        if (ignitedVerticies.Count < 1)
            return;

        if(spreadTimer >= spreadingSpeed)
        {
            spreadTimer = 0;
            Propagate();
        }
        spreadTimer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IgniteClosestVertex(transform.InverseTransformPoint(collision.GetContact(0).point));
    }

    public List<Vector2> GetSurroundingVerticies(Vector2 point)
    {
        List<Vector2> surroundingPoints = new List<Vector2>();
        for (int i = -1; i < 2; i++)
            for (int j = -1; j < 2; j++)
                surroundingPoints.Add(point + new Vector2(i, j));

        return surroundingPoints;
    }

    private void IgniteClosestVertex(Vector3 point)
    {
        point = point/gridSquarewidth + new Vector3(gridRows / 2, 0, gridCols / 2);
        Vector2 flatenedPoint = new Vector2((int)point.x, (int)point.z);
        if (flatenedPoint.x > gridRows + 1)
            flatenedPoint.x = gridRows + 1;
        if (flatenedPoint.x < 0)
            flatenedPoint.x = 0;
        if (flatenedPoint.y > gridCols + 1)
            flatenedPoint.y = gridCols + 1;
        if (flatenedPoint.y < 0)
            flatenedPoint.y = 0;
        if (spreadableVerticies.Contains(flatenedPoint))
        {
            ignitedVerticies.Add(flatenedPoint);
            spreadableVerticies.Remove(flatenedPoint);
        }

    }
    private void Propagate()
    {
        Debug.Log("calculating propagation");
        List<Vector2> removeList = new List<Vector2>();
        HashSet<Vector2> addList = new HashSet<Vector2>();
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
        GenerateFireMesh();
    }
    void UpdateBoxCollider()
    {
        if (collider)
            collider.size = new Vector3(gridSquarewidth * gridRows, collider.size.y, gridSquarewidth * gridCols);
    }
    private void GenerateFireMesh()
    {
        Debug.Log("generating mesh");
        Dictionary<Vector2, int> vertexIndex = new Dictionary<Vector2, int>();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
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

                    if(!vertexIndex.ContainsKey(new Vector2(i, top)))
                    {
                        vertexIndex.Add(new Vector2(i, top),vertices.Count);
                        vertices.Add(new Vector3((i - (gridRows / 2))*gridSquarewidth, 0, (top - (gridCols / 2)) * gridSquarewidth));
                    }
                    if (!vertexIndex.ContainsKey(new Vector2(i + 1, top)))
                    {
                        vertexIndex.Add(new Vector2(i + 1, top), vertices.Count);
                        vertices.Add(new Vector3((i + 1 - (gridRows / 2)) * gridSquarewidth, 0, (top - (gridCols / 2)) * gridSquarewidth));
                    }
                    if (!vertexIndex.ContainsKey(new Vector2(i, bot)))
                    {
                        vertexIndex.Add(new Vector2(i, bot), vertices.Count);
                        vertices.Add(new Vector3((i - (gridRows / 2)) * gridSquarewidth, 0, (bot - (gridCols / 2)) * gridSquarewidth));
                    }
                    if (!vertexIndex.ContainsKey(new Vector2(i + 1, bot)))
                    {
                        vertexIndex.Add(new Vector2(i + 1, bot), vertices.Count);
                        vertices.Add(new Vector3((i + 1 - (gridRows / 2)) * gridSquarewidth, 0, (bot - (gridCols / 2)) * gridSquarewidth));
                    }

                    triangles.Add(vertexIndex[new Vector2(i, top)]);
                    triangles.Add(vertexIndex[new Vector2(i+1, top)]);
                    triangles.Add(vertexIndex[new Vector2(i, bot)]);

                    triangles.Add(vertexIndex[new Vector2(i+1, top)]);
                    triangles.Add(vertexIndex[new Vector2(i + 1, bot)]);
                    triangles.Add(vertexIndex[new Vector2(i, bot)]);

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
        meshFilter.mesh = mesh;
        //fireEffect.SetMesh("fireMesh", mesh);
        //fireEffect.SetVector3("firePos", transform.position);
    }
    bool CheckIfBuring(Vector2 v1,Vector2 v2)
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

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < gridRows + 1; i++)
        {
            for (int j = 0; j < gridCols + 1; j++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.TransformPoint(new Vector3((i-(gridRows/2))* gridSquarewidth, 0, (j-(gridCols/2))*gridSquarewidth)), 0.05f);
            }
        }
    }

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

        public bool IsIgnitable()
        {
            if (status == VertexStatus.normal)
                return true;
            return false;
        }
        public FireVertex(Vector2 pos,VertexStatus vStatus = VertexStatus.normal)
        {
            gridPos = pos;
            status = vStatus;
        }
    }
}
