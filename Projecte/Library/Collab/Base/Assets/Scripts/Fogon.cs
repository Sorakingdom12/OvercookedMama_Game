using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fogon : MonoBehaviour
{
    private GameObject Holding;
    private ParticleSystem ps_fuego;
    public GameObject Progressbar;
    private GameObject c;
    private int times = 300;
    private float LifeTime = 1;
    private int no_apagando = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount > 0)
        {
            foreach(Transform child in transform)
            {
                if (child.tag == "Sarten" || child.tag == "Olla")
                {
                    Holding = child.gameObject;
                    Holding.transform.parent = transform;                    
                }
                else if (child.tag == "Fuego")
                {
                    Debug.Log("set FirePart");

                    ps_fuego = child.GetComponent<ParticleSystem>();
                }
            }            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        no_apagando++;
        if (no_apagando == 100) {
            no_apagando = 0;
            if ( onFire() ) startFire();
        }
    }

    public bool hasItem()
    {
        return Holding != null;
    }

    public bool isDoneCooking()
    {
        if (Holding.tag == "Sarten") return Holding.GetComponent<Sarten>().isDoneCooking();
        else return Holding.GetComponent<Olla>().isDoneCooking();
    }

    public GameObject getItem()
    {
        GameObject item = Holding;
        Holding = null;
        //Destroy(c);
        return item;
    }

    public void setItem(GameObject item)
    {
        Holding = item;
        Vector3 newPos = new Vector3(transform.position.x, item.transform.position.y, transform.position.z);
        item.transform.position = newPos;
        if (item.tag != "Sarten") item.transform.rotation = new Quaternion(0f, -90f, 0f, 1f);
        //c = Instantiate(Progressbar, transform);
    }
    public bool hasContainerSpace()
    {
        if (Holding.tag == "Olla") return !Holding.GetComponent<Olla>().isFull();
        else return !Holding.GetComponent<Sarten>().isFull();
    }
    public GameObject getItem2check()
    {
        return Holding;
    }
    public void startFire()
    {
        ps_fuego.Play();
        times = 300;
        LifeTime = 1;
        var main = ps_fuego.main;
        main.startLifetime = LifeTime;
        no_apagando = 0;
    }

    public void stopFire()
    {
        ps_fuego.Stop();
        times = -1;
        no_apagando = 0;
    }

    public void extinguishFire()
    {
        if (ps_fuego.isPlaying && times != 0)
        {
            no_apagando = 0;
            if (times % 100 == 0) {
                LifeTime -= 0.25f;
                var main = ps_fuego.main;
                main.startLifetime = LifeTime;
            }
            times--;
        }
        else if(times == 0)
        {
            stopFire();
        }
        
    }

    public bool onFire()
    {
        Debug.Log("TAG: " + name + " ps_fuego: " + (ps_fuego == null));
        return ps_fuego.isPlaying;
    }

}
