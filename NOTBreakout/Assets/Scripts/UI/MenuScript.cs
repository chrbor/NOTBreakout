using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class MenuScript : MonoBehaviour
{
    public static MenuScript menu;

    private void Awake()
    {
        menu = this;
    }


    public void ChangeGameMenu(bool open)
    {
        if (open) gamePause = true;



        if (!open) gamePause = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
