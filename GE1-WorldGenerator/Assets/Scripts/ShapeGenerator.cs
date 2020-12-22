using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShapeGenerator
{
    /*
    private PlanetSettings settings;
    private NoiseLayer[] noiseLayers;
    public MinMax elevationMinMax;//for getting the highest and lowest points

    public void UpdateSettings(PlanetSettings settings)
    {
        this.settings = settings;
        noiseLayers = settings.noiseLayers;
        elevationMinMax = new MinMax();
        
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        //float elevation = noiseFilter.Evaluate(pointOnUnitSphere);
        float elevation = 0;
        
        //Use the previous layers as a mask so spikes go on top of other mountains not randomly
        if (noiseLayers.Length > 0)
        {
            firstLayerValue = noiseLayers[0].Evaluate(pointOnUnitSphere);
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
                elevation += noiseLayers[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        elevation = settings.planetRadius * (1 + elevation);
        elevationMinMax.AddValue(elevation);
        return pointOnUnitSphere * elevation;

    }
    */
}
