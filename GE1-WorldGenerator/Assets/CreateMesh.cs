using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour
{
    public MeshFilter meshFilter;
    void Start()
    {
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
    }
}
