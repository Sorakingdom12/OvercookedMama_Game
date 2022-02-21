using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image progress;
    [SerializeField]
    public float temps = 30; //30 segons a omplir-se al màxim
    private float amount;//amount que s'ha de omplir cada frame
    private float fill = 0;
    private float time_passed = 0;
    private GameObject warning;
    private int frames = 0;
    private GameObject c;
    private bool start = false;
    private float temps_actualitzat = 0;
    //public AudioManager au;
    // Start is called before the first frame update
    void Start()
    {
        progress.fillAmount = fill;
        warning = transform.GetChild(2).gameObject;
        c = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(c.transform);
        frames++;
        if (fill < 1) //només s'omplirà si estem tallant i no està plena
        {
            fill = 1 - (temps_actualitzat / temps); //quan temps_Actualitzat == 0 fill = 1
        }
        progress.fillAmount = fill;
        if(fill >= 1)
        {
            foreach (Transform child in transform)
            {
                if (child.name == "Fons") child.gameObject.SetActive(false);
                if (child.name == "Progress") child.gameObject.SetActive(false);
            }
        }
        if (start)
        {
            
            if(frames >= 240)
            {
                //au.Play("Warning");
                bool isactive = warning.activeSelf;
                frames = 0;
                warning.SetActive(!isactive);
            }
        }
        
    }

    public void quitar_plato()
    {
        Destroy(gameObject);
    }

    public void setTemps(float t)
    {
        temps = t;
    }
    public void start_warning(bool b)
    {
        start = b;
    }

    public void estic_cuinant(float f)
    {
        temps_actualitzat = f;
    }
}
