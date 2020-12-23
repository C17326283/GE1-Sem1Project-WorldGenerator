using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    //resolution for the amount of square that makes up a face, max 256
    [Range(2, 256)] 
    public int res = 100;

    
    //settings for editing shape and colour
    //public ShapeSettings shapeSettings;
    public PlanetSettings planetSettings;

    [HideInInspector] public bool colourSettingsFoldout;

    //Info about the generated shape
    private ColourGenerator colourGenerator = new ColourGenerator();
    
    //Info about faces
    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;
    
    public TerrainMinMaxHeights elevationMinMax;//for getting the highest and lowest points




    //Whenever anything is changed in editor
    private void OnValidate()
    {
        GeneratePlanet();
    }

    //If first time making shape then generate all the faces
    void Initialize()
    {
        colourGenerator.UpdateSettings(planetSettings);
        
        //make the 6 sides of the cube that will be spherized
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];
        
        
        //get all the directions to be used as the sides of the cube
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};
    
        //todo add water object
        //todo add atmoshereobject
        //todo add the spawners
        //create objects and components for all the faces
        for (int i = 0; i < 6; i++)
        {
            UpdateSettings(planetSettings);
            //todo try to split face further
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("PlanetFaceMesh");
                meshObj.transform.parent = transform;
                meshObj.transform.position = transform.position;
                meshObj.transform.tag = "Ground";//for letting the spawners hit it
            
                meshObj.AddComponent<MeshRenderer>();
                meshObj.AddComponent<MeshCollider>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
                
                
            }
            
            //int randBiome = Random.Range(0, planetSettings.biomeGradients.Length);//todo make this based on amount of biomes

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = planetSettings.planetMaterials[0];
            //add to list of faces
            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh,res,directions[i],elevationMinMax, planetSettings);
        }
    }

    //Generate whole planet
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();//turn this back on, its just annoying for editing
    }
    
    //Make mesh from all terrain faces
    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
        colourGenerator.UpdateHeightInShader(elevationMinMax);
    }

    void GenerateColours()
    {
        colourGenerator.UpdateTextureInShader();
    }
    
    public void UpdateSettings(PlanetSettings settings)
    {
        elevationMinMax = new TerrainMinMaxHeights();
    }
}