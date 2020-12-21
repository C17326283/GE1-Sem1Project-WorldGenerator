using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for setting the min and max of the material for terrain elevation colours
public class ColourGenerator
{
    private ColourSettings settings;
    
    public ColourGenerator(ColourSettings settings)
    {
        this.settings = settings;
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min,elevationMinMax.Max));
    }
}
