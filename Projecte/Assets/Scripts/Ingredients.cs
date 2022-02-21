using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredients : MonoBehaviour
{
    private List<GameObject> ingredients = new List<GameObject>();
    public GameObject ingredient;
    private float width;
    private int num = 0;
    private GameObject c;
    private int limit;
    private GameObject fire;
    private string pare;
    private bool foc;
    // Start is called before the first frame update
    void Start()
    {
        foc = false;
        fire = transform.GetChild(0).gameObject;
        RectTransform rt = (RectTransform)ingredient.transform;
        width = rt.rect.width; //scale
        c = GameObject.Find("Main Camera");
        string tag = transform.parent.gameObject.tag; //pot ser plat, sarten o olla
        switch (tag)
        {
            case "Plate":
                limit = 4;
                pare = "Plate";
                break;
            case "Sarten":
                limit = 1;
                pare = "Sarten";
                break;
            case "Olla":
                pare = "Olla";
                limit = 3;
                break;
            case "Horno":
                pare = "Oven";
                limit = 4;
                break;
        }
    }

    void Update()
    {
        transform.LookAt(c.transform);
        switch (pare)
        {
            case "Plate":
                foc = transform.parent.gameObject.GetComponent<Plate>().isBurned();
                break;
            case "Sarten":
                foc = transform.parent.gameObject.GetComponent<Sarten>().isBurned();
                break;
            case "Olla":
                foc = transform.parent.gameObject.GetComponent<Olla>().isBurned();
                break;
            case "Oven":
                foc = transform.parent.gameObject.GetComponent<Oven>().isBurned();
                break;
        }
        
        fire.SetActive(foc);

    }

    public void spawn_ingredient(string s) //la cridarem des de l'script de la escena amb la string de l'ingredient que agafi el player
    {
        if (num < limit) //si ens passem ja no spawnegem
        {
            ingredients.Add(Instantiate(ingredient, transform) as GameObject);
            ingredients[num].GetComponentInChildren<IngController>().setSprite(s);
            ingredients[num].transform.Translate(width*(num+1), 0, 0);
            num++;
        }
        
    }

    public void clear()
    {
        foreach (GameObject g in ingredients)
        {
            Destroy(g);
        }
        ingredients.Clear();
        ingredients = new List<GameObject>();
        num = 0;
    }

}
