using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Floats\\
    public float moveSpeed;
    public float invulnerableTime;
    [HideInInspector]
    public float defaultLocalScaleX, defaultLocalScaleY;
    //~~~~~~~\\

    //Rigidbodies\\
    [HideInInspector]
    public Rigidbody2D rigidBody;
    //~~~~~~~~~~~~\\

    //Sprites\\
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    //~~~~~~~~\\

    //Booleans\\
    [HideInInspector]
    public bool canBeHit;
    //~~~~~~~~~\\

    //Animation\\
    [HideInInspector]
    public Animator animator;
    //~~~~~~~~~~\\

    //Game Logic Controllers\\
    [HideInInspector]
    public GameController gameController;
    //~~~~~~~~~~~~~~~~~~~~~~~\\

    //Audio\\
    public AudioClip[] hurtSounds;
    public AudioListener audioListener;
    //~~~~~~\\

    // Start is called before the first frame update
    public void AssignVariables()
    {
        //Floats\\
        defaultLocalScaleX = transform.localScale.x;
        defaultLocalScaleY = transform.localScale.y;
        invulnerableTime = 1f;
        //~~~~~~~\\

        //Bools\\
        canBeHit = true;
        //~~~~~~\\

        //RigidBodies\\
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        //~~~~~~~~~~~~\\

        //Sprites\\
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //~~~~~~~~\\

        //Animation\\
        animator  = gameObject.GetComponent<Animator>();
        //~~~~~~~~~~\\

        //Game Logic Controllers\\
        gameController = FindObjectOfType<GameController>().GetComponent<GameController>();
        //~~~~~~~~~~~~~~~~~~~~~~~\\

        //Audio\\
        audioListener = gameObject.GetComponent<AudioListener>();
        //~~~~~~\\
    }

    public void CharacterControls()
    {
        if(Input.GetAxis("Horizontal") != 0)
        {
            //If there is horizontal input, the amount of input is used as a speed multiplier and the character is moved left or right
            float speedMultiplier = Input.GetAxis("Horizontal");
            rigidBody.velocity = new Vector2(moveSpeed * speedMultiplier * Time.fixedDeltaTime, rigidBody.velocity.y);
            //Starts playing the walking animation
            animator.SetBool("isWalking", true);
            if(Input.GetAxis("Horizontal") < 0)
            {
                //Turns the character to face left
                transform.localScale = new Vector2(defaultLocalScaleX * -1, defaultLocalScaleY);
            }
            else
            {
                //Turns the character to face right
                transform.localScale = new Vector2(defaultLocalScaleX, defaultLocalScaleY);
            }
        }
        else if(Input.GetAxis("Horizontal") == 0)
        {
            //If there is no horizontal input, the walking animation is stopped
            animator.SetBool("isWalking", false);
        }
    }
}
