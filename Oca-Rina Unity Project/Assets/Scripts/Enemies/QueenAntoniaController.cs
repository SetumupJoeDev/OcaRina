using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenAntoniaController : MonoBehaviour
{
    //Integers\\
    public int queenHealth;
    //~~~~~~~~~\\

    //Floats\\
    private float moveSpeed;
    private float dazedTimer;
    //~~~~~~~\\

    //Booleans\\
    public bool isDazed;
    private bool isDead;
    //~~~~~~~~~\\

    //Vectors\\
    private Vector3 facingRight;
    private Vector3 facingLeft;
    //~~~~~~~~\\

    //GameObjects\\
    private GameObject ocaObject;
    private GameObject rinaObject;
    private GameObject levelExitMarker;
    public GameObject levelExitPrefab;
    //~~~~~~~~~~~~\\

    //Audio\\
    public AudioClip[] hurtSounds;
    public AudioClip deathSound;
    private AudioSource sceneMusic;
    //~~~~~~\\

    //Game Logic Controllers\\
    private OcaController ocaController;
    //~~~~~~~~~~~~~~~~~~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        //Integers\\
        queenHealth = 5;
        //~~~~~~~~~\\

        //Floats\\
        moveSpeed = 2f;
        dazedTimer = 6f;
        //~~~~~~~\\

        //Booleans\\
        isDazed = false;
        isDead = false;
        //~~~~~~~~~\\

        //Vectors\\
        facingRight = new Vector3(0, 0, 0);
        facingLeft = new Vector3(0, -180, 0);
        //~~~~~~~~\\

        //GameObjects\\
        ocaObject = GameObject.Find("Oca");
        rinaObject = GameObject.Find("Rina");
        levelExitMarker = GameObject.Find("LevelExitMarker");
        //~~~~~~~~~~~~\\

        //Audio\\
        sceneMusic = GameObject.Find("SceneMusic").GetComponent<AudioSource>();
        //~~~~~~\\

        //Game Logic Controllers\\
        ocaController = ocaObject.GetComponent<OcaController>();
        //~~~~~~~~~~~~~~~~~~~~~~~\\
    }

    // Update is called once per frame
    void Update()
    {
        //While Queen Antonia is not dazed or dead, she will chase the player around the level
        if (!isDazed && !isDead)
        {
            ChasePlayer();
        }
        else
        {
            //If she is dazed, a timer is started. When it reaches 0, she is no longer dazed and will continue to chase the player
            dazedTimer -= Time.fixedDeltaTime;
            if(dazedTimer <= 0f)
            {
                isDazed = false;
                dazedTimer = 3f;
            }
        }
    }

    IEnumerator WaitToDestroy()
    {
        //Waits two seconds before creating a level exit
        yield return new WaitForSeconds(2);
        GameObject gameExit = Instantiate(levelExitPrefab, levelExitMarker.transform.position, Quaternion.identity);
        gameExit.transform.localScale = new Vector3(2, 2, 1);
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        if(queenHealth > 1)
        {
            isDazed = true;
            queenHealth--;
            Debug.Log(queenHealth);
            int soundIndex = Random.Range(0, hurtSounds.Length);
            AudioSource.PlayClipAtPoint(hurtSounds[soundIndex], transform.position, 1);
        }
        else
        {
            isDead = true;
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 1);
            sceneMusic.Stop();
            StartCoroutine(WaitToDestroy());
        }
    }

    public void ChasePlayer()
    {
        if (ocaController.isActiveCharacter)
        {
            //Moves towards Oca's X position if he is the active character
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(ocaObject.transform.position.x, transform.position.y), moveSpeed * Time.deltaTime);
        }
        else
        {
            //Moves towards Rina's X position if she is the active character
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(rinaObject.transform.position.x, transform.position.y), moveSpeed * Time.deltaTime);
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
