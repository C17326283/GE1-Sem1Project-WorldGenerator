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
    [Range(0, 10)] 
    public float roughness = 1;
    [Range(0, 15)] 
    public int NumOfNoiseLayers = 1;
    [Range(0, 10)] 
    public float persistance = .5f; //amplitude halves each layer
    [Range(0, 10)] 
    public float baseRoughness = 1;
    [Range(0, 10)] 
    public float minValue;

    public Vector3 centre;

    //use the noise from libnoise-dotnet.
    Noise noise = new Noise();
    
    public float Evaluate(Vector3 point)
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

        noiseValue = Mathf.Max(0, noiseValue - minValue);//flatten to min Value
        
        return noiseValue * strength;
    }
}
