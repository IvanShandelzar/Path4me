using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Arch : MonoBehaviour
{
    public enum Style { Rounded, Pointed, Parabolic }
    public Style style = Style.Rounded;

    public float width  = 3f;     // ширина проёма
    public float height = 3f;     // высота арки
    public float band   = 0.3f;   // толщина дуги
    public float depth  = 0.4f;   // глубина
    [Range(4, 64)] public int segments = 24;

    [ContextMenu("Generate")]
    public void Generate()
    {
        float hw = width * 0.5f;
        float ohw = hw + band;
        int N = segments + 1;

        var verts = new List<Vector3>();
        var tris  = new List<int>();

        for (int i = 0; i < N; i++)
        {
            float u = i / (float)(N - 1) * 2f - 1f;   // -1..1
            float yi = Curve(u) * height;
            float yo = Curve(u) * (height + band);
            float z = depth * 0.5f;
            verts.Add(new Vector3(u * hw,  yi, z));   // front inner
            verts.Add(new Vector3(u * ohw, yo, z));   // front outer
        }
        int backStart = verts.Count;
        for (int i = 0; i < N; i++)
        {
            var fi = verts[i * 2];
            var fo = verts[i * 2 + 1];
            verts.Add(new Vector3(fi.x, fi.y, -fi.z));
            verts.Add(new Vector3(fo.x, fo.y, -fo.z));
        }

        for (int i = 0; i < N - 1; i++)
        {
            int fi0 = i * 2,       fo0 = i * 2 + 1;
            int fi1 = (i+1) * 2,   fo1 = (i+1) * 2 + 1;
            int bi0 = backStart + i * 2,       bo0 = backStart + i * 2 + 1;
            int bi1 = backStart + (i+1) * 2,   bo1 = backStart + (i+1) * 2 + 1;

            // перед
            tris.Add(fi0); tris.Add(fo1); tris.Add(fo0);
            tris.Add(fi0); tris.Add(fi1); tris.Add(fo1);
            // зад
            tris.Add(bi0); tris.Add(bo0); tris.Add(bo1);
            tris.Add(bi0); tris.Add(bo1); tris.Add(bi1);
            // внешняя кромка
            tris.Add(fo0); tris.Add(fo1); tris.Add(bo1);
            tris.Add(fo0); tris.Add(bo1); tris.Add(bo0);
            // внутренняя кромка
            tris.Add(fi0); tris.Add(bi0); tris.Add(bi1);
            tris.Add(fi0); tris.Add(bi1); tris.Add(fi1);
        }

        var mesh = new Mesh { name = "ArchMesh" };
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    float Curve(float u)
    {
        u = Mathf.Clamp(u, -1f, 1f);
        switch (style)
        {
            case Style.Rounded:   return Mathf.Sqrt(1f - u * u);
            case Style.Pointed:   return 1f - Mathf.Abs(u);
            case Style.Parabolic: return 1f - u * u;
        }
        return 0f;
    }
}