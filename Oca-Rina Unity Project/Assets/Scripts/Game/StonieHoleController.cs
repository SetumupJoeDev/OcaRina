using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonieHoleController : MonoBehaviour
{

    //Integers\\
    public int stoniesRequired;
    //~~~~~~~~~\\

    //Strings\\
    public string levelName;
    private string wallState;
    //~~~~~~~~\\

    //Sprite Elements\\
    public  Sprite filledSprite;
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer wallSpriteRenderer;
    //~~~~~~~~~~~~~~~~\\

    //Game Logic Controllers\\
    private GameController gameController;
    private PlayerPrefsController playerPrefsController;
    //~~~~~~~~~~~~~~~~~~~~~~~\\

    //Colliders\\
    public BoxCollider2D wallCollider;
    //~~~~~~~~~~\\

    //Particles\\
    public ParticleSystem wallParticles;
    //~~~~~~~~~~\\

    //Audio\\
    public AudioSource wallBreakSound;
    //~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        //Sprite Elements\\
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //~~~~~~~~~~~~~~~~\\

        //Game Logic Controllers\\
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        playerPrefsController = GameObject.Find("PlayerPrefsController").GetComponent<PlayerPrefsController>();
        //~~~~~~~~~~~~~~~~~~~~~~~\\

        if(stoniesRequired == 0)
        {
            //Logs an error if the variable is unassigned as the wall will not work as an obstacle with a value of 0
            Debug.LogError("stoniesRequired is unassigned.");
        }
        //Gets the saved state of the wall to see if the player has already destroyed it
        wallState = playerPrefsController.GetWallState(levelName);
        //If the player has already destroyed the wall, the mechanism is set to its broken state
        if(wallState == "Broken")
        {
            //Sets the wall and hole to their broken and filled states respectively
            spriteRenderer.sprite = filledSprite;
            wallSpriteRenderer.enabled = false;
            wallCollider.enabled = false;
        }

    }

    IEnumerator DestroyWall()
    {
        //Waits half a second to destroy the wall
        yield return new WaitForSeconds(0.5f);
        //Plays the particles and audio for the wall breaking
        wallParticles.Play();
        wallBreakSound.Play();
        //Hides the sprite for the wall and disables collisions with it
        wallSpriteRenderer.enabled = false;
        wallCollider.enabled = false;
        playerPrefsController.UpdateWallState(levelName);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Oca" || collision.gameObject.tag == "Rina")
        {
            if (wallState != "Broken")
            {
                //If the wall isn't already broken, the hint about interaction is shown
                gameController.DisplayStonieHoleHint(stoniesRequired);
            }
            if (Input.GetAxis("Interact") == 1 && gameController.stonieCount >= stoniesRequired)
            {
                //Updates the sprite of the hole to show it filled by a stonie before running the coroutine to destroy the wall
                spriteRenderer.sprite = filledSprite;
                StartCoroutine(DestroyWall());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gameController.HideStonieHoleHint();
    }
}
