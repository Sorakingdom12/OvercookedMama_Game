using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GestioEscena : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image progress;
    public int minuts;
    public int segons;
    private bool invoking;
    private GameObject pedidos;
    private float temps = 0;
    private bool timeout = false;
    private float time_passed = 0;
    private float fill = 0;
    private float temps_total;
    public TextMeshProUGUI score;
    private List<int> pedidosLevel;
    private int actual_score, score_to_reach;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            if (child.name == "Pedidos") pedidos = child.gameObject;
        }
        temps_total = minuts * 60 + segons;
        actual_score = 0;
        score_to_reach = 0;
        Invoke("newPedido", 2f);
        invoking = true;
    }

    public int checkRecipe(List<string> ings, bool locked)
    {
        List<string> pedIngs;
        int pNum = pedidos.GetComponent<Pedidos>().getPedidosCount();
        bool valid = true;
        for(int i = 0; i < pNum; i++)
        {
            pedIngs = pedidos.GetComponent<Pedidos>().getIngredients(i);
            bool sopa = pedidos.GetComponent<Pedidos>().getSopa(i);
            bool pizza = pedidos.GetComponent<Pedidos>().getPizza(i);
            if ((sopa || pizza) && !locked)
            {
                continue;
            }
            if (pedIngs.Count != ings.Count)
            {
                continue;
            }
            valid = true;
            foreach(string s in pedIngs)
            {
                if (!ings.Contains(s))
                {
                    valid = false;
                    break;
                }
            }
            if(valid)return i;
        }
        return -1;
    }

    public void Deliver(int res)
    {
        pedidos.GetComponent<Pedidos>().despawn_pedido(res);
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeout)
        {
            if (pedidos.GetComponent<Pedidos>().getPedidosCount() <= 4 && !invoking)
            {
                invoking = true;
                Invoke("newPedido", 20f);
            }
            temps += Time.deltaTime;
            time_passed += Time.deltaTime;
            fill = 1 - (time_passed / temps_total);
            progress.fillAmount = fill;
            if (fill < 0.66 && fill >= 0.33) {
                progress.GetComponent<Image>().color = new Color32(255, 85, 0, 255);
            }
            else if (fill < 0.33){
                progress.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            }
            if (temps >= 1)  { //cada segon actualitzem
                temps = 1 - temps;
                segons--;
            }
            if (segons == -1)
            {
                segons = 59;
                minuts--;
            }
            if (minuts == -1)
            {
                minuts = 0;
                segons = 0;
                timeout = true;
            }
            text.text = minuts.ToString("00") + ":" + segons.ToString("00");
        }
        else
        {
            GameObject.Find("GameController").GetComponent<GameController>().endGame();
        }
        if (actual_score < score_to_reach) actual_score++; //augmentem de 1 a 1 la score per tal de que es vegi com puja
        if (actual_score > score_to_reach) actual_score--;
        score.text = actual_score.ToString();
    }

    public void getScore(int i) {
        if (score_to_reach + i >= 0)
        {
            score_to_reach += i;
        }
        else score_to_reach = 0;
    }

    public GameObject getPedidos()
    {
        return gameObject.transform.GetChild(1).gameObject;
    }

    public void setPedidos(List<int> indicePedidos)
    {
        pedidosLevel = indicePedidos;
    }
    public int getActualScore()
    {
        return actual_score;
    }

    private void newPedido()
    {
        invoking = false;
        int num = Random.Range(0, pedidosLevel.Count);
        pedidos.GetComponent<Pedidos>().spawn_pedido(pedidosLevel[num]);
    }
}

