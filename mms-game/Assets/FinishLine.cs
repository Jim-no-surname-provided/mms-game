using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("Sie haben das Ziel erreicht!");

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int totalSceneCount = SceneManager.sceneCountInBuildSettings;
            // If there is a next scene
            if(currentSceneIndex < totalSceneCount - 1)
            {
             SceneManager.LoadScene(currentSceneIndex + 1);
             }
             // If there is no next scene, load scene 0
             else
             {
             SceneManager.LoadScene(0);
             }
        }

    }
}
