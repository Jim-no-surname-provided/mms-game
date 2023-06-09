using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeLevelLogic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject InitialMenu;
    [SerializeField] GameObject LevelMenu;

    void Start()
    {
        if(PauseMenu.setLevelMenu || FinishAndDeathScreen.setLevelMenu)
        {
            InitialMenu.SetActive(false);
            LevelMenu.SetActive(true);
            PauseMenu.setLevelMenu = false;
            FinishAndDeathScreen.setLevelMenu = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
