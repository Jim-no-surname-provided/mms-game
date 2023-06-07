using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathScreen;
    public static bool setLevelMenu;
    
    public void Death()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }    

    public void TryAgain()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeLevel(int sceneID)
    {
        Time.timeScale = 1f;
        setLevelMenu = true;
        SceneManager.LoadScene(sceneID);
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }
}
