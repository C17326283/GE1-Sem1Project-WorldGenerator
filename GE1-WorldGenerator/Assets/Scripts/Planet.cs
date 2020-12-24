using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    //resolution for the amount of square that makes up a face, max 256
    [Range(20, 256)] 
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
    [SerializeField, HideInInspector]
    private MeshFilter[] waterMeshFilters;
    private TerrainFace[] terrainFaces;
    private TerrainFace[] waterFaces;//I definitely could have made the water faces in abetter way but i just generated terrain again without noise
    
    
    public TerrainMinMaxHeights elevationMinMax;//for getting the highest and lowest points
    public GameObject[] biomeObjs;//4




    //Whenever anything is changed in editor
    private void OnValidate()
    {
        //GeneratePlanet();
    }

    //If first time making shape then generate all the faces
    void Create()
    {
        colourGenerator.UpdateSettings(planetSettings);
        
        //make the 6 sides of the cube that will be spherized
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
            waterMeshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];
        waterFaces = new TerrainFace[6];
        
        
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
                GameObject waterObj = new GameObject("WaterMesh");
                meshObj.transform.parent = transform;
                waterObj.transform.parent = transform;
                meshObj.transform.position = transform.position;
                waterObj.transform.position = transform.position;
                meshObj.transform.tag = "Ground";//for letting the spawners hit it
                waterObj.transform.tag = "Water";
            
                meshObj.AddComponent<MeshRenderer>();
                waterObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                waterMeshFilters[i] = waterObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
                waterMeshFilters[i].sharedMesh = new Mesh();
                meshFilters[i].sharedMesh.name = "sharedMesh";
                waterMeshFilters[i].sharedMesh.name = "sharedWaterMesh";
            }

            SetBiomes();
            
            //int randBiome = Random.Range(0, planetSettings.biomeGradients.Length);//todo make this based on amount of biomes

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = planetSettings.planetMaterial;
            waterMeshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = planetSettings.waterMaterial;
            //add to list of faces
            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh,res,directions[i],elevationMinMax, planetSettings,true);
            waterFaces[i] = new TerrainFace(waterMeshFilters[i].sharedMesh,res,directions[i],elevationMinMax, planetSettings,false);
            
            DestroyImmediate(meshFilters[i].transform.gameObject.GetComponent<MeshCollider>());
            var collider = meshFilters[i].transform.gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = meshFilters[i].sharedMesh;
            
            DestroyImmediate(waterMeshFilters[i].transform.gameObject.GetComponent<MeshCollider>());
            var col2 = waterMeshFilters[i].transform.gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = waterMeshFilters[i].sharedMesh;
            //meshFilters[i].gameObject.AddComponent<MeshCollider>();//add mesh collider after changing terrain
        }
    }

    //Generate whole planet
    public void GeneratePlanet()
    {
        Create();
        GenerateMesh();
        GenerateColours();//turn this back on, its just annoying for editing
    }
    
    //Make mesh from all terrain faces
    void GenerateMesh()
    {
        Debug.Log("gen");
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
        foreach (TerrainFace face in waterFaces)
        {
            face.ConstructMesh();
        }
        colourGenerator.UpdateHeightInShader(elevationMinMax);
    }

    void GenerateColours()
    {
        colourGenerator.UpdateTextureInShader(biomeObjs[0],biomeObjs[1],biomeObjs[2],biomeObjs[3]);
    }
    
    public void UpdateSettings(PlanetSettings settings)
    {
        elevationMinMax = new TerrainMinMaxHeights();
    }

    void SetBiomes()
    {
        if (biomeObjs == null)
        {
            biomeObjs =  new GameObject[4];
            for (int j = 0; j < 4; j++)
            {
                biomeObjs[j] = new GameObject("BiomePlacementObject");
                biomeObjs[j].transform.parent = this.transform;
                if (planetSettings.havePoles && j<2)
                {
                    if (j == 0)
                    {
                        biomeObjs[j].transform.position = transform.up * planetSettings.planetRadius;
                    }
                    else if(j==1)
                    {
                        biomeObjs[j].transform.position = -transform.up * planetSettings.planetRadius;
                    }
                }
                else
                {
                    biomeObjs[j].transform.position = Random.onUnitSphere * planetSettings.planetRadius;
                }
            }
        }
    }
}