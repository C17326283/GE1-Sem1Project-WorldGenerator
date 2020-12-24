using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseLayer
{
    public bool enabled = true;
    public bool useFirstLayerAsMask;
    
    //Settings
    public float strength = 0.1f;
    public float roughness = 1;
    public int NumOfNoiseLayers = 1;
    public float persistance = .5f; //amplitude halves each layer
    public float baseRoughness = 1;
    public float minValue;

    public Vector3 centre;

    //use the noise from libnoise-dotnet.
    Noise noise = new Noise();
    
    public float AddNoise(Vector3 point)
    {
        //float noiseValue = (noise.Evaluate(point * settings.roughness + settings.centre) + 1) * .5f;
        float noiseValue = 0;
        float frequency = baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < NumOfNoiseLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + centre);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= roughness;
            amplitude *= persistance;
        }

        noiseValue = Mathf.Max(0, noiseValue - minValue);//clamp so anything with noise doesnt go below water level, leaving space for other object
        
        return noiseValue * strength;
    }
}
