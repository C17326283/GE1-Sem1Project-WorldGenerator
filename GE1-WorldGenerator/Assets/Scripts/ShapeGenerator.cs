using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShapeGenerator
{
    
    private ShapeSettings settings;
    private NoiseFilter[] noiseFilters;
    public MinMax elevationMinMax;//for getting the highest and lowest points

    public ShapeGenerator(ShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new NoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = new NoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
        elevationMinMax = new MinMax();
        
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        //float elevation = noiseFilter.Evaluate(pointOnUnitSphere);
        float elevation = 0;
        
        //Use the previous layers as a mask so spikes go on top of other mountains not randomly
        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }
            
            
        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;//? if true do this else set default of 1
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        elevation = settings.planetRadius * (1 + elevation);
        elevationMinMax.AddValue(elevation);
        return pointOnUnitSphere * elevation;

    }
    
}
