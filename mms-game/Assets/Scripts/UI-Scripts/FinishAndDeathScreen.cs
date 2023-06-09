using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishAndDeathScreen : MonoBehaviour
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

    public void NextLevel(int sceneID)
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        if(SceneManager.GetActiveScene().buildIndex < 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            setLevelMenu = true;
            SceneManager.LoadScene(sceneID);
        }
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
