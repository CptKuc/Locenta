using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    } // Method for loading first level
    // This method needs the scenes to be set the Build Settings menu in Unity editor
    // First level is the second scene in Build Settings

    public void QuitGame()
    {
        Debug.Log("Quit !!");
        Application.Quit();
    } // This method stop the app.
}
