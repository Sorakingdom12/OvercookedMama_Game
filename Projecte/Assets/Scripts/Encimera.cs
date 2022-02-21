using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encimera : MonoBehaviour
{
    private GameObject Holding;
    //public GameObject Progressbar;
    //private GameObject c;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount > 0) {
            Holding = transform.GetChild(0).gameObject;
            Holding.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool hasItem()
    {
        return Holding != null;
    }
    public void setItem(GameObject item)
    {
        Holding = item;
        Vector3 newPos = new Vector3(transform.position.x, item.transform.position.y, transform.position.z);
        item.transform.position = newPos;
        //if (item.tag == "Cheese") item.transform.rotation = new Quaternion(0f, -90f, 0f, 1f);
        //c = Instantiate(Progressbar, transform);

    }
    public GameObject getItem()
    {
        GameObject item = Holding;
        Holding = null;
        return item;        
    }

    public bool hasIngredient()
    {
        switch (Holding.tag)
        {
            case "Cheese":
            case "Steak":
            case "Mushroom":
            case "Lettuce":
            case "Tomato":
            case "Dough":
            case "Sausage":
            case "Bread":
                return true;
            default: return false;
        }
    }

    public bool hasContainer()
    {
        if (Holding != null && (Holding.tag == "Plate" || Holding.tag == "Olla" || Holding.tag == "Sarten")) return true;
        else return false;
    }

    public bool hasContainerSpace()
    {
        if (Holding.tag == "Plate") return !Holding.GetComponent<Plate>().isFull();
        else if (Holding.tag == "Olla") return !Holding.GetComponent<Olla>().isFull();
        else return !Holding.GetComponent<Sarten>().isFull();
    }

    public bool isPlateReady()
    {
        return Holding.GetComponent<Ingredient>().isPlateReady();
    }

    public string getItemTag()
    {
        return Holding.tag;
    }

    public bool isCookable()
    {
        return Holding.GetComponent<Ingredient>().isCookable();
    }

    public GameObject getItem2check()
    {
        return Holding;
    }
}
