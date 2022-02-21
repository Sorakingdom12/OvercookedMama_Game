using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    List<GameObject> Content;
    private Animator anim;
    private bool cocinado;
    private bool burn;
    private double counter;
    public GameObject Progressbar;
    private GameObject c;
    private ParticleSystem ps_fuego;
    private int times = 300;
    private float LifeTime = 1;
    private int no_apagando = 0;
    public bool GM_NoCremar = false;
    public AudioManager au;
    private bool playingw;
    public bool isCooking = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Content = new List<GameObject>();
        cocinado = false;
        burn = false;
        counter = 0;
        foreach (Transform child in transform)
        {
            if (child.tag == "Fuego") ps_fuego = child.GetComponent<ParticleSystem>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //si tengo contenido -> cocinar
        if (!isEmpty() && !burn)
        {
            Cocinar();
            if (cocinado && !burn)
            {
                // warning;
            }
        }
        else isCooking = false;
        no_apagando++;
        if (no_apagando == 100)
        {
            no_apagando = 0;
            if (onFire()) startFire();
        }

    }

    public bool isEmpty() { return Content.Count == 0; }

    public bool isDoneCooking() { return cocinado; }

    public bool isBurned() { return burn; }

    public void setItems(List<GameObject> items)
    {
        anim.SetBool("Empty", false);
        foreach (GameObject item in items) {
            Content.Add(item);
            counter += item.GetComponent<Ingredient>().getCookTime();
            transform.GetChild(3).GetComponent<Ingredients>().spawn_ingredient(item.tag);
            item.transform.parent = transform;
            item.SetActive(false);
            
        }
    }

    public List<GameObject> getItems()
    {
        anim.SetBool("Empty", true);
        List<GameObject> items = Content;
        Content = new List<GameObject>();
        counter = 0;
        cocinado = false;
        burn = false;
        foreach (GameObject item in items) { item.SetActive(true); }
        au.Stop("Fire");
        if (playingw) au.Stop("Warning");
        playingw = false;
        clearUI();
        return items;
    }

    private void Cocinar()
    {
        isCooking = true;
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
            if (prog == 0)
            {
                c = Instantiate(Progressbar, transform);
                c.transform.Translate(0f, 2f, 0f);
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
                c.GetComponent<ProgressBar>().start_warning(true);
                if (!playingw)
                {
                    playingw = true;
                    au.Play("Warning");
                }
            }
            else if (!GM_NoCremar)
            {
                startFire(); //Fuego
                burn = true;
                if (playingw) au.Stop("Warning");
                playingw = false;
                au.Play("Fire");
                clearUI();
            }
        }
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
        au.Stop("Fire");
    }

    public void extinguishFire()
    {
        if (ps_fuego.isPlaying && times != 0)
        {
            no_apagando = 0;
            if (times % 100 == 0)
            {
                LifeTime -= 0.25f;
                var main = ps_fuego.main;
                main.startLifetime = LifeTime;
            }
            times--;
        }
        else if (times == 0)
        {
            stopFire();
        }

    }

    public bool onFire()
    {
        return ps_fuego.isPlaying;
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
    }

    public void setCounter(float f)
    {
        counter = f;
        foreach (Transform child in transform)
        {
            if (child.tag == "ProgressBar")
            {
                child.GetComponent<ProgressBar>().estic_cuinant((float)counter);
            }
        }
    }


}
