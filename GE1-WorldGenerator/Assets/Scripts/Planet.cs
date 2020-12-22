using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    //resolution for the amount of square that makes up a face, max 256
    [Range(2, 256)] 
    public int res = 10;

    
    //settings for editing shape and colour
    //public ShapeSettings shapeSettings;
    public PlanetSettings planetSettings;

    [HideInInspector] public bool colourSettingsFoldout;

    //Info about the generated shape
    private ColourGenerator colourGenerator = new ColourGenerator();
    
    //Info about faces 
    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;//all the mesh objects
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
            meshFilters = new MeshFilter[24];
        }
        terrainFaces = new TerrainFace[6];
        
        
        //get all the directions to be used as the sides of the cube
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};

        int k = 0;
        //create objects and components for all the faces
        for (int i = 0; i < 6; i++)
        {
            UpdateSettings(planetSettings);
            //todo try to split face further
            for (int j = 0; j < 4; j++)
            {
                print(k+""+j);
                if (meshFilters[k+j] == null)
                {
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;
                    meshObj.transform.position = transform.position;
            
                    meshObj.AddComponent<MeshRenderer>();
                    meshFilters[k+j] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[k+j].sharedMesh = new Mesh();
                }
            
                int randBiome = Random.Range(0, planetSettings.biomeGradients.Length);//todo make this based on amount of biomes

                meshFilters[k+j].GetComponent<MeshRenderer>().sharedMaterial = planetSettings.planetMaterials[randBiome];
            }
            
            //add to list of faces
            terrainFaces[i] = new TerrainFace(meshFilters[k+0].sharedMesh,meshFilters[k+1].sharedMesh,meshFilters[k+2].sharedMesh, meshFilters[k+3].sharedMesh,res, directions[i],elevationMinMax, planetSettings);
            k+=4;
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
