using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move_scene : MonoBehaviour
{
    public string scene; //escena a la que volem passar
    public AudioManager au;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        string level = SceneManager.GetActiveScene().name;
        switch (level)
        {
            case "Main_Menu":
                au.Stop("MainMenu");
                au.Play("Score");
                break;
            default:
                au.Play("MainMenu");
                break;

        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void change_scene()
    {
        //au.Stop("MainMenu");
        //au.Play("Theme");
        SceneManager.LoadScene(scene);
    }
}