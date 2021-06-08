using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinaController : Player
{

    //Integers\\
    [HideInInspector]
    public int followPositionXOffset;
    //~~~~~~~~~\\

    //Booleans\\
    [HideInInspector]
    public bool isActiveCharacter;
    //~~~~~~~~~\\

    //Floats\\
    public float followDelayX, followDelayY;
    private float invertedLocalScaleX;
    private float followPosX, followPosY;
    private float defaultGravityScale;
    //~~~~~~~\\

    //Game Objects\\
    private GameObject ocaObject;
    //~~~~~~~~~~~~~\\

    //Vectors\\
    private Vector2 followVelocity;
    //~~~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        AssignVariables();
        audioListener.enabled = false;

        //Floats\\
        moveSpeed = 100f;
        followPositionXOffset = -1;
        invertedLocalScaleX = -0.5f;
        defaultGravityScale = rigidBody.gravityScale;
        //~~~~~~~\\

        //Booleans\\
        isActiveCharacter = false;
        //~~~~~~~~~\\

        //Game Objects\\
        ocaObject = GameObject.FindGameObjectWithTag("Oca");
        //~~~~~~~~~~~~~\\
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActiveCharacter)
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                followPositionXOffset = 1;
                transform.localScale = new Vector2(invertedLocalScaleX, defaultLocalScaleY);
            }                                                                                   //Flips the direction Rina faces while she is not the active character
            else if (Input.GetAxis("Horizontal") > 0)                                           //Manipulates the local scale to ensure the colliders flip instead of just the sprite
            {
                followPositionXOffset = -1;
                transform.localScale = new Vector2(defaultLocalScaleX, defaultLocalScaleY);
            }
        }
        if (!canBeHit)
        {
            invulnerableTime -= Time.deltaTime;
            if(invulnerableTime <= 0f)
            {                                       //Starts the invulnerability timer and changes the animation layer to show the invulnerability animations
                canBeHit = true;
                invulnerableTime = 1f;
                animator.SetLayerWeight(1, 0f);
            }
        }
        if (Input.GetAxis("Jump") == 1)
        {
            //Reduces Rina's gravity scale so she can glide
            rigidBody.gravityScale = defaultGravityScale / 4;
        }
        else if (Input.GetAxis("Jump") == 0)
        {
            //Resets Rina's gravity scale to its default so she falls properly
            rigidBody.gravityScale = defaultGravityScale;
        }
    }

    private void FixedUpdate()
    {
        if (isActiveCharacter)
        {
            CharacterControls();
        }
        else
        {
            FollowOca();
        }
    }
    
    public void SwapActiveElements()
    {
        //Toggles the rigidbody on or off depending on if Rina is the active character or not
        if (isActiveCharacter)
        {
            //Simulates Rina's physics and renders her sprite in front of Oca's
            rigidBody.simulated = true;
            spriteRenderer.sortingOrder = 5;
            //Enables Rina's audio listener for 3D audio
            audioListener.enabled = true;
        }
        else if (!isActiveCharacter)
        {
            //Stops simulating Rina's physics and renders her sprite behind Oca's
            rigidBody.simulated = false;
            spriteRenderer.sortingOrder = 4;
            //Disables Rina's audio listener so Oca's is the only active one in the scene
            audioListener.enabled = false;
        }
    }

    public void FollowOca()
    {
        //While Rina is following Oca, her coordinates are gradually changed to reach her follow offset position to simulate smooth movement
        followPosX = Mathf.SmoothDamp(transform.position.x, ocaObject.transform.position.x + (0.5f * followPositionXOffset), ref followVelocity.x, followDelayX);
        followPosY = Mathf.SmoothDamp(transform.position.y, ocaObject.transform.position.y + 0.6f, ref followVelocity.y, followDelayY);
        transform.position = new Vector2(followPosX, followPosY);
        //If Rina gets stuck and ends up too far from Oca, her position is set to that of Oca
        if(Vector3.Distance(gameObject.transform.position, ocaObject.transform.position) > 5)
        {
            transform.position = ocaObject.transform.position;
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && canBeHit == true)
        {
            //On collision with the enemy, the player is made invulnerable and the invulnerability animation layer is shown
            canBeHit = false;
            animator.SetLayerWeight(1, 1f);
            //Plays a random hurt sound effect
            int clipToPlay = Random.Range(0, 4);
            AudioSource.PlayClipAtPoint(hurtSounds[clipToPlay], transform.position);
            //Moves the player backwards depending on which way they are facing
            rigidBody.AddForce(new Vector2(10 * transform.localScale.x, 4f));
            gameController.LoseHealth();
        }
    }

}
