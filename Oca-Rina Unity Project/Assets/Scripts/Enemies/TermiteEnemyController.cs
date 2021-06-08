using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TermiteEnemyController : MonoBehaviour
{
    //Floats\\
    private float basicMoveSpeed;
    private float chasingSpeed;
    private float distanceFromPlayer;
    //~~~~~~~\\

    //Booleans\\
    private bool moveRight;
    //~~~~~~~~~\\

    //Vectors\\
    private Vector3 facingRight;
    private Vector3 facingLeft;
    //~~~~~~~~\\

    //GameObjects\\
    private GameObject ocaObject;
    private GameObject rinaObject;
    public GameObject raycastOrigin;
    //~~~~~~~~~~~~\\

    //Audio\\
    public AudioClip[] deathSounds;
    //~~~~~~\\

    //Game Logic Controllers\\
    private OcaController ocaController;
    //~~~~~~~~~~~~~~~~~~~~~~~\\

    //Raycasting\\
    private RaycastHit2D raycaster;
    //~~~~~~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        //Floats\\
        basicMoveSpeed = 1.5f;
        chasingSpeed = 3f;
        //~~~~~~~\\

        //Vectors\\
        facingRight = new Vector3(0, 0, 0);
        facingLeft = new Vector3(0, -180, 0);
        //~~~~~~~~\\

        //GameObjects\\
        ocaObject = GameObject.Find("Oca");
        rinaObject = GameObject.Find("Rina");
        //~~~~~~~~~~~~\\

        //Game Logic Controllers\\
        ocaController = ocaObject.GetComponent<OcaController>();
        //~~~~~~~~~~~~~~~~~~~~~~~\\

        //Raycasting\\
        raycaster = Physics2D.Raycast(raycastOrigin.transform.position, Vector2.down, 0.95f);
        //~~~~~~~~~~~\\
    }

    // Update is called once per frame
    void Update()
    {
        GetDistanceFromPlayer();
        if(distanceFromPlayer < 5)
        {
            ChasePlayer();
        }
        else
        {
            PatrolArea();
        }
    }

    private void GetDistanceFromPlayer()
    {
        if (ocaController.isActiveCharacter)
        {
            //If Oca is the active character, the distance between the enemy this script is attached to and Oca is calculated and assigned to distanceFromPlayer
            distanceFromPlayer = Vector3.Distance(gameObject.transform.position, ocaObject.transform.position);
        }
        else
        {
            //If Rina is the active character, the distance between the enemy this script is attached to and Rina is calculated and assigned to distanceFromPlayer
            distanceFromPlayer = Vector3.Distance(gameObject.transform.position, rinaObject.transform.position);
        }
    }

    private void PatrolArea()
    {
        //Gets the latest information from the raycast to determine if the termite has reached an edge or an obstacle
        raycaster = Physics2D.Raycast(raycastOrigin.transform.position, Vector2.down, 0.95f);
        //Moves the termite by its basic move speed
        transform.Translate(Vector2.right * basicMoveSpeed * Time.deltaTime);
        //If the enemy runs into an obstacle or the edge of a platform, it flips direction
        if (raycaster.collider == false || raycaster.collider == true && raycaster.collider.tag != "Ground")
        {     
            switch (moveRight)
            {
                case true:
                    //Flips the enemy so it faces and moves left
                    transform.eulerAngles = facingLeft;
                    moveRight = false;
                    break;
                case false:
                    //Flips the enemy so it faces and moves right
                    transform.eulerAngles = facingRight;
                    moveRight = true;
                    break;
            }
        }
    }

    public void Die()
    {
        int soundIndex = Random.Range(0, deathSounds.Length);
        AudioSource.PlayClipAtPoint(deathSounds[soundIndex], transform.position, 1);
        Destroy(gameObject);
    }

    private void ChasePlayer()
    {
        if (ocaController.isActiveCharacter)
        {
            //Moves towards Oca's X position if he is the active character
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(ocaObject.transform.position.x, transform.position.y), chasingSpeed * Time.deltaTime);
        }
        else
        {
            //Moves towards Rina's X position if she is the active character
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(rinaObject.transform.position.x, transform.position.y), chasingSpeed * Time.deltaTime);
        }
        if(transform.position.x < ocaObject.transform.position.x || transform.position.x < rinaObject.transform.position.x)
        {
            //If the enemy is on the left of Oca or Rina, the termite is flipped to face right
            transform.eulerAngles = facingRight;
        }
        if(transform.position.x > ocaObject.transform.position.x || transform.position.x > rinaObject.transform.position.x)
        {
            //If the enemy is on the right of Oca or Rina, the termite is flipped to face left
            transform.eulerAngles = facingLeft;
        }
    }

}
