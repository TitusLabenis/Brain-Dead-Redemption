using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
    }

    public void Controls()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuScreen");
    }

}
