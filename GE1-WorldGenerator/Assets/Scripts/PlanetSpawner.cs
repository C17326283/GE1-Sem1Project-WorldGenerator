using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSpawner : MonoBehaviour
{
    [HideInInspector]
    public GameObject planet;
    public PlanetSettings planetSettings;
    public Material mat;
    public GameObject extras;

    private GameObject spawnedExtras;
    private Planet planetScript;

    public Slider resSlider;
    
    // Start is called before the first frame update
    public void Generate()
    {
        planet = new GameObject("Spawned planet");
        planet.transform.parent = this.transform;
        planetScript = planet.AddComponent<Planet>();
        planetScript.planetSettings = planetSettings;
        planetScript.planetSettings.planetMaterials = mat;

        planetScript.GeneratePlanet();
    }

    public void AddExtras()
    {
        if(spawnedExtras == null)
            spawnedExtras = Instantiate(extras, planet.transform);
    }

    public void updateSettingsFromGUI()
    {
        planetScript.res = (int)resSlider.value;
        planetScript.GeneratePlanet();
    }
}
