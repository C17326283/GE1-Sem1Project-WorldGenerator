using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace : MonoBehaviour
{
    //private ShapeGenerator shapeGenerator;
    private Mesh mesh1;
    private Mesh mesh2;
    private Mesh mesh3;
    private Mesh mesh4;

    private int res;//resolution of mesh
    public int biome;

    private Vector3 localUp;//the way its facing
    private Vector3 axisA;
    private Vector3 axisB;
    
    private PlanetSettings settings;
    private NoiseLayer[] noiseLayers;
    private GameObject[] faceQuads;
    private TerrainMinMaxHeights elevationMinMax;



    //constructor for initalising the terrain face parameters
    public TerrainFace(Mesh mesh1,Mesh mesh2,Mesh mesh3,Mesh mesh4, int res, Vector3 localUp, TerrainMinMaxHeights elevationMinMax,PlanetSettings planetSettings)
    {
        //this.shapeGenerator = shapeGenerator;
        this.mesh1 = mesh1;
        this.mesh2 = mesh2;
        this.mesh3 = mesh3;
        this.mesh4 = mesh4;
        this.res = res;
        this.localUp = localUp;
        this.elevationMinMax = elevationMinMax;
        this.settings = planetSettings;
        this.noiseLayers = planetSettings.noiseLayers;//copy all noise layers from planetSettings
        
        
        axisA = new Vector3(localUp.y, localUp.z,localUp.x);//swap coords to get axis along face
        axisB = Vector3.Cross(localUp, axisA);//use cross product to get angle perpendicular to terrain and axisa
    }

    //Make the actual mesh of the face with verticies and triangles
    public void ConstructMesh()
    {
        int r = (res/2)+1;
        
        //make an array for verticies and triangles based on the resolution
        Vector3[] allVertices = new Vector3[res*res];
        //GameObject[] faceQuads = new GameObject[4];//intialize array
        Vector3[] vertices1 = new Vector3[r*r];
        Vector3[] vertices2 = new Vector3[r*r];
        Vector3[] vertices3 = new Vector3[r*r];
        Vector3[] vertices4 = new Vector3[r*r];
        
        
        int[] allTriangles = new int[(res-1)*(res-1)*6];//res-1^2 is num of faces * by 2 triangles per square * verticies per triangle
        int[] triangles1 = new int[(r - 1) * (r - 1) * 6];
        int[] triangles2 = new int[(r - 1) * (r - 1) * 6];
        int[] triangles3 = new int[(r - 1) * (r - 1) * 6];
        int[] triangles4 = new int[(r - 1) * (r - 1) * 6];
        int allTriIndex = 0;//Index for each individual point
        int triIndex1 = 0;
        int triIndex2 = 0;
        int triIndex3 = 0;
        int triIndex4 = 0;

        int i = 0;
        int k1 = 0;
        int k2 = 0;
        int k3 = 0;
        int k4 = 0;
        //edit each vertex to the right position on sphere
        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                Vector2 percent = new Vector2(x,y) / (res-1);//percentage of width for even spacing
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f)*2*axisA + (percent.y - .5f)*2*axisB;//get position of individual point on grid
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;//normalise it to get where it should be on sphere
                
                
                //use the spherized point with noise to find where it should be
                allVertices[i] = AddNoiseToVertex(pointOnUnitSphere);
                
                //understood verticies using this vid at this time https://youtu.be/QN39W020LqU?t=439
                //set the points to make triangles from vertexes//just based on way the points are indexed
                if(x != res-1 &&  y != res-1)//as long as not on the right or bottom edge
                {
                    //first triangle of square points. on a 4x4 grid this would be 0,5,4 because of clockwise allocation
                    allTriangles[allTriIndex] = i;
                    allTriangles[allTriIndex+1] = i+res+1;
                    allTriangles[allTriIndex+2] = i+res;
                    
                    //second triangle of square points. 0,1,5
                    allTriangles[allTriIndex+3] = i;
                    allTriangles[allTriIndex+4] = i+1;
                    allTriangles[allTriIndex+5] = i+res+1;
                    
                    allTriIndex += 6;

                    if (x<r-1 && y<r-1)//top left quad of shape
                    {
                        vertices1[k1] = allVertices[i];
                        triangles1[triIndex1] = k1;//first vertex
                        triangles1[triIndex1 + 1] = k1 + r + 1;//second vertex of the first triangle
                        triangles1[triIndex1 + 2] = k1 + r;

                        triangles1[triIndex1 + 3] = k1;
                        triangles1[triIndex1 + 4] = k1 + 1;
                        triangles1[triIndex1 + 5] = k1 + r + 1;
                        triIndex1 += 6;
                        k1++;
                    }
                    else if (x>r-1 && y<r-1)//top right
                    {
                        vertices2[k2] = allVertices[i];
                        triangles2[triIndex2] = k2;//first vertex
                        triangles2[triIndex2 + 1] = k2 + r + 1;//second vertex of the first triangle
                        triangles2[triIndex2 + 2] = k2 + r;

                        triangles2[triIndex2 + 3] = k2;
                        triangles2[triIndex2 + 4] = k2 + 1;
                        triangles2[triIndex2 + 5] = k2 + r + 1;

                        triIndex2 += 6;
                        k2++;
                    }
                    else if (x<r-1 && y>r-1)//bottom left
                    {
                        vertices3[k3] = allVertices[i];
                        triangles3[triIndex3] = k3;//first vertex
                        triangles3[triIndex3 + 1] = k3 + r + 1;//second vertex of the first triangle
                        triangles3[triIndex3 + 2] = k3 + r;

                        triangles3[triIndex3 + 3] = k3;
                        triangles3[triIndex3 + 4] = k3 + 1;
                        triangles3[triIndex3 + 5] = k3 + r + 1;

                        triIndex3 += 6;
                        k3++;
                    }
                    else if (x>r-1 && y>r-1)//bottom right
                    {
                        vertices4[k4] = allVertices[i];
                        triangles4[triIndex4] = k4;//first vertex
                        triangles4[triIndex4 + 1] = k4 + r + 1;//second vertex of the first triangle
                        triangles4[triIndex4 + 2] = k4 + r;

                        triangles4[triIndex4 + 3] = k4;
                        triangles4[triIndex4 + 4] = k4 + 1;
                        triangles4[triIndex4 + 5] = k4 + r + 1;

                        triIndex4 += 6;
                        k4++;
                    }
                }

                i++;

            }
        }
        mesh1.Clear();//clear data from mesh for when recalculating resolution
        mesh1.vertices = vertices1;
        mesh1.triangles = triangles1;
        mesh1.RecalculateNormals();
        mesh2.Clear();//clear data from mesh for when recalculating resolution
        mesh2.vertices = vertices2;
        mesh2.triangles = triangles2;
        mesh2.RecalculateNormals();
        mesh3.Clear();//clear data from mesh for when recalculating resolution
        mesh3.vertices = vertices3;
        mesh3.triangles = triangles3;
        mesh3.RecalculateNormals();
        mesh4.Clear();//clear data from mesh for when recalculating resolution
        mesh4.vertices = vertices4;
        mesh4.triangles = triangles4;
        mesh4.RecalculateNormals();
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
