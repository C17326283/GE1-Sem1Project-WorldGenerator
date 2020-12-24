﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomGenSpawner : MonoBehaviour
{
    [SerializeField]//Make private visible in inspector, need private so doesnt give error
    private ObjectPool multObjectPoolObj;
    public GameObject parentObject;
    public GameObject planetObject;
    private Vector3 core;//for raycasting spawn points
    
    public int amountToSpawn = 10000;//also limited by pool max
    public int spawnerDistanceFromCore = 5000;
    public float heightFromHitPoint = 0;
    public String tagToSpawnOn = "Ground";
    public bool isRotatingObject;//for clouds
    public int poolIndexNumber = 0;

    
    [Header("Only randomises if condition is true")]
    public bool RandomiseScaleAndRotation = false;
    public float minScale = 1f;
    public float maxScale = 2f;
    public float randomXZTilt = 2f;

    private GameObject newObj;//declare here so can edit in reposition

    // Start is called before the first frame update
    
    void Awake()
    {
        multObjectPoolObj = GetAssociatedPool();
        if (planetObject == null)
        {
            planetObject = this.transform.root.gameObject; //get the base object which will be the planet
            parentObject = new GameObject("Holder");
            if (isRotatingObject)
                parentObject.AddComponent<RotateEnvironment>();//for making the clouds move

        }

        core = planetObject.transform.position;
        multObjectPoolObj.InitPool();//force pool to be initiated instead of waiting for awake
        
        StartCoroutine(Spawn());
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(gameObject.transform.position, core - gameObject.transform.position, Color.magenta);
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            yield return new WaitForSeconds(.1f);
            newObj = null;

            RaycastHit hit; //shoot ray and if its ground then spawn at that location
            if (Physics.Raycast(transform.position, core - gameObject.transform.position, out hit, 10000))
            {
                Debug.Log(hit.transform.name + hit.transform.position);
                if (hit.transform.CompareTag(tagToSpawnOn)) //Checks its allowed spawn there
                {
                    newObj = multObjectPoolObj.GetObj();
                    if (newObj != null)
                    {
                        newObj.SetActive(true);

                        newObj.transform.position = hit.point; //place object at hit
                        newObj.transform.up = newObj.transform.position - core; //set rotation so orients properly
                        newObj.transform.position =
                            newObj.transform.position +
                            newObj.transform.up * heightFromHitPoint; //repoisition to correct height from hit


                        if (RandomiseScaleAndRotation)
                        {
                            newObj.transform.parent =
                                gameObject.transform
                                    .parent; //make its own parent so that scaling works after reactivating

                            float scale = Random.Range(minScale, maxScale);
                            newObj.transform.localScale = Vector3.one * scale; //.one for all round scale
                            newObj.transform.Rotate(Random.Range(-randomXZTilt, randomXZTilt), Random.Range(0, 360),
                                Random.Range(-randomXZTilt, randomXZTilt));
                        }

                        newObj.transform.parent = parentObject.transform; //set parent to correct obj
                    }
                }
            }
            else
            {
                Debug.Log("no hit");
            }
        }

        Resposition();
    }


    public ObjectPool GetAssociatedPool()
    {
        //See if any of the pools match and return that else return null
        foreach (var Pool in transform.root.GetComponents<ObjectPool>())
        {
            print("Pool.poolIndex"+Pool.poolIndex+"poolIndexNumber"+poolIndexNumber);
            if(poolIndexNumber == Pool.poolIndex)//if have matching index numbers. This is so clouds can have cloud settings etc.
                return Pool;
        }
        return null;
    }
    public void Resposition()
    {
        gameObject.transform.position = core;
        gameObject.transform.rotation  = Random.rotation;
        gameObject.transform.position = transform.forward * spawnerDistanceFromCore;
    }
}
