using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public int id_scene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { }

    public void GoToScene()
    {
        Time.timeScale = 1;
        if (id_scene == 0) Application.Quit();
        else SceneManager.LoadScene(id_scene);
    }

}
