using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class RoadMeshCreator : MonoBehaviour
{
    [Header("Road settings")]
    public float roadWidth = 1;
    [Range(0, .5f)]
    public float thickness = .15f;
    public bool flattenSurface;

    [Header("Material settings")]
    public Material roadMaterial;
    public Material undersideMaterial;
    public float textureTiling = 1;

    [SerializeField, HideInInspector]
    GameObject meshHolder;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;

    MeshCollider meshCollider;
    Vector2[] points;
    public VertexPath vp;

    private void PathUpdated()
    {
        AssignMeshComponents();
        AssignMaterials();
        CreateRoadMesh();
    }

    void Start()
    {
        //meshHolder = this.GetComponentInChildren<Mesh>();
        // get the car_root_with_wings object from the resources folder
        // car = Resources.Load("Prefabs/prefab_car_wings") as GameObject;

        // The position of the first point on the path
        //Vector3 position = pathCreator.bezierPath.GetPoint(0);

        // raise the position in the y axis by 0.1 so the car is not in the ground
        //position.y += 0.1f;

        //GameObject.Find("Car").transform.position = position;


        // Instantiate the car in the direction of the track
        //Instantiate(car, position, Quaternion.LookRotation(pathCreator.path.GetDirection(0)));

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            points = new Vector2[] { new Vector2(0, 0), new Vector2(0, 100), new Vector2(100, 100), new Vector2(100, 0) };
            // Create a closed, 2D bezier path from the supplied points array
            // These points are treated as anchors, which the path will pass through
            // The control points for the path will be generated automatically
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BezierPath bezierPath = new BezierPath(points, true, PathSpace.xy);
            // Then create a vertex path from the bezier path, to be used for movement etc
            vp = new VertexPath(bezierPath, transform);
            PathUpdated();
            GetComponent<TrackCheckPoints>().Generate();
        }
    }

    void CreateRoadMesh()
    {
        Vector3[] verts = new Vector3[vp.NumPoints * 8];
        Vector2[] uvs = new Vector2[verts.Length];
        Vector3[] normals = new Vector3[verts.Length];

        int numTris = 2 * (vp.NumPoints - 1) + ((vp.isClosedLoop) ? 2 : 0);
        int[] roadTriangles = new int[numTris * 3];
        int[] underRoadTriangles = new int[numTris * 3];
        int[] sideOfRoadTriangles = new int[numTris * 2 * 3];

        int vertIndex = 0;
        int triIndex = 0;

        // Vertices for the top of the road are layed out:
        // 0  1
        // 8  9
        // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
        int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
        int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };

        bool usePathNormals = !(vp.space == PathSpace.xyz && flattenSurface);

        for (int i = 0; i < vp.NumPoints; i++)
        {
            Vector3 localUp = (usePathNormals) ? Vector3.Cross(vp.GetTangent(i), vp.GetNormal(i)) : vp.up;
            Vector3 localRight = (usePathNormals) ? vp.GetNormal(i) : Vector3.Cross(localUp, vp.GetTangent(i));

            // Find position to left and right of current path vertex
            Vector3 vertSideA = vp.GetPoint(i) - localRight * Mathf.Abs(roadWidth);
            Vector3 vertSideB = vp.GetPoint(i) + localRight * Mathf.Abs(roadWidth);

            // Add top of road vertices
            verts[vertIndex + 0] = vertSideA;
            verts[vertIndex + 1] = vertSideB;
            // Add bottom of road vertices
            verts[vertIndex + 2] = vertSideA - localUp * thickness;
            verts[vertIndex + 3] = vertSideB - localUp * thickness;

            // Duplicate vertices to get flat shading for sides of road
            verts[vertIndex + 4] = verts[vertIndex + 0];
            verts[vertIndex + 5] = verts[vertIndex + 1];
            verts[vertIndex + 6] = verts[vertIndex + 2];
            verts[vertIndex + 7] = verts[vertIndex + 3];

            // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
            uvs[vertIndex + 0] = new Vector2(0, vp.times[i]);
            uvs[vertIndex + 1] = new Vector2(1, vp.times[i]);

            // Top of road normals
            normals[vertIndex + 0] = localUp;
            normals[vertIndex + 1] = localUp;
            // Bottom of road normals
            normals[vertIndex + 2] = -localUp;
            normals[vertIndex + 3] = -localUp;
            // Sides of road normals
            normals[vertIndex + 4] = -localRight;
            normals[vertIndex + 5] = localRight;
            normals[vertIndex + 6] = -localRight;
            normals[vertIndex + 7] = localRight;

            // Set triangle indices
            if (i < vp.NumPoints - 1 || vp.isClosedLoop)
            {
                for (int j = 0; j < triangleMap.Length; j++)
                {
                    roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                    // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                    underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
                }
                for (int j = 0; j < sidesTriangleMap.Length; j++)
                {
                    sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
                }

            }

            vertIndex += 8;
            triIndex += 6;
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.subMeshCount = 3;
        mesh.SetTriangles(roadTriangles, 0);
        mesh.SetTriangles(underRoadTriangles, 1);
        mesh.SetTriangles(sideOfRoadTriangles, 2);
        mesh.RecalculateBounds();
    }

    // Add MeshRenderer and MeshFilter components to this gameobject if not already attached
    void AssignMeshComponents()
    {

        if (meshHolder == null)
        {
            meshHolder = new GameObject("Road Mesh Holder");
            meshHolder.transform.parent = this.transform;
        }

        meshHolder.transform.rotation = Quaternion.identity;
        meshHolder.transform.position = Vector3.zero;
        meshHolder.transform.localScale = Vector3.one;

        // Ensure mesh renderer and filter components are assigned
        if (!meshHolder.gameObject.GetComponent<MeshFilter>())
        {
            meshFilter = meshHolder.gameObject.AddComponent<MeshFilter>();
        }
        if (!meshHolder.GetComponent<MeshRenderer>())
        {
            meshRenderer = meshHolder.gameObject.AddComponent<MeshRenderer>();
        }
        if (!meshHolder.GetComponent<MeshCollider>())
        {
            meshCollider = meshHolder.gameObject.AddComponent<MeshCollider>();
        }
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;   
    }

    void AssignMaterials()
    {
        if (roadMaterial != null && undersideMaterial != null)
        {
            meshRenderer.sharedMaterials = new Material[] { roadMaterial, undersideMaterial, undersideMaterial };
            meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3(1, textureTiling);
        }
    }

}