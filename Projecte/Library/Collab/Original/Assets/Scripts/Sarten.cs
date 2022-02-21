using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sarten : MonoBehaviour
{
    private GameObject Content;
    Vector3 StackPos;
    bool cocinado;
    bool burn;
    bool warning;
    private double counter;
    public GameObject Progressbar;
    private GameObject c;
    public bool GM_NoCremar = false;
    private bool UIIngredients = false;

    // Start is called before the first frame update
    void Start()
    {
        Content = null;
        cocinado = false;
        burn = false;
        counter = 0;
        StackPos = new Vector3(transform.position.x, 1.8f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //si padre fogon -> cocinar.
        if (transform.parent.CompareTag("fogon") && isFull() && !burn) {
            Cocinar();
        }
        if (!UIIngredients && !burn) UIIngr();

    }

    public bool isDoneCooking()
    {
        return cocinado;
    }

    public void dumpContent()
    {
        Destroy(Content);
        Content = null;
        clearUI();
        cocinado = false;
        burn = false;
        counter = 0;
    }

    public bool isFull()
    {
        return Content != null;
    }

    public void setItem(GameObject item)
    {
        Content = item;
        counter = Content.GetComponent<Ingredient>().getCookTime();

        //mirar en que orientaci?n est? la sarten para centrar la carne(0->up, 90-> derecha, 180-> down, 270->Izquierda )
        Vector3 rotacion = item.transform.parent.transform.rotation.eulerAngles;
        float move_x = 0;
        float move_z = 0;
        if (rotacion.y >= 45 && rotacion.y <= 135) move_x = 0.32f;
        else if (rotacion.y >= 135 && rotacion.y <= 225) move_z = -0.32f; 
        else if (rotacion.y >= 225 && rotacion.y <= 315) move_x = -0.32f; 
        else move_z = 0.32f;

        item.transform.parent = transform;
        Vector3 newPos = new Vector3(transform.position.x + move_x , item.transform.position.y, transform.position.z + move_z);
        item.transform.position = newPos;
        transform.GetChild(0).GetComponent<Ingredients>().spawn_ingredient(item.tag);
        UIIngredients = true;
    }

    public GameObject getItem()
    {
        GameObject item = Content;
        Content = null;
        counter = 0;
        cocinado = false;
        burn = false;
        clearUI();
        return item;
    }

    private void Cocinar() {
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
            if (!cocinado)
            {
                Debug.Log("Listo");
                cocinado = true;
                counter = Content.GetComponent<Ingredient>().getCookTime() / 2;
                Content.GetComponent<Ingredient>().changeState("Sarten");
                foreach (Transform child in transform)
                {
                    if (child.tag == "ProgressBar") child.GetComponent<ProgressBar>().start_warning(true);
                }
            }
            else if (!GM_NoCremar)
            {
                transform.parent.GetComponentInParent<Fogon>().startFire(); //Fuego
                burn = true;
                clearUI();
            }
        }        
    }

    public bool isBurned() { return burn; }

    public int getLimit()
    {
        return 1;
    }
    public void Toggle_GM_NoCremar()
    {
        GM_NoCremar = !GM_NoCremar;
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
        if(Content != null)transform.GetChild(0).GetComponent<Ingredients>().spawn_ingredient(Content.tag);
        UIIngredients = true;
    }

}
