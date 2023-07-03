using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BF_AddSnow : MonoBehaviour
{
    public Material snowMat;
    public bool isAuto = false;
    public float angle = 80f;

    private Mesh originalMesh;
    private MeshFilter meshFilter;
    private Mesh newMesh;
    private GameObject newGO;

    private int[] oldTri;
    private Vector3[] oldVert;
    private Vector3[] oldNorm;
    private Vector3[] oldNormWorld;
    private Vector2[] oldUV;
    private Color[] oldCol;

    private List<int> triangles = new List<int>();
    private List<Vector3> vertexs = new List<Vector3>();
    private List<Vector3> norms = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<Color> cols = new List<Color>();


    void Start()
    {
        CheckValues();
        BuildGeometry();
    }


    private void CheckValues()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        originalMesh = meshFilter.mesh;
        oldTri = originalMesh.triangles;
        oldVert = originalMesh.vertices;
        oldNorm = originalMesh.normals;
        oldNormWorld = oldNorm;

        if (isAuto)
        {
            int k = 0;
            foreach (Vector3 norm in oldNorm)
            {
                oldNormWorld[k] = this.transform.localToWorldMatrix.MultiplyVector(norm).normalized;
                k++;
            }
            oldCol = new Color[oldVert.Length];
        }
        else
        {
            oldCol = originalMesh.colors;
        }
        oldUV = originalMesh.uv;
    }

    private void ClearGeometry()
    {
        triangles.Clear();
        triangles.TrimExcess();
        vertexs.Clear();
        vertexs.TrimExcess();
        uvs.Clear();
        uvs.TrimExcess();
        cols.Clear();
        cols.TrimExcess();
    }

    private void BuildGeometry()
    {
        if (meshFilter == null)
        {
            meshFilter = gameObject.GetComponent<MeshFilter>();
        }
        newMesh = new Mesh();
        newGO = new GameObject();
        MeshFilter mF = newGO.AddComponent<MeshFilter>();
        MeshRenderer mR = newGO.AddComponent<MeshRenderer>();
        mF.mesh = newMesh;
        mR.material = snowMat;
        snowMat.SetFloat("_ISADD", 1);
        snowMat.EnableKeyword("IS_ADD");
        newGO.transform.parent = this.transform;
        newGO.transform.localPosition = Vector3.zero;
        newGO.transform.localScale = Vector3.one;
        newGO.transform.localRotation = Quaternion.identity;
        //meshFilter.mesh = mesh;

        int indexNewV = 0;
        foreach (Vector3 v in oldVert)
        {
            //vertexs.Add(v + (oldNorm[indexNewV]));
            vertexs.Add(v + new Vector3(0,0,0));
            uvs.Add(oldUV[indexNewV]);
            //cols.Add(new Color(1 * ((float)i / (float)(faces - 1)), 1 * ((float)i / (float)(faces - 1)), 1 * ((float)i / (float)(faces - 1))));

            indexNewV++;
        }
        indexNewV = 0;
        foreach (int innt in oldTri)
        {
            triangles.Add(oldTri[indexNewV]);

            indexNewV++;
        }

        if (isAuto)
        {
            int j = 0;
            foreach (Vector3 norm in oldNormWorld)
            {
                if(j>= oldCol.Length)
                {
                    break;
                }
                oldCol[j] = Color.red;
                float theAngle = Vector3.Angle(Vector3.up, norm);
                if (theAngle < (angle+10f))
                {
                    oldCol[j] = Color.Lerp(Color.white, Color.red, Mathf.Max(0f,theAngle- angle/2f) / (angle / 2f));
                }
                j++;
            }
        }

        cols = oldCol.ToList();
        //norms = oldNormWorld.ToList();
        newMesh.vertices = vertexs.ToArray();
        newMesh.triangles = triangles.ToArray();
        newMesh.uv = uvs.ToArray();
        newMesh.colors = cols.ToArray();
        newMesh.normals = originalMesh.normals;
        //newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();
        newMesh.Optimize();
    }
}
