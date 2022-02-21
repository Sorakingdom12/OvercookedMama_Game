using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsBox : MonoBehaviour
{
    [SerializeField] private string ingredientType;
    private Animator anim;
    GameObject childTapa;
    private bool opened;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        childTapa = transform.GetChild(0).gameObject;
        AnimationClose();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void AnimationOpen()
    {
        childTapa.GetComponent<Animator>().SetBool("Tick", true);
        Invoke("AnimationClose", 1f);
    }

    public void AnimationClose()
    {
        childTapa.GetComponent<Animator>().SetBool("Tick", false);
    }

    public string getIngredientName()
    {
        return ingredientType;
    }
}
