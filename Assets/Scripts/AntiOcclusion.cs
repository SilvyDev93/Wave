using UnityEngine;

public class AntiOcclusion : MonoBehaviour
{
    void Awake()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10000f);
    }
}
