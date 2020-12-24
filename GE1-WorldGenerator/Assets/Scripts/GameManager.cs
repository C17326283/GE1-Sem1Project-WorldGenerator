using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject planet;
    public PlanetSettings planetSettings;
    public Material mat;
    
    // Start is called before the first frame update
    void Start()
    {
        planet = new GameObject("Spawned planet");
        Planet planetScript = planet.AddComponent<Planet>();
        planetScript.planetSettings = planetSettings;
        planetScript.planetSettings.planetMaterials = mat;
        
        
        planetScript.GeneratePlanet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
