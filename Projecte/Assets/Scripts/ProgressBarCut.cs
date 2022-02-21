using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarCut : MonoBehaviour
{
    [SerializeField]
    private Image progress;
    [SerializeField]
    public float temps = 30; //30 segons a omplir-se al màxim
    private float amount;//amount que s'ha de omplir cada frame
    private float fill = 0;
    private float time_passed = 0;
    private bool tallant;
    private float temps_actualitzat = 0;
    // Start is called before the first frame update
    void Start()
    {
        tallant = false;
        progress.fillAmount = fill;
    }

    // Update is called once per frame
    void Update()
    {
        if (fill < 1 && tallant) //només s'omplirà si estem tallant i no està plena
        {
            fill = 1- (temps_actualitzat/temps); //quan temps_Actualitzat == 0 fill = 1
        }
        progress.fillAmount = fill;
        if (fill >= 1) quitar_plato();
    }

    public void quitar_plato()
    {
        Destroy(gameObject);
    }

    public void cutting(bool b)
    {
        tallant = b;
    }

    public void setTemps(float f)
    {
        temps = f;
    }

    public void estic_tallant(float f)
    {
        temps_actualitzat = f;
    }
}
