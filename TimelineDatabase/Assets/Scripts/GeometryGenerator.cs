using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GeometryGenerator : MonoBehaviour
{

    public MeshFilter mf;
    public MeshRenderer mr;
    public MeshCollider mc;
    public GameObject PointA;
    public GameObject PointB;
    private Vector3 positionA, positionB;
    public ChapterController cc;
    public float width = 1.0f;

    private void Start()
    {
        UpdatePositions();
        GenerateGeometry();
        mr.sharedMaterial = new Material(Shader.Find("Standard"));
    }
    private void UpdatePositions()
    {

        positionA = PointA.transform.localPosition;
        positionB = PointB.transform.localPosition;
        cc.eventPointsUpdate.Invoke(PointA.transform, PointB.transform);

    }
    public void GenerateGeometry()
    {
        
        

        Mesh mesh = new Mesh();
        float halfWidth = width * 0.5f;
        
        Vector3[] vertices = new Vector3[4]
        {
            //bottom left
            
            transform.InverseTransformPoint(PointA.transform.position-Vector3.right*halfWidth),
            //bottom right
            transform.InverseTransformPoint(PointA.transform.position)+Vector3.right*halfWidth,
            
            //top left
            transform.InverseTransformPoint(PointB.transform.position-Vector3.right*halfWidth),
            
            //top right
            transform.InverseTransformPoint(PointB.transform.position)+Vector3.right*halfWidth

        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        mf.mesh = mesh;
        mc.sharedMesh = null;
        mc.sharedMesh = mesh;
    }
    public void Update()
    {
        if (PointA.transform.localPosition!=positionA || PointB.transform.localPosition != positionB)
        {
            UpdatePositions();
            GenerateGeometry();
            
        }
        Debug.DrawLine(PointA.transform.position, PointB.transform.position);
    }

}

