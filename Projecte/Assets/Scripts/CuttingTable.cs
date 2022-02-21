using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTable : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Holding;
    public AudioManager au;
    private Animator anim;
    private GameObject Knife;
    private bool cutting;
    private bool oldState;
    private double counter;
    public GameObject Progressbar;
    private GameObject c;
    private bool ingredient = false;
    void Start()
    {
        Knife = transform.GetChild(1).gameObject;
        Holding = null;
        waiting();
    }

    // Update is called once per frame
    void Update()
    {

        if (!oldState && cutting)
        {
            au.Play("Cutting");
        }
        else if (oldState && !cutting)
        {
            au.Stop("Cutting");
        }
        if (cutting ) cut();
        else waiting();
        if (ingredient)
        {
            foreach (Transform child in transform)
            {
                if (child.tag == "ProgressBarC") c.GetComponent<ProgressBarCut>().cutting(oldState);
            }
            
        }
    }

    public void cut()
    {           
        lowerCounter();
        Knife.GetComponent<Animator>().SetBool("Cutting", true);
        oldState = true;
        //c.GetComponent<ProgressBarCut>().cutting(false);
        
        cutting = false;
    }

    public void waiting()
    {
        Knife.GetComponent<Animator>().SetBool("Cutting", false);
        oldState = false;
    }

    private void lowerCounter()
    {
        if (counter > 0)
        {
            counter -= 60 * Time.deltaTime;
            c.GetComponent<ProgressBarCut>().estic_tallant((float)counter);
        }
        else if (counter <= 0 && Holding.GetComponent<Ingredient>().isCuttable())
        {
            Holding.GetComponent<Ingredient>().changeState("Cut");
        }
    }

    public void setItem(GameObject item)
    {
        Holding = item;
        counter = Holding.GetComponent<Ingredient>().getCutTime();
        Vector3 newPos = new Vector3(transform.position.x, item.transform.position.y, transform.position.z);
        item.transform.position = newPos;
        item.transform.rotation = new Quaternion(0f, -90f, 0f, 1f);
        c = Instantiate(Progressbar, transform);
        c.GetComponent<ProgressBarCut>().setTemps((float)counter);
        ingredient = true;
    }

    public GameObject getItem()
    {
        GameObject item = Holding;
        ingredient = false;
        Holding = null;
        Destroy(c);
        return item;
    }

    public bool hasItem()
    {
        return Holding != null;
    }

    public bool isCut()
    {
        return Holding.GetComponent<Ingredient>().isPlateReady() || Holding.GetComponent<Ingredient>().isCookable();
    }
    public bool isCutting()
    {
        return cutting;
    }
    public void setCutting()
    {
        if( Holding.GetComponent<Ingredient>().isCuttable()) cutting = true;
    }

    public bool isPlateReady()
    {
        return Holding.GetComponent<Ingredient>().isPlateReady();
    }
    public bool isCookable()
    {
        return Holding.GetComponent<Ingredient>().isCookable();
    }

    public GameObject getItem2Check()
    {
        return Holding;
    }
}
