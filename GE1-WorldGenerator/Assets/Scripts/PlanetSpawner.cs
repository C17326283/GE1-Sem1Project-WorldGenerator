using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    private GameObject planet;
    public PlanetSettings planetSettings;
    public Material mat;
    public GameObject extras;
    
    // Start is called before the first frame update
    void Start()
    {
        planet = new GameObject("Spawned planet");
        planet.transform.parent = this.transform;
        Planet planetScript = planet.AddComponent<Planet>();
        planetScript.planetSettings = planetSettings;
        planetScript.planetSettings.planetMaterials = mat;

        planetScript.GeneratePlanet();
        Instantiate(extras, planet.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
