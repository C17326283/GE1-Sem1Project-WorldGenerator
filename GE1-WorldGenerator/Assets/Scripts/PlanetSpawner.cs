using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlanetSpawner : MonoBehaviour
{
    [HideInInspector]
    public GameObject planet;
    public PlanetSettings planetSettings;
    public PlanetSettings defaultPlanetSettings;
    public Material mat;
    public Material waterMat;
    public Material atmosphereMat;
    public GameObject extras;

    private GameObject spawnedExtras;
    private GameObject spawnerSpawners;
    private Planet planetScript;

    public GameObject resolutionOption;
    private Slider resSlider;
    private TextMeshProUGUI resTextMeshPro;
    public GameObject strengthOption;
    private Slider strengthSlider;
    private TextMeshProUGUI strengthTextMeshPro;
    public GameObject roughnessOption;
    private Slider roughnessSlider;
    private TextMeshProUGUI roughnessTextMeshPro;
    public GameObject persistanceOption;
    private Slider persistanceSlider;
    private TextMeshProUGUI persistanceTextMeshPro;
    public GameObject baseRoughnessOption;
    private Slider baseRoughnessSlider;
    private TextMeshProUGUI baseRoughnessTextMeshPro;
    
    public GameObject noiseLayersOption;
    private Slider noiseLayersSlider;
    private TextMeshProUGUI noiseLayersTextMeshPro;

    public void Start()
    {
        planetSettings = Instantiate(defaultPlanetSettings);
        resSlider = resolutionOption.GetComponentInChildren<Slider>();
        resTextMeshPro = resolutionOption.GetComponentInChildren<TextMeshProUGUI>();
        strengthSlider = strengthOption.GetComponentInChildren<Slider>();
        strengthTextMeshPro = strengthOption.GetComponentInChildren<TextMeshProUGUI>();
        roughnessSlider = roughnessOption.GetComponentInChildren<Slider>();
        roughnessTextMeshPro = roughnessOption.GetComponentInChildren<TextMeshProUGUI>();
        persistanceSlider = persistanceOption.GetComponentInChildren<Slider>();
        persistanceTextMeshPro = persistanceOption.GetComponentInChildren<TextMeshProUGUI>();
        baseRoughnessSlider = baseRoughnessOption.GetComponentInChildren<Slider>();
        baseRoughnessTextMeshPro = baseRoughnessOption.GetComponentInChildren<TextMeshProUGUI>();
        
        noiseLayersSlider = noiseLayersOption.GetComponentInChildren<Slider>();
        noiseLayersTextMeshPro = noiseLayersOption.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    public void Generate()
    {
        if (planet == null)
        {
            planet = new GameObject("Spawned planet");
            planet.transform.parent = this.transform;
            planetScript = planet.AddComponent<Planet>();
            planetScript.planetSettings = planetSettings;
            planetScript.planetSettings.planetMaterial = mat;
            planetScript.planetSettings.waterMaterial = waterMat;
            setDefaultSettings();
            planetScript.planetSettings.planetRadius *= 10;//make 10 times bigger
        }
        

        updateSettingsFromGUI();
        planetScript.GeneratePlanet();
    }

    public void AddExtras()
    {
        if (spawnedExtras == null)
        {
            spawnedExtras = Instantiate(extras, planet.transform);
            GameObject atmosphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            atmosphere.GetComponent<Collider>().enabled = false;
            atmosphere.transform.localScale = Vector3.one * ((planetSettings.planetRadius*2) + planet.GetComponent<Planet>().elevationMinMax.Max);//make it outside the radius
            atmosphere.GetComponent<Renderer>().material = atmosphereMat;
            FlipNormals(atmosphere);

        }
    }

    //sets the settings using the gui details when generate is clicked
    public void updateSettingsFromGUI()
    {
        planetScript.res = (int)resSlider.value;
        planetSettings.noiseLayers[0].strength = strengthSlider.value;
        planetSettings.noiseLayers[0].roughness = roughnessSlider.value;
        planetSettings.noiseLayers[0].persistance = persistanceSlider.value;
        planetSettings.noiseLayers[0].baseRoughness = baseRoughnessSlider.value;
        
        planetSettings.noiseLayers[0].NumOfNoiseLayers = (int)noiseLayersSlider.value;
    }
    
    //sets the value and slider at beginning from the base template
    public void setDefaultSettings()
    {
        planetScript.res = 100;
        resSlider.value = planetScript.res;
        
        planetSettings.noiseLayers[0].strength = defaultPlanetSettings.noiseLayers[0].strength;
        strengthSlider.value = defaultPlanetSettings.noiseLayers[0].strength;

        planetSettings.noiseLayers[0].roughness = defaultPlanetSettings.noiseLayers[0].roughness;
        roughnessSlider.value = defaultPlanetSettings.noiseLayers[0].roughness;
        planetSettings.noiseLayers[0].persistance = defaultPlanetSettings.noiseLayers[0].persistance;
        persistanceSlider.value = defaultPlanetSettings.noiseLayers[0].persistance;
        planetSettings.noiseLayers[0].baseRoughness = defaultPlanetSettings.noiseLayers[0].baseRoughness;
        baseRoughnessSlider.value = defaultPlanetSettings.noiseLayers[0].baseRoughness;
        
        planetSettings.noiseLayers[0].NumOfNoiseLayers = defaultPlanetSettings.noiseLayers[0].NumOfNoiseLayers;
        noiseLayersSlider.value = defaultPlanetSettings.noiseLayers[0].NumOfNoiseLayers;
        UpdateGUIDetails();
    }

    //sets the gui details whenever the sliders are moved
    public void UpdateGUIDetails()
    {
        
        resTextMeshPro.text = "Resolution: "+resSlider.value;
        strengthTextMeshPro.text = "Strength: "+strengthSlider.value;
        roughnessTextMeshPro.text = "Roughness: "+roughnessSlider.value;
        persistanceTextMeshPro.text = "Amplification: "+persistanceSlider.value;
        baseRoughnessTextMeshPro.text = "Continent Spread: "+baseRoughnessSlider.value;
        noiseLayersTextMeshPro.text = "Cycles: "+noiseLayersSlider.value;
        //resTextMeshPro.text = "Resolution: "+defaultPlanetSettings.noiseLayers[0].strength;
    }

    public void MoveLand()
    {
        planetSettings.noiseLayers[0].centre = new Vector3(Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f));
        Generate();
    }

    public void Reset()
    {
        planetScript.res = 100;
        resSlider.value = planetScript.res;
        
        planetSettings.noiseLayers[0].strength = defaultPlanetSettings.noiseLayers[0].strength;
        strengthSlider.value = defaultPlanetSettings.noiseLayers[0].strength;

        planetSettings.noiseLayers[0].roughness = defaultPlanetSettings.noiseLayers[0].roughness;
        roughnessSlider.value = defaultPlanetSettings.noiseLayers[0].roughness;
        planetSettings.noiseLayers[0].persistance = defaultPlanetSettings.noiseLayers[0].persistance;
        persistanceSlider.value = defaultPlanetSettings.noiseLayers[0].persistance;
        planetSettings.noiseLayers[0].baseRoughness = defaultPlanetSettings.noiseLayers[0].baseRoughness;
        baseRoughnessSlider.value = defaultPlanetSettings.noiseLayers[0].baseRoughness;
        
        planetSettings.noiseLayers[0].NumOfNoiseLayers = defaultPlanetSettings.noiseLayers[0].NumOfNoiseLayers;
        noiseLayersSlider.value = defaultPlanetSettings.noiseLayers[0].NumOfNoiseLayers;
        planetSettings.noiseLayers[0].centre = defaultPlanetSettings.noiseLayers[0].centre;
        planetSettings.havePoles = true;
        Generate();
    }

    public void Explore()
    {
        
    }

    public void RandomiseBiomes()
    {
        
        planetSettings.havePoles = false;
        planet.GetComponent<Planet>().SetBiomes();
        Generate();
    }

    public void LiveGeneration()
    {
        
    }

    public void FlipNormals(GameObject atmoObj)
    {
        //get normals from mesh
        Mesh mesh = atmoObj.GetComponent<MeshFilter>().mesh;
        Vector3[] normals = mesh.normals;
        //flip mesh
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -1 * normals[i];
        }

        mesh.normals = normals;
        //reverse clockwise triangles
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] tris = mesh.GetTriangles(i);
            for (int j = 0; j < tris.Length; j+=3)
            {
                int temp = tris[j];
                tris[j] = tris[j + 1];
                tris[j + 1] = temp;
            }
            mesh.SetTriangles(tris,i);

        }
        
        
    }
}
