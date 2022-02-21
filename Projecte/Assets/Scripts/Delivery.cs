using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    private GameObject spawner;
    private GameObject UI;
    [SerializeField] private AudioManager Au;
    [SerializeField] GameObject Plate;

    // Start is called before the first frame update
    void Start()
    {
        //buscar spawnwe platos
        spawner = GameObject.FindWithTag("SpawnPlates");
        UI = GameObject.Find("UIEscena");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void payPoints(List<GameObject> gameObjects, bool locked)
    {

        List<string> ings = new List<string>();
        foreach (GameObject go in gameObjects)
        {
            if (!go.GetComponent<Ingredient>().isInPlate()) return;
            ings.Add(go.tag);
        }
        int res = UI.GetComponent<GestioEscena>().checkRecipe(ings,locked);
        if (res != -1)
        {
            UI.GetComponent<GestioEscena>().Deliver(res);
        }
        else Au.Play("Failure");
        return;
    }

    public void setItem(GameObject item)
    {
        List<GameObject> ingreds = item.GetComponent<Plate>().getItems();
        bool locked = item.GetComponent<Plate>().isLocked();
        payPoints(ingreds,locked);
        foreach (GameObject go in ingreds) Destroy(go);
        item.SetActive(false);
        Destroy(item);
        Invoke("addNewPlate", 5);
    }

    private void addNewPlate()
    {
        spawner.GetComponent<PlateSpawner>().setItem();
    }
}
