using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } // reload the current level

    public void QuitToMenu()
    {
        Debug.Log("Quit !!");
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    } // This method close the current level and load the Menu scene
}
