using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> Content;
    private int limit = 4;
    Vector3 StackPos;

    void Start()
    {
        Content = new List<GameObject>();
        StackPos = new Vector3(transform.position.x, 3.12f, transform.position.z);
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

    public void dumpContent()
    {
        foreach (GameObject Ing in Content) Destroy(Ing);
        Content = new List<GameObject>();
    }

    internal bool isEmpty()
    {
        return Content.Count == 0;
    }

    public void setItem(GameObject item)
    {
        item.GetComponent<Ingredient>().changeState("Plate");

        Content.Add(item);
        item.transform.parent = transform;

        float stack_y = 0.0f;

        //asumimos que en un mismo nivel no puedes hacer pizzas y hambirguesas.
        if ( has_Bread() && !has_Steak() ) stack_y = 0.2f;
        else if ( has_Bread() && has_Steak() ) stack_y = 0.4f;
        else if ( has_Dough() ) stack_y = 0f; // ?
        else stack_y = 0.1f; 

        switch (item.tag)
        {
            case "Bread":
                //inside, move ingridients.
                stack_y = 0.1f;
                if( !isEmpty() ) moveIngridientsUp(0.3f);
                break;
            case "Dough":
                //encima, move ingridients.
                break;
            case "Cheese":
                //es un pixel en y.
                break;
            case "Lettuce": break;
            case "Mushroom": break;
            case "Sausage": break;
            case "Steak": break;
            case "Tomato": break;
        }

        stack_y += StackPos.y;
        Vector3 newPos = new Vector3(transform.position.x, stack_y, transform.position.z);
        item.transform.position = newPos;
        item.transform.rotation = new Quaternion(0f, -90f, 0f, 1f);
        
    }

    public List<GameObject> getItems()
    {
        List<GameObject> items = Content;
        Content = new List<GameObject>();
        StackPos = new Vector3(transform.position.x, 3.12f, transform.position.z);
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
}
