using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private GameObject deathMenu;

    private void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
    }

    public void PlayGame()
    {
        deathMenu.SetActive(false);
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
