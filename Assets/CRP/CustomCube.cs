using UnityEngine;

public class CustomCube : MonoBehaviour
{
    public Material material;
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material = material;

        Vector3[] vertices = {
            // Front face
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),

            // Back face
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),

            // Left face
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),

            // Right face
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),

            // Top face
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),

            // Bottom face
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f)
        };

        int[] triangles = {
            // Front face
            0, 2, 1,
            0, 3, 2,

            // Back face
            4, 6, 5,
            4, 7, 6,

            // Left face
            8, 10, 9,
            8, 11, 10,

            // Right face
            12, 14, 13,
            12, 15, 14,

            // Top face
            16, 18, 17,
            16, 19, 18,

            // Bottom face
            20, 22, 21,
            20, 23, 22
        };

        Vector2[] uvs = {
            // Front face
            new Vector2(0.25f, 0),
            new Vector2(0.5f, 0),
            new Vector2(0.5f, 1),
            new Vector2(0.25f, 1),

            // Back face
            new Vector2(0.75f, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0.75f, 1),

            // Left face
            new Vector2(0, 0), //Bottom left
            new Vector2(0.25f, 0), //Bottom right
            new Vector2(0.25f, 1),  //Top right
            new Vector2(0, 1),  //Top left

            // Right face
            new Vector2(0.5f, 0),
            new Vector2(0.75f, 0),
            new Vector2(0.75f, 1),
            new Vector2(0.5f, 1),

            // Top face
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),

            // Bottom face
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
    }
}
