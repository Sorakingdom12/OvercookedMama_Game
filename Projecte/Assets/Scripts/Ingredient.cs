using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    //public GameObject controlador;
    //private GameObject c;
    public string ingredient;
    private List<GameObject> states;
    private int ActualState;
    private bool cuttable = false;
    private bool plateReady = false;
    private bool cookable = false;
    private bool inPlate = false;
    private int cutTime;
    private int cookTime;
    // Start is called before the first frame update
    void Start()
    {
        cutTime = 90;
        cookTime = 500;
        states = new List<GameObject>();
        foreach (Transform child in transform){
            states.Add(child.gameObject);
            if (child.gameObject.activeSelf) ActualState = states.Count - 1;
        }
        if (states[0].name == "BurgerBuns") plateReady = true;
        else cuttable = true;
        /*c = Instantiate(controlador, transform) as GameObject; //és la UI amb la foto de l'ingredient
        c.transform.GetChild(1).GetComponent<IngController>().setSprite(ingredient);*/
    }

    public void setInplate()
    {
        cuttable = false;
        cookable = false;
        plateReady = false;
        inPlate = true;
    }

    // Update is called once per frame
    void Update()
    {
        //afegir que c sempre estigui mirant cap a la càmara
    }

    public void changeState(string from)
    {
        if (from == "Plate" && plateReady)
        {
            plateReady = false;
            inPlate = true;
            if (states[0].name != "doughRaw" && states[0].name != "sausageRaw" && states[0].name != "steakRaw" && ActualState != states.Count - 1)
            {
                states[ActualState].SetActive(false);
                ActualState++;
                states[ActualState].SetActive(true);
            }
        }
        else if (from == "Oven" && inPlate)
        {
            if (ActualState != states.Count - 1)
            {
                states[ActualState].SetActive(false);
                ActualState++;
                states[ActualState].SetActive(true);
            }
        }
        else if (from == "Sarten" && cookable)
        {
            cookable = false;
            plateReady = true;
            states[ActualState].SetActive(false);
            ActualState++;
            states[ActualState].SetActive(true);
        }
        else if (from == "Cut" && cuttable) {
            cuttable = false;
            if (tag != "Steak") plateReady = true;
            else cookable = true;
            states[ActualState].SetActive(false);
            ActualState++;
            states[ActualState].SetActive(true);
            if (tag == "Dough") transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
            if (tag == "Sausage") transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
        }
        else if(from == "sopa")
        {
            inPlate = true;
        }
    }

    public bool isCuttable()
    {
        return cuttable;
    }

    public bool isPlateReady()
    {
        return plateReady;
    }
    public bool isInPlate()
    {        
        return inPlate;
    }
    public bool isCookable()
    {
        return cookable;
    }
    public int getCutTime()
    {
        return cutTime;
    }
    public void setCutTime(int timeLeft)
    {
         cutTime = timeLeft;
    }

    public int getCookTime()
    {
        return cookTime;
    }
    public void setCookTime(int timeLeft)
    {
        cookTime = timeLeft;
    }
}
