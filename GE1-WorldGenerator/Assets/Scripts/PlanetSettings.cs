﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlanetSettings : ScriptableObject
{
    //public Gradient[] gradients;
    //Have a list of the gradients and the materials to put them on for each biome
    public Gradient[] biomeGradients;
    public Material planetMaterials;

    [HideInInspector]
    public float planetRadius =100;//always public, could change in future but scaling works for other planets
    public NoiseLayer[] noiseLayers;
    public Boolean havePoles = true;

    //public GameObject biomeObj;




}
