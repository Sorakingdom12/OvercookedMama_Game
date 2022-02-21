using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTable : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Holding;
    private Animator anim;
    private GameObject Knife;
    private bool cutting;
    private bool oldState;
    private double counter;
    public GameObject Progressbar;
    private GameObject c;
    void Start()
    {
        Knife = transform.GetChild(1).gameObject;
        Holding = null;
        waiting();
    }

    // Update is called once per frame
    void Update()
    {
        if (oldState != cutting)
        {
            if (cutting) cut();
            else waiting();
        }
    }

    public void cut()
    {           
        Debug.Log("cut " + counter);
        if(Holding != null && Holding.GetComponent<Ingredient>().isCuttable())
        {
            lowerCounter();
            Knife.GetComponent<Animator>().SetBool("Cutting", true);
            oldState = true;
        }        
    }

    public void waiting()
    {
        //Debug.Log("wait");
        Knife.GetComponent<Animator>().SetBool("Cutting", false);
        oldState = false;
    }

    private void lowerCounter()
    {
        Debug.Log(counter);
        if (counter > 0) counter -= 60 * Time.deltaTime;
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
    }

    public GameObject getItem()
    {
        GameObject item = Holding;
        //Debug.Log(item.name);
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
        return Holding.GetComponent<Ingredient>().isPlateReady();
    }

}
