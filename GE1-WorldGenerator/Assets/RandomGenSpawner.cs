﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomGenSpawner : MonoBehaviour
{
    [SerializeField]//Make private visible in inspector, need private so doesnt give error
    private ObjectPool multObjectPoolObj;
    public GameObject activeParent;
    private Vector3 core;//for raycasting spawn points
    
    public int amountToSpawn = 100;//also limited by pool max
    public int spawnerDistanceFromCore = 5000;
    public float heightFromHitPoint = 0;
    public String tagToSpawnOn = "Ground";

    [Header("Only randomises if condition is true")]
    public bool RandomiseScaleAndRotation = false;
    public float minScale = 1f;
    public float maxScale = 2f;
    public float randomXZTilt = 2f;

    private GameObject newObj;//declare here so can edit in reposition

    // Start is called before the first frame update
    void Awake()
    {
        core = activeParent.transform.position;
        multObjectPoolObj.InitPool();//force pool to be initiated instead of waiting for awake
        
        for (int i = 0; i < amountToSpawn; i++)
        {
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(gameObject.transform.position, core - gameObject.transform.position, Color.magenta);
    }

    private void Spawn()
    {
        newObj = null;
        
        RaycastHit hit;//shoot ray and if its ground then spawn at that location
        if (Physics.Raycast(transform.position, core - gameObject.transform.position, out hit, 5000))
        {
            if (hit.transform.CompareTag(tagToSpawnOn))//Checks its allowed spawn there
            {
                newObj = multObjectPoolObj.GetObj();
                if(newObj != null)
                {
                    newObj.SetActive(true);

                    newObj.transform.position = hit.point;//place object at hit
                    newObj.transform.up = newObj.transform.position - core;//set rotation so orients properly
                    newObj.transform.position = newObj.transform.position + newObj.transform.up * heightFromHitPoint;//repoisition to correct height from hit
                    
                    
                    if (RandomiseScaleAndRotation)
                    {
                        newObj.transform.parent = gameObject.transform.parent;//make its own parent so that scaling works after reactivating
                
                        float scale = Random.Range(minScale, maxScale);
                        newObj.transform.localScale = Vector3.one * scale;//.one for all round scale
                        newObj.transform.Rotate(Random.Range(-randomXZTilt,randomXZTilt),Random.Range(0,360),Random.Range(-randomXZTilt,randomXZTilt));
                    }
                    newObj.transform.parent = activeParent.transform;//set parent to correct obj
                }
            }
        }
        else
        {
            Debug.Log("no hit");
        }

        Resposition();
    }

    public void Resposition()
    {
        gameObject.transform.position = core;
        gameObject.transform.rotation  = Random.rotation;
        gameObject.transform.position = transform.forward * spawnerDistanceFromCore;
    }
}
