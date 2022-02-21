using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Level Info File")]
    [SerializeField] private TextAsset Recetas;
    [Header("Ingredient Objects")]
    [SerializeField] private GameObject bread;
    [SerializeField] private GameObject cheese;
    [SerializeField] private GameObject mushroom;
    [SerializeField] private GameObject steak;
    [SerializeField] private GameObject lettuce;
    [SerializeField] private GameObject tomato;
    [SerializeField] private GameObject sausage;
    [SerializeField] private GameObject dough;

    private GameObject PauseScreen;
    private GameObject score;
    private GameObject scoreBoard;
    private GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        string level = SceneManager.GetActiveScene().name;
        string info = Recetas.text;
        string[] infolevels = info.Split(';');
        List<int> pedidos = new List<int>();
        switch (level)
        {
            case "Level1":
                foreach (string recipe in infolevels[0].Split(','))
                    pedidos.Add(int.Parse(recipe));
                break;
            case "Level2":
                foreach (string recipe in infolevels[1].Split(','))
                    pedidos.Add(int.Parse(recipe));
                break;
            case "Level3":
                foreach (string recipe in infolevels[2].Split(','))
                    pedidos.Add(int.Parse(recipe));
                break;
            case "Level4":
                foreach (string recipe in infolevels[3].Split(','))
                    pedidos.Add(int.Parse(recipe));
                break;
            case "Level5":
                foreach (string recipe in infolevels[4].Split(','))
                    pedidos.Add(int.Parse(recipe));
                break;
            default:
                foreach (string recipe in infolevels[0].Split(','))
                    pedidos.Add(int.Parse(recipe));
                break;
        }
        UI = GameObject.Find("UIEscena");
        UI.GetComponent<GestioEscena>().setPedidos(pedidos);
        PauseScreen = GameObject.Find("Pause");
        PauseScreen.SetActive(false);
        score = UI.transform.GetChild(2).gameObject;
        scoreBoard = UI.transform.GetChild(4).gameObject;
        scoreBoard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) pause();
        else if (Input.GetKeyDown(KeyCode.Period)) setNextRequieredPlate();
    }    
    public GameObject getItem(string objectName)
    {
        Debug.Log("Ingredient Requested: " + objectName);
        switch (objectName)
        {
            case "bread":
                return bread;
            case "cheese":
                return cheese;
            case "dough":
                return dough;
            case "mushroom":
                return mushroom;
            case "tomato":
                return tomato;
            case "lettuce":
                return lettuce;
            case "steak":
                return steak;
            case "sausage":
                return sausage;
        }
        return bread;
    }
    private void pause()
    {
        Time.timeScale = 0;
        PauseScreen.SetActive(true);
    }
    public void unPause()
    {
        Time.timeScale = 1;
        PauseScreen.SetActive(false);
    }

    public void endGame()
    {
        Time.timeScale = 0;
        scoreBoard.SetActive(true);
        Text scorePoint = scoreBoard.transform.GetChild(5).GetChild(0).GetComponent<Text>();
        Debug.Log(UI.GetComponent<GestioEscena>().getActualScore());
        scorePoint.text = UI.GetComponent<GestioEscena>().getActualScore().ToString();
    }
    private void setNextRequieredPlate()
    {
        GameObject.Find("Player").GetComponent<Player>().setNextCompletedPlate();
    }
}
