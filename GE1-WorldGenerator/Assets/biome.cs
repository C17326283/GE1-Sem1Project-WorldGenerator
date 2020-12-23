using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class biome : MonoBehaviour
{
    public Material biomeMaterial;
    //public float dist;
    public GameObject biomeObj;

    void Update()
    {
//        Debug.Log(biomeObj.transform);
        biomeMaterial.SetVector("_objPos",biomeObj.transform.position);
    }
}
