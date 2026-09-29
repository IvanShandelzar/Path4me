using UnityEngine;
using Unity.AI.Navigation;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ArenaGenerator : MonoBehaviour
{
    [Range(3, 64)] public int sides = 12;         // количество граней
    public float radius = 10f;                     // радиус
    public float wallHeight = 4f;                  // высота стен
    public float floorThickness = 0.5f;            // толщина пола
    public bool generateFloor = true;
    public bool generateWalls = true;
    public bool generateCeiling = false;

    [Header("NavMesh")]
    [SerializeField] private NavMeshSurface navMeshSurface;

    void OnValidate()
    {
        Generate();
        RebuildNavMesh();
    }

    void Start()
    {
        Generate();
        RebuildNavMesh();
    }

    [ContextMenu("Regenerate")]
    public void Generate()
    {
        Mesh mesh = new Mesh { name = "ArenaMesh" };

        var verts = new System.Collections.Generic.List<Vector3>();
        var tris  = new System.Collections.Generic.List<int>();

        float angleStep = Mathf.PI * 2f / sides;

        // ---------- ПОЛ ----------
        if (generateFloor)
        {
            int centerTop = verts.Count;
            verts.Add(Vector3.zero);
            int ringTopStart = verts.Count;
            for (int i = 0; i < sides; i++)
            {
                float a = i * angleStep;
                verts.Add(new Vector3(Mathf.Cos(a) * radius, 0, Mathf.Sin(a) * radius));
            }
            for (int i = 0; i < sides; i++)
            {
                int next = (i + 1) % sides;
                tris.Add(centerTop);
                tris.Add(ringTopStart + next);
                tris.Add(ringTopStart + i);
            }
        }

        // ---------- СТЕНЫ ----------
        if (generateWalls)
        {
            int wallBase = verts.Count;
            for (int i = 0; i < sides; i++)
            {
                float a = i * angleStep;
                float x = Mathf.Cos(a) * radius;
                float z = Mathf.Sin(a) * radius;
                verts.Add(new Vector3(x, 0, z));
                verts.Add(new Vector3(x, wallHeight, z));
            }
            for (int i = 0; i < sides; i++)
            {
                int next = (i + 1) % sides;
                int b0 = wallBase + i * 2;
                int t0 = b0 + 1;
                int b1 = wallBase + next * 2;
                int t1 = b1 + 1;

                tris.Add(b0); tris.Add(t1); tris.Add(t0);
                tris.Add(b0); tris.Add(b1); tris.Add(t1);
            }
        }

        // ---------- ПОТОЛОК ----------
        if (generateCeiling)
        {
            int centerTop = verts.Count;
            verts.Add(new Vector3(0, wallHeight, 0));
            int ringStart = verts.Count;
            for (int i = 0; i < sides; i++)
            {
                float a = i * angleStep;
                verts.Add(new Vector3(Mathf.Cos(a) * radius, wallHeight, Mathf.Sin(a) * radius));
            }
            for (int i = 0; i < sides; i++)
            {
                int next = (i + 1) % sides;
                tris.Add(centerTop);
                tris.Add(ringStart + i);
                tris.Add(ringStart + next);
            }
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void RebuildNavMesh()
    {
        if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh();
    }
}