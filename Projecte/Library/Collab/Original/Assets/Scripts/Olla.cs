using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Olla : MonoBehaviour
{
    List<GameObject> Content;
    List<GameObject> statesTomato;
    List<GameObject> statesMushroom;
    bool cocinado;
    bool burn;
    private int maxSize = 3;
    private double counter = 0;
    private int ActualState = 0;
    public GameObject Progressbar;
    private GameObject c;
    public bool GM_NoCremar = false;
    private bool first = true;
    private bool UIIngredients = false;

    // Start is called before the first frame update
    void Start()
    {
        statesTomato = new List<GameObject>();
        statesMushroom = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if(child.name == "ollaTomato")
            {
                foreach (Transform Tomatochild in child.transform) {
                    statesTomato.Add(Tomatochild.gameObject);
                }
                
            }
            else if (child.name == "ollaMushroom") {
                foreach (Transform Mushroomchild in child.transform)
                {
                    statesMushroom.Add(Mushroomchild.gameObject);
                }
      
            }
            Debug.Log(child.name);
            //if (child.gameObject.activeSelf) ActualState = states.Count - 1;
        }
        Content = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        //si padre fogon -> cocinar.
        if (transform.parent.CompareTag("fogon") && !isEmpty() && !burn)
        {

            Cocinar();            
        }
        if (!UIIngredients && !burn) UIIngr();
    }
    public bool isFull()
    {
        if (Content.Count == maxSize) return true;
        else return false;
    }

    public bool isEmpty()
    {
        return Content.Count == 0;
    }

    public bool isDoneCooking()
    {
        return cocinado;
    }

    public void dumpContent()
    {
        foreach (GameObject Ing in Content) Destroy(Ing);
        Content = new List<GameObject>();
        ActualState = 0;
        counter = 0;
        cocinado = false;
        burn = false;
        clearUI();
        ollaVaciaState();
    }



    public void setItem(GameObject item)
    {
        Content.Add(item);
        counter += item.GetComponent<Ingredient>().getCookTime();
        if (cocinado)
        { //quan un tomàquet/mushroom s'ha acabat de cuinar però n'hi afegim un altre
            cocinado = false;
            clearUI();
            UIIngr();
        }
        else transform.GetChild(3).GetComponent<Ingredients>().spawn_ingredient(item.tag);
        changeState(Content[0].tag);

        item.SetActive(false);
        item.transform.parent = transform;
        Vector3 newPos = new Vector3(transform.position.x, item.transform.position.y, transform.position.z - 0.32f);
        item.transform.position = newPos;
        
        
        UIIngredients = true;
    }

    public List<GameObject> getItems()
    {
        List<GameObject> items = Content;
        Content = new List<GameObject>();
        ActualState = 0;
        counter = 0;
        cocinado = false;
        burn = false;
        ollaVaciaState();
        clearUI();
        return items;
    }

    private void Cocinar()
    {        
        int prog = 0;
        if (counter > 0)
        {
            counter -= 60 * Time.deltaTime;
            foreach (Transform child in transform)
            {
                if (child.tag == "ProgressBar")
                {
                    child.GetComponent<ProgressBar>().estic_cuinant((float)counter);
                    prog++;
                }
            }
            if(prog == 0)
            {
                c = Instantiate(Progressbar, transform);
                c.GetComponent<ProgressBar>().setTemps((float)counter);
            }            
        }
        else if (counter <= 0)
        {
            //terminar de cocinar
            if (!cocinado)
            {
                cocinado = true;
                counter = (Content[0].GetComponent<Ingredient>().getCookTime() / 2) * Content.Count;
                foreach (Transform child in transform)
                {
                    if (child.tag == "ProgressBar") child.GetComponent<ProgressBar>().start_warning(true);
                }
            }
            else if (!GM_NoCremar)
            {
                Debug.Log("Fuego");
                transform.parent.GetComponentInParent<Fogon>().startFire(); //Fuego
                burn = true;
                clearUI();
            }

        }

    }


    public bool isBurned() { return burn; }

    private void ollaVaciaState()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "ollaVacia") child.gameObject.SetActive(true);
            else if (child.name == "ollaTomato")
            {
                foreach (Transform Tomatochild in child.transform)
                {
                    Tomatochild.gameObject.SetActive(false);
                }
                child.gameObject.SetActive(false);
            }
            else if (child.name == "ollaMushroom")
            {
                foreach (Transform Mushroomchild in child.transform)
                {
                    Mushroomchild.gameObject.SetActive(false);
                }
                child.gameObject.SetActive(false);
            }
        }
    }

    private void changeState( string type ) {

        foreach (Transform child in transform)
        {
            if ( child.name == "ollaVacia" && child.gameObject.activeSelf ) {
                child.gameObject.SetActive(false);
            }
            else if ( child.name == "ollaTomato" && type == "Tomato") {
                if (child.gameObject.activeSelf) {
                    statesTomato[ActualState].SetActive(false);
                    ActualState++;
                    statesTomato[ActualState].SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(true);
                    statesTomato[ActualState].SetActive(true);
                }
            }
            else if ( child.name == "ollaMushroom" && type == "Mushroom") {
                if (child.gameObject.activeSelf)
                {
                    statesMushroom[ActualState].SetActive(false);
                    ActualState++;
                    statesMushroom[ActualState].SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(true);
                    statesMushroom[ActualState].SetActive(true);
                }
            }
        }
            
    }
    public int getLimit()
    {
        return maxSize;
    }

    public void clearUI()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Ingredients") child.GetComponent<Ingredients>().clear();
            if (child.tag == "ProgressBar") child.GetComponent<ProgressBar>().quitar_plato();
        }
        UIIngredients = false;
    }

    private void UIIngr()
    {
        foreach (GameObject g in Content)
        {
            transform.GetChild(3).GetComponent<Ingredients>().spawn_ingredient(g.tag);        
        }
        UIIngredients = true;
    }

    public void Toggle_GM_NoCremar()
    {
        GM_NoCremar = !GM_NoCremar;
    }
}
