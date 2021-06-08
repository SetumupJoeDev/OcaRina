using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    //Integers\\
    public int levelToLoad;
    //~~~~~~~~~\\

    //Colliders\\
    private BoxCollider2D exitCollider;
    //~~~~~~~~~~\\

    //Game Logic Controllers\\
    private GameController gameController;
    //~~~~~~~~~~~~~~~~~~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        //Colliders\\
        exitCollider = gameObject.GetComponent<BoxCollider2D>();
        //~~~~~~~~~~\\

        //Game Logic Controllers\\
        gameController = FindObjectOfType<GameController>().GetComponent<GameController>();
        //~~~~~~~~~~~~~~~~~~~~~~~\\
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Oca" || collision.gameObject.tag == "Rina")
        {
            //Disables the collider so it can't be triggered more than once and loads the level determined by levelToLoad
            exitCollider.enabled = false;
            gameController.LoadNewLevel(levelToLoad);
        }
    }

}
