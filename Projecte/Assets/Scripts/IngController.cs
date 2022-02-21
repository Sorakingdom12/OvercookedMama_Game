using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngController : MonoBehaviour
{
    public Sprite ingredient1, ingredient2, ingredient3, ingredient4, ingredient5, ingredient6, ingredient7, ingredient8;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setSprite(string s)
    {
        switch (s)
        {
            case "Bread":
                this.GetComponent<Image>().sprite = ingredient1;
                break;
            case "Cheese":
                this.GetComponent<Image>().sprite = ingredient2;
                break;
            case "Lettuce":
                this.GetComponent<Image>().sprite = ingredient3;
                break;
            case "Dough":
                this.GetComponent<Image>().sprite = ingredient4;
                break;
            case "Mushroom":
                this.GetComponent<Image>().sprite = ingredient5;
                break;
            case "Sausage":
                this.GetComponent<Image>().sprite = ingredient6;
                break;
            case "Steak":
                this.GetComponent<Image>().sprite = ingredient7;
                break;
            case "Tomato":
                this.GetComponent<Image>().sprite = ingredient8;
                break;
        }
    }
}
