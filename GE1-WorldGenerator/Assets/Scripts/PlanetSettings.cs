using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlanetSettings : ScriptableObject
{
    //public Gradient[] gradients;
    //Have a list of the gradients and the materials to put them on for each biome
    public Gradient[] biomeGradients;
    public Material[] planetMaterials;

    
    public float planetRadius =1;
    public NoiseLayer[] noiseLayers;
    
    
}
