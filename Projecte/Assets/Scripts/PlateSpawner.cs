using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSpawner : MonoBehaviour
{
    private List<GameObject> plates;
    private int spawnablePlates;
    private int timer;
    [SerializeField] GameObject Plate;
    // Start is called before the first frame update
    void Start()
    {
        plates = new List<GameObject>();
        spawnablePlates = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnablePlates > 0 && timer == 0)
        {
            plates.Add(Instantiate(Plate, transform.position + new Vector3(0,3.2f,0), transform.rotation, transform));
            timer = 1000;
            spawnablePlates--;
        }
        if (timer > 0) --timer;
    }

    public void setItem()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Plate");
        if (list.Length < 3)
        {
            spawnablePlates++;
        }
    }
    public GameObject getItem()
    {
        GameObject lastPlate = plates[plates.Count - 1];
        plates.RemoveAt(plates.Count - 1);
        return lastPlate;
    }

    public bool hasItem()
    {
        return plates.Count > 0;
    }
}
