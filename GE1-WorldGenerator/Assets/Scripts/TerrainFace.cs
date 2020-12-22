﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace : MonoBehaviour
{
    //private ShapeGenerator shapeGenerator;
    private Mesh mesh;

    private int res;//resolution of mesh
    public int biome;

    private Vector3 localUp;//the way its facing
    private Vector3 axisA;
    private Vector3 axisB;
    
    private PlanetSettings settings;
    private NoiseLayer[] noiseLayers;
    private TerrainMinMaxHeights elevationMinMax;



    //constructor for initalising the terrain face parameters
    public TerrainFace(Mesh mesh, int res, Vector3 localUp, int biome, TerrainMinMaxHeights elevationMinMax,PlanetSettings planetSettings)
    {
        //this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.res = res;
        this.localUp = localUp;
        this.biome = biome;
        this.elevationMinMax = elevationMinMax;
        this.settings = planetSettings;
        this.noiseLayers = planetSettings.noiseLayers;//copy all noise layers from planetSettings
        
        
        axisA = new Vector3(localUp.y, localUp.z,localUp.x);//swap coords to get axis along face
        axisB = Vector3.Cross(localUp, axisA);//use cross product to get angle perpendicular to terrain and axisa
    }

    //Make the actual mesh of the face with verticies and triangles
    public void ConstructMesh()
    {
        int r = 4;
        
        //make an array for verticies and triangles based on the resolution
        Vector3[] vertices = new Vector3[res*res];
        Vector3[] vertices2 = new Vector3[r*r];
        int[] triangles = new int[(res-1)*(res-1)*6];//res-1^2 is num of faces * by 2 triangles per square * verticies per triangle
        int[] triangles2 = new int[(r - 1) * (r - 1) * 6];
        int triIndex = 0;//Index for each individual point
        int triIndex2 = 0;//Index for each individual point

        //int i = 0;
        //edit each vertex to the right position on sphere
        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                int i = x + y * res;
                Vector2 percent = new Vector2(x,y) / (res-1);//percentage of width for even spacing
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f)*2*axisA + (percent.y - .5f)*2*axisB;//get position of individual point on grid
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;//normalise it to get where it should be on sphere
                
                
                //use the spherized point with noise to find where it should be
                vertices[i] = AddNoiseToVertex(pointOnUnitSphere);
                if(x != r-1 &&  y != r-1 && i <= r)
                    vertices2[i] = vertices[i];
                
                //understood verticies using this vid at this time https://youtu.be/QN39W020LqU?t=439
                //set the points to make triangles from vertexes//just based on way the points are indexed
                if(x != res-1 &&  y != res-1)//as long as not on the right or bottom edge
                {
                    //first triangle of square points. on a 4x4 grid this would be 0,5,4 because of clockwise allocation
                    triangles[triIndex2] = i;
                    triangles[triIndex2+1] = i+res+1;
                    triangles[triIndex2+2] = i+res;
                    
                    //second triangle of square points. 0,1,5
                    triangles[triIndex2+3] = i;
                    triangles[triIndex2+4] = i+1;
                    triangles[triIndex2+5] = i+res+1;

                    if (x<r-1 && y<r-1)
                    {
                        int k = x + y * r;
                        Debug.Log(triIndex2+"    "+k+"    "+triangles2.Length);
                        triangles2[triIndex] = k;//first vertex
                        triangles2[triIndex + 1] = k + res + 1;//second vertex of the first triangle
                        triangles2[triIndex + 2] = k + res;

                        triangles2[triIndex + 3] = k;
                        triangles2[triIndex + 4] = k + 1;
                        triangles2[triIndex + 5] = k + res + 1;

                        triIndex2 += 6;
                    }

                    triIndex += 6;
                }
                /*

                if(x != meshHalf-1 &&  y != meshHalf-1 && i < meshHalf)
                {
                    Debug.Log(vertices2.Length+" "+i);
                    vertices2[i] = vertices[i];
                    

                    if (x != meshHalf - 2 && y != meshHalf - 2) //as long as not on the right or bottom edge
                    {
                        Debug.Log(i);
                        triangles2[triIndex] = i;
                        triangles2[triIndex+1] = i+res+1;
                        triangles2[triIndex+2] = i+res;
                    
                        triangles2[triIndex+3] = i;
                        triangles2[triIndex+4] = i+1;
                        triangles2[triIndex+5] = i+res+1;

                    }
                }
                */
            }
        }
        mesh.Clear();//clear data from mesh for when recalculating resolution
        
        mesh.vertices = vertices2;
        mesh.triangles = triangles2;
        
        mesh.RecalculateNormals();
    }
    
    public Vector3 AddNoiseToVertex(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        //float elevation = noiseFilter.Evaluate(pointOnUnitSphere);
        float elevation = 0;
        
        //Use the previous layers as a mask so spikes go on top of other mountains not randomly
        if (noiseLayers.Length > 0)
        {
            firstLayerValue = noiseLayers[0].AddNoise(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }
            
            
        for (int i = 1; i < noiseLayers.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;//? if true do this else set default of 1
                elevation += noiseLayers[i].AddNoise(pointOnUnitSphere) * mask;
            }
        }

        elevation = settings.planetRadius * (1 + elevation);
        //Debug.Log(elevation);
        //check if is exact radius, if so then lower it to allow for the water mesh to be fitted properly
        if (elevation < settings.planetRadius+0.3f)
        {
            //dont evaluated point so the colours dont rely on water level
            elevation = (settings.planetRadius * 1)-1f;
            return pointOnUnitSphere * elevation;
        }
        else
        {
            elevationMinMax.AddValue(elevation);
            return pointOnUnitSphere * elevation;
        }

    }
}
