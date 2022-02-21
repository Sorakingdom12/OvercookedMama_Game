using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pedidos : MonoBehaviour
{
    public GameObject pedido1, pedido2, pedido3, pedido4, pedido5, pedido6, pedido7, pedido8;
    public GameObject tick, cross;
    public AudioManager au;
    private int num = 0;
    private float width;
    private Vector3 pos;
    private int frames = -1;
    private int index;
    private List<GameObject> pedidos = new List<GameObject>();
    private bool ordenats = true;
    private Vector3 coords = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rt = (RectTransform)pedido1.transform;
        width = rt.rect.width*0.3f*1.2f; //agafem l'amplada del canvas de cada pedido i la multipliquem per l'escala (0.3)
        coords = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (frames >= 0) frames++;
        for(int i = 0; i < pedidos.Count; ++i)
        {
            if (pedidos[i].GetComponent<DeprogressBar>().getTimeLeft() == 0) eliminar_pedido(i);
        }
        num = pedidos.Count;
        if (Input.GetKeyDown("1"))
        {
            if(num < 5) spawn_pedido(1);
        }
        if (Input.GetKeyDown("2"))
        {
            if (num < 5) spawn_pedido(2);
        }
        if (Input.GetKeyDown("3"))
        {
            if (num < 5) spawn_pedido(3);
        }
        if (Input.GetKeyDown("4"))
        {
            if (num < 5) spawn_pedido(4);
        }
        if (Input.GetKeyDown("5"))
        {
            if (num < 5) spawn_pedido(5);
        }
        if (Input.GetKeyDown("6"))
        {
            if (num < 5) spawn_pedido(6);
        }
        if (Input.GetKeyDown("7"))
        {
            if (num < 5) spawn_pedido(7);
        }
        if (Input.GetKeyDown("8"))
        {
            if (num < 5) spawn_pedido(8);
        }
        if (Input.GetKeyDown("0"))
        {
            if(num>0) despawn_pedido(0);
        }     
    }

    public bool getPizza(int i)
    {
        switch (pedidos[i].tag)
        {
            case "Pedido6":
            case "Pedido7":
            case "Pedido8":
                return true;
            default:
                return false;
        }
    }

    public bool getSopa(int i)
    {
        switch (pedidos[i].tag)
        {
            case "Pedido4":
            case "Pedido5":
                return true;
            default:
                return false;
        }
    }

    public void colocar()
    {
        for(int i = 0; i < pedidos.Count; ++i)
        {
            pedidos[i].transform.position = new Vector3(coords.x + width * i, coords.y, coords.z);
        }
        ordenats = true;
    }

    public void spawn_pedido(int i) {
        if (ordenats)
        {
            switch (i)
            {
                case 1:
                    pedidos.Add(Instantiate(pedido1, transform) as GameObject);
                    break;
                case 2:
                    pedidos.Add(Instantiate(pedido2, transform) as GameObject);
                    break;
                case 3:
                    pedidos.Add(Instantiate(pedido3, transform) as GameObject);
                    break;
                case 4:
                    pedidos.Add(Instantiate(pedido4, transform) as GameObject);
                    break;
                case 5:
                    pedidos.Add(Instantiate(pedido5, transform) as GameObject);
                    break;
                case 6:
                    pedidos.Add(Instantiate(pedido6, transform) as GameObject);
                    break;
                case 7:
                    pedidos.Add(Instantiate(pedido7, transform) as GameObject);
                    break;
                case 8:
                    pedidos.Add(Instantiate(pedido8, transform) as GameObject);
                    break;
            }
            pedidos[num].transform.position = new Vector3(coords.x + width * num, coords.y, coords.z);
            num = pedidos.Count;
        }      
    }

    public void despawn_pedido(int i) { //afegir animacions de validació o , ja qué només farem despawn si hem entregat o se'ns ha acabat el temps
        
        pedidos[i].transform.GetChild(0).GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        Instantiate(tick, pedidos[i].transform);
        au.Play("Success");

        int score = pedidos[i].GetComponent<DeprogressBar>().getTimeLeft();
        gameObject.transform.parent.gameObject.GetComponent<GestioEscena>().getScore(score);
        var t = pedidos[i];

        pedidos.Remove(pedidos[i]);
        Destroy(t, 1f);
        StartCoroutine(ExecuteAfterTime(1, i));
    }

    void eliminar_pedido(int i)
    {
        au.Play("Failure");
        //index = i;
        ordenats = false;
        frames = 0;
        gameObject.transform.parent.gameObject.GetComponent<GestioEscena>().getScore(-50);
        pedidos[i].transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        Instantiate(cross, pedidos[i].transform);
        var t = pedidos[i];
        pedidos.Remove(pedidos[i]);
        
        Destroy(t, 1f);
        StartCoroutine(ExecuteAfterTime(1, i));
    }

    public List<string> getIngredients(int p){
        List<string> aux = new List<string>();
        Transform ingreds = pedidos[p].transform.GetChild(0).GetChild(1);
        foreach (Transform ingredient in ingreds)
        {
            aux.Add(ingredient.name);
        }
        return aux;
    }

    public int getPedidosCount()
    {
        return pedidos.Count;
    }

    IEnumerator ExecuteAfterTime(float time, int i)
    {
        yield return new WaitForSeconds(time);
        //num--;
        //shuffle_pedidos(i);
        colocar();
        // Code to execute after the delay
    }

    void shuffle_pedidos(int i) {
        for(int j = i; j < pedidos.Count; j++) {
            pedidos[j].transform.Translate(width*(-1), 0, 0);
        }
        ordenats = true;
    }

    public bool esta_ordenat()
    {
        return ordenats;
    }
}