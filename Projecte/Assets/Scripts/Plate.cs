using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> Content;
    private List<GameObject> states;
    private int limit = 4;
    bool locked = false;
    bool burnedFood = false;
    Vector3 StackPos;

    void Start()
    {
        Content = new List<GameObject>();
        StackPos = transform.position;
        states = new List<GameObject>();

        foreach (Transform child in transform) { 
            if(child.name == "plate" || child.name == "plateSopaT" || child.name == "plateSopaM") states.Add(child.gameObject);
            else if (child.name != "Ingredients")
            {
                Content.Add(child.gameObject);
                child.gameObject.GetComponent<Ingredient>().setInplate();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isFull()
    {
        if (Content.Count == limit) return true;
        else return false;
    }

    public bool isLocked()
    {
        return locked;
    }

    public void dumpContent()
    {
        foreach (GameObject Ing in Content) Destroy(Ing);
        Content = new List<GameObject>();
        locked = false;
        burnedFood = false;
        foreach (GameObject state in states) {
            if (state.name == "plate") state.SetActive(true);
            else state.SetActive(false);
        }
        clearUI();

    }

    internal bool isEmpty()
    {
        return Content.Count == 0;
    }

    public void setSopa(List<GameObject> items) {

        changeState(items[0].tag);

        foreach (GameObject item in items) {
            item.GetComponent<Ingredient>().changeState("sopa");
            Content.Add(item);
            item.transform.parent = transform;
            transform.GetChild(3).GetComponent<Ingredients>().spawn_ingredient(item.tag);
        }
        locked = true;
    }

    public void setPizza(List<GameObject> items)
    {
        locked = true;
        foreach (GameObject item in items){
            if (item.tag == "Dough") item.GetComponent<Ingredient>().changeState("Oven");
            transform.GetChild(3).GetComponent<Ingredients>().spawn_ingredient(item.tag);
            emplatar(item);
        }
    }

    public void setItem(GameObject item)
    {
        item.GetComponent<Ingredient>().changeState("Plate");
        emplatar(item);
        transform.GetChild(3).GetComponent<Ingredients>().spawn_ingredient(item.tag);
    }

    private void emplatar( GameObject item ) {
        Content.Add(item);
        item.transform.parent = transform;

        float stack_y = 0.0f;
        Vector3 newPos = new Vector3(transform.position.x, StackPos.y, transform.position.z);

        switch (item.tag)
        {
            case "Bread":
                float acumB = StackPos.y;
                stack_y = transform.position.y + 0.7f;
                newPos = new Vector3(transform.position.x, stack_y, transform.position.z);
                StackPos.y = transform.position.y + 0.2f;

                if (!isEmpty())
                {
                    //inside, move ingridients.
                    moveIngridientsUp(0.2f);
                    StackPos.y += (acumB - transform.position.y);
                }
                break;
            case "Dough":
                float acumD = StackPos.y;
                stack_y = transform.position.y + 0.0f;
                newPos = new Vector3(transform.position.x, stack_y, transform.position.z);
                StackPos.y = transform.position.y + 0.1f;

                if (!isEmpty())
                {
                    //encima, move ingridients.
                    moveIngridientsUp(0.1f);
                    StackPos.y += (acumD - transform.position.y);
                }
                break;
            case "Steak":
                //son 2 pixeles en y.
                stack_y = StackPos.y+0.9f; // 0.9f
                StackPos.y += 0.2f;
                newPos = new Vector3(transform.position.x, stack_y, transform.position.z);
                break;
            case "Cheese":
            case "Lettuce":
            case "Mushroom":
            case "Sausage":
            case "Tomato":
                //es un pixel en y.
                stack_y = StackPos.y + 0.0f;
                StackPos.y += 0.1f;
                newPos = new Vector3(transform.position.x, stack_y, transform.position.z);
                break;
        }

        item.transform.position = newPos;
        item.transform.rotation = new Quaternion(0f, -90f, 0f, 1f);
    }

    public List<GameObject> getItems()
    {
        List<GameObject> items = Content;
        Content = new List<GameObject>();
        StackPos = transform.position;
        clearUI();
        return items;
    }

    public bool has_Bread()
    {
        return has_Ingridient("Bread");
    }

    private bool has_Steak()
    {
        return has_Ingridient("Steak");
    }

    public bool has_Dough()
    {
        return has_Ingridient("Dough");
    }

    private bool has_Ingridient(string tagName)
    {
        foreach (GameObject Ing in Content)
        {
            if (Ing.tag == tagName) return true;
        }
        return false;
    }

    private void moveIngridientsUp(float up) {
        foreach (GameObject Ing in Content)
        {
            Vector3 newPos = new Vector3(Ing.transform.position.x, Ing.transform.position.y, Ing.transform.position.z);
            newPos.y += up;
            Ing.transform.position = newPos;
        }
    }

    private void changeState(string typeSopa)
    {
        string SopaName;
        if (typeSopa == "Tomato") SopaName = "plateSopaT";
        else SopaName = "plateSopaM";

        foreach (GameObject state in states)
        {
            if (state.name == SopaName) state.SetActive(true);
            else state.SetActive(false);
        }
    }

    public int getLimit()
    {
        return limit;
    }

    public void setBurnedFood()
    {
        burnedFood = true;
        clearUI();
    }

    public bool isBurned() { return burnedFood; }

    public void clearUI()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Ingredients") child.GetComponent<Ingredients>().clear();
        }
    }

    public void setLocked()
    {
        locked = true;
    }
}
