using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OcaController : Player
{
    //Floats\\
    public float jumpPower;
    public float defaultMoveSpeed;
    public float rollSpeed;
    public float rollingJumpPower;
    [HideInInspector]
    public float timeCanRoll;
    private float swapCooldown;
    private float minSwapDist;
    //~~~~~~~\\

    //Booleans\\
    public bool isGrounded;
    public bool hasRolled;
    public bool startSwapCooldown;
    [HideInInspector]
    public bool isActiveCharacter, isRolling;
    //~~~~~~~~~\\

    //Audio\\
    public AudioClip[] jumpSounds;
    //~~~~~~\\

    //Colliders\\
    private PolygonCollider2D polygonCollider;
    private CircleCollider2D circleCollider;
    //~~~~~~~~~~\\

    //UI Elements\\
    private Slider rollTimeSlider;
    private UIElementController sliderController;
    //~~~~~~~~~~~~\\

    //Rina elements\\
    private GameObject rinaObject;
    private RinaController rinaController;
    //~~~~~~~~~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        //Floats\\
        moveSpeed = 150f;
        defaultMoveSpeed = moveSpeed;
        jumpPower = 200f;
        rollSpeed = 225f;
        rollingJumpPower = 75f;
        timeCanRoll = 5f;
        swapCooldown = 1f;
        minSwapDist = 4.5f;
        //~~~~~~~~\\

        //Booleans\\
        isActiveCharacter = true;
        //~~~~~~~~~\\
        
        //Colliders\\
        polygonCollider = gameObject.GetComponent<PolygonCollider2D>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        //~~~~~~~~~~\\

        //UI Elements\\
        rollTimeSlider = GameObject.Find("RollTimeSlider").GetComponent<Slider>();
        sliderController = GameObject.Find("RollTimeFrame").GetComponent<UIElementController>();
        //~~~~~~~~~~~~\\

        //Rina Elements\\
        rinaObject = GameObject.FindGameObjectWithTag("Rina");
        rinaController = rinaObject.GetComponent<RinaController>();
        //~~~~~~~~~~~~~~\\

        AssignVariables();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveCharacter)
        {
            OcaControls();
        }
        else
        {
            //If Oca is not the active character, all of his animation booleans are set to false
            animator.SetBool("isWalking", false);
        }

        if (Input.GetAxis("Swap") == 1 && swapCooldown >= 1)
        {
            float distance = Vector2.Distance(rinaObject.transform.position, transform.position);
            if (isActiveCharacter)
            {
                SwapCharacter();
            }
            else if(!isActiveCharacter && distance <= minSwapDist)
            {
                SwapCharacter();
            }
        }

        if (startSwapCooldown)
        {
            swapCooldown += Time.deltaTime;         //Timer is used to prevent multiple button presses being registered when the swap button is pressed
            if(swapCooldown >= 1f)                  //Prevents multiple character swaps from happening at once
            {
                startSwapCooldown = false;
                swapCooldown = 1f;
            }
        }

    }

    private void SwapCharacter()
    {
        //Starts the character swapping cooldown timer to prevent multiple swaps on one keypress
        swapCooldown = 0f;
        startSwapCooldown = true;
        //Inverts the active character boolean to switch easily between the two characters
        isActiveCharacter = !isActiveCharacter;
        rinaController.isActiveCharacter = !rinaController.isActiveCharacter;
        //Toggles Rina's rigidbody on or off, depending on which character is active
        rinaController.SwapActiveElements();
        if (isActiveCharacter)
        {
            //Enables Oca's audio listener for 3D audio and renders him in front of Rina
            audioListener.enabled = true;
            spriteRenderer.sortingOrder = 5;
        }
        else if (!isActiveCharacter)
        {
            //Disables Oca's audio listener so Rina's is the only active one in the scene
            audioListener.enabled = false;
            spriteRenderer.sortingOrder = 4;
        }
    }

    private void OcaControls()
    {
        if (Input.GetAxis("Roll") == 1 && timeCanRoll >= 1f)
        {
            //Increases Oca's speed value so he will move faster while rolling
            moveSpeed = rollSpeed;
            //Sets the animator boolean isRolling to true
            animator.SetBool("isRolling", true);
            isRolling = true;
            //Disables Oca's walking collider
            polygonCollider.enabled = false;
            //Enables Oca's rolling collider
            circleCollider.enabled = true;
            //Makes Oca invulnerable while he is rolling
            canBeHit = false;
            hasRolled = true;
        }
        if (Input.GetAxis("Roll") == 0 && hasRolled || timeCanRoll <= 0 && hasRolled)
        {
            isRolling = false;
            hasRolled = false;
            //Sets Oca's speed value back to his normal walking pace
            moveSpeed = defaultMoveSpeed;
            //Stops Oca's rolling animation
            animator.SetBool("isRolling", false);
            //Moves Oca upwards so then when he uncurls he isn't stuck in the ground
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
            //Resets Oca's rotation so he doesn't emerge upside down
            transform.localRotation = new Quaternion(0, 0, 0, 0);
            //Disables the collider used when Oca is rolling
            circleCollider.enabled = false;
            //Enables the collider used for when Oca is walking
            polygonCollider.enabled = true;
        }
        if (isRolling && Input.GetAxis("Horizontal") != 0)
        {
            //Creates a local variable used to determine how fast to rotate Oca
            float rotationSpeedModifier = Input.GetAxis("Horizontal");
            //Rotates Oca around the Z axis to make it look like he is rolling
            transform.Rotate(Vector3.back * 360 * rotationSpeedModifier * Time.deltaTime, Space.Self);
        }
        if (Input.GetAxis("Jump") == 1 && isGrounded)
        {
            //Sets isGrounded to false so Oca can only jump once before landing again
            isGrounded = false;
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            //Triggers Oca's melee attack animation
            animator.SetTrigger("meleeAttack");
        }
        if (!canBeHit && !isRolling)
        {
            //Reduces the time Oca has left of invulnerability by the amount of time passed since the last frame
            invulnerableTime -= Time.deltaTime;
            if (invulnerableTime <= 0f)
            {
                //When the amount of time Oca can be invulnerable for runs out, he is now able to take damage, has the timer reset and stops using the invulnerable animation 
                canBeHit = true;
                invulnerableTime = 1f;
                animator.SetLayerWeight(1, 0f);
            }
        }
        if (isRolling)
        {
            RollingTimer();
        }
        else if (!isRolling && timeCanRoll < 5f)
        {
            //Increases the time Oca can roll for by the amount of time passed since the last frame
            timeCanRoll += Time.deltaTime;
            //If the value of timeCanRoll goes over 5, it is set to 5 as this is the maximum value
            if (timeCanRoll > 5f)
            {
                timeCanRoll = 5f;
            }
            //Updates the slider value to reflect how long Oca can roll for
            rollTimeSlider.value = timeCanRoll;
        }
    }

    private void RollingTimer()
    {
        //Moves the roll time slider into view
        sliderController.LowerImage();
        //Reduces the time that Oca can roll for by the amount of time that's passed since the last frame
        timeCanRoll -= Time.deltaTime;
        //Updates the slider to reflect how much time Oca can roll for
        rollTimeSlider.value = timeCanRoll;
    }

    private void FixedUpdate()
    {
        if (isActiveCharacter)
        {
            CharacterControls();
            if (rigidBody.velocity.x == 0)
            {
                //If Oca is no longer moving on the X axis, his animation state is returned to idle
                animator.SetBool("isWalking", false);
            }
            if (rigidBody.velocity.y == 0)
            {
                //If Oca is no longer moving on the Y axis, his animation state is returned to idle or walking
                animator.SetBool("isFalling", false);
                animator.SetBool("isJumping", false);
            }
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
            rigidBody.AddForce(new Vector2(25 * transform.localScale.x, 4f));
            gameController.LoseHealth();
        }
        else if(collision.gameObject.tag == "Enemy" && isRolling)
        {
            TermiteEnemyController termite = collision.gameObject.GetComponent<TermiteEnemyController>();
            termite.Die();
        }
        else if(collision.gameObject.tag == "FinalBoss" && isRolling && transform.position.y > collision.gameObject.transform.position.y)
        {
            QueenAntoniaController queenAntonia = collision.gameObject.GetComponent<QueenAntoniaController>();
            queenAntonia.TakeDamage();
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "FinalBoss")
        {
            //If the player is on top of and riding an enmy, they are knocked off backwards
            rigidBody.AddForce(new Vector2(25 * transform.localScale.x, 12.5f));
        }
    }

    public void Jump()
    {
        if (!isRolling)
        {
            //Increases Oca's Y velocity by his jump power
            rigidBody.AddForce(new Vector2(0, jumpPower));
        }
        else
        {
            //Increases Oca's Y velocity by his jump power, reduced while rolling to prevent strange jump physics
            rigidBody.AddForce(new Vector2(0, rollingJumpPower));
        }
        //Generates a random number between 0 and 10 to select a jump sound from the array and plays it at the player's current position
        int jumpSoundIndex = Random.Range(0, 11);
        AudioSource.PlayClipAtPoint(jumpSounds[jumpSoundIndex], transform.position, 1);
    }
}
