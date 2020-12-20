﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    //resolution for the amount of square that makes up a face, max 256
    [Range(2, 256)] 
    public int res = 10;

    
    //settings for editing shape and colour
    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    [HideInInspector] public bool shapeSettingsFoldout;
    [HideInInspector] public bool colourSettingsFoldout;

    //Info about the generated shape
    private ShapeGenerator shapeGenerator;
    
    //Info about faces
    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;
    
    


    //Whenever anything is changed in editor
    private void OnValidate()
    {
        GeneratePlanet();
    }

    //If first time making shape then generate all the faces
    void Initialize()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);
        
        //make the 6 sides of the cube that will be spherized
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];
        
        
        //get all the directions to be used as the sides of the cube
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};
    
        //create objects and components for all the faces
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;
            
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            //add to list of faces
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh,res,directions[i]);
        }
    }

    //Generate whole planet
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }
    
    //In only shape settings have changed then can just update mesh
    public void OnShapeSettingsUpdated()
    {
        Initialize(); 
        GenerateMesh();
    }

    //If only the colour settings have changed then you can just update the colours
    public void OnColourSettingsUpdated()
    {
        Initialize(); 
        GenerateColours();
    }
    
    //Make mesh from all terrain faces
    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    void GenerateColours()
    {
        foreach (MeshFilter m in meshFilters)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.planetColour;
        }
    }
}
