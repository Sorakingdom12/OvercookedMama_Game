using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeprogressBar : MonoBehaviour
{
    [SerializeField]
    private Image progress;
    [SerializeField]
    public float temps = 120; //120 segons a buidar-se al màxim
    private float amount;//amount que s'ha de omplir cada frame
    private float fill = 0;
    private float time_passed = 0;
    // Start is called before the first frame update
    void Start()
    {
        progress.fillAmount = fill;
    }

    // Update is called once per frame
    void Update()
    {
        time_passed += Time.deltaTime;
        fill = 1 - (time_passed / temps);
        progress.fillAmount = fill;
        if(fill < 0.66 && fill >= 0.33)
        {
            progress.GetComponent<Image>().color = new Color32(255, 85, 0, 255);
        }
        else if(fill < 0.33)
        {
            progress.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }
    }

    public int getTimeLeft()
    {
        return (int)(temps - time_passed);
    }
}
