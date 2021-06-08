using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    //Integers\\
    private int itemID;
    private int isCollected;
    //~~~~~~~~~\\

    //Floats\\
    private float rotationSpeed;
    //~~~~~~~\\

    //Game Logic Controllers\\
    private GameController gameController;
    private PlayerPrefsController playerPrefsController;
    //~~~~~~~~~~~~~~~~~~~~~~~\\

    //Audio Elements\\
    [SerializeField]
    private AudioClip stonieCollected, stickieCollected;
    //~~~~~~~~~~~~~~~\\

    public void Start()
    {
        isCollected = 0;
        rotationSpeed = 45f;
        gameController = FindObjectOfType<GameController>().GetComponent<GameController>();
        playerPrefsController = GameObject.Find("PlayerPrefsController").GetComponent<PlayerPrefsController>();
    }

    public void Update()
    {
        Rotate();
    }

    // Update is called once per frame
    public void Rotate()
    {
        //Rotates the collectable around its Z axis by the amount determined by rotationSpeed
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
    }

    public int GetIsCollected()
    {
        return isCollected;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Oca" || collision.gameObject.tag == "Rina")
        {
            //If the gameObject the script is attached to is a Stickie, it increases the player's Stickie count by 1, plays the Stickie collection jingle and destroys the gameObject
            if(gameObject.tag == "Stickie")
            {
                gameController.stickieCount++;
                AudioSource.PlayClipAtPoint(stickieCollected, transform.position, 1);
                isCollected = 1;
                playerPrefsController.AssignStickieState(itemID, isCollected);
            }
            //Same as the code above, but for the Stonie
            else if(gameObject.tag == "Stonie")
            {
                gameController.stonieCount++;
                AudioSource.PlayClipAtPoint(stonieCollected, transform.position, 1);
                isCollected = 1;
                playerPrefsController.AssignStonieState(itemID, isCollected);
            }
            gameController.UpdateCollectableCount();
            Destroy(gameObject);
        }
    }

    public void SetID(int newID)
    {
        itemID = newID;
    }

    public int GetID()
    {
        return itemID;
    }

}
