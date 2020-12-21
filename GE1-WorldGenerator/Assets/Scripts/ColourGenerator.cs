using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for setting the min and max of the material for terrain elevation colours
public class ColourGenerator
{
    private ColourSettings settings;
    Texture2D texture;//texture for assigning gradient to shadergraph
    private int textureResolution = 50;
    
    
    public void UpdateSettings(ColourSettings settings)
    {
        this.settings = settings;
        if (texture == null)
        {
            texture = new Texture2D(textureResolution,1);
        }
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min,elevationMinMax.Max));
    }

    public void UpdateColours()
    {
        
        
        Color[] colours = new Color[textureResolution];
        for (int i = 0; i < textureResolution; i++)
        {
            int randBiome = Random.Range(0, 2);
            
            colours[i] = settings.gradients[randBiome].Evaluate(i / (textureResolution - 1f));
            texture.SetPixels(colours);
            texture.Apply();
            settings.planetMaterial.SetTexture("_texture",texture);
        }
    }
}
