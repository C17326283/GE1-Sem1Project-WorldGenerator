using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace : MonoBehaviour
{
    private ShapeGenerator shapeGenerator;
    private Mesh mesh;

    private int res;
    public int biome;

    private Vector3 localUp;
    private Vector3 axisA;
    private Vector3 axisB;


    //constructor for initalising the terrain face parameters
    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int res, Vector3 localUp, int biome)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.res = res;
        this.localUp = localUp;
        this.biome = biome;
        
        
        axisA = new Vector3(localUp.y, localUp.z,localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    //Make the actual mesh of the face with verticies and triangles
    public void ConstructMesh()
    {
        //make an array for verticies and triangles based on the resolution
        Vector3[] vertices = new Vector3[res*res];
        int[] triangles = new int[(res-1)*(res-1)*6];//-1 to avoid the points at end that dont need triangles
        int triIndex = 0;//Index for each individual point

        //edit each vertex to the right position
        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                
                int i = x + y * res;//get the point on the grid
                Vector2 percent = new Vector2(x,y) / (res-1);//percentage of width for even spacing
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f)*2*axisA + (percent.y - .5f)*2*axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;//inflate mesh to be sphere
                //Move the varticies to where ti would be on the sphere
                vertices[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);
                
                //get trianle points from points on mesh
                if(x != res-1 &&  y != res-1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex+1] = i+res+1;
                    triangles[triIndex+2] = i+res;
                    
                    triangles[triIndex+3] = i;
                    triangles[triIndex+4] = i+1;
                    triangles[triIndex+5] = i+res+1;

                    triIndex += 6;
                }
            }
        }
        
        //get the mesh points from the made points
        mesh.Clear();//clear data from mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
    }
}
