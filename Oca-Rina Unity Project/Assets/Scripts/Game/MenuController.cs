using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    //Game Logic Controllers\\
    public GameController gameController;
    //~~~~~~~~~~~~~~~~~~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        //Game Logic Controllers\\
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        //~~~~~~~~~~~~~~~~~~~~~~~\\
    }

    public void LoadHubWorld()
    {
        gameController.LoadHubWorld();
    }

    public void LoadCreditsScene()
    {
        gameController.LoadCreditsScene();
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}
