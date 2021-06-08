using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowController : MonoBehaviour
{
    //Floats\\
    public float posXOffset;
    private float maxDistance, distanceToOca;
    //~~~~~~~\\

    //Game Objects\\
    public GameObject connectedBurrow;
    private GameObject ocaObject;
    //~~~~~~~~~~~~~~\\

    //Game Logic Controllers\\
    private OcaController ocaController;
    private GameController gameController;
    //~~~~~~~~~~~~~~~~~~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        //Floats\\
        maxDistance = 1f;
        //~~~~~~~~\\

        //GameObjects\\
        ocaObject = GameObject.Find("Oca");
        //~~~~~~~~~~~~\\

        //Game Logic Controllers\\
        ocaController = ocaObject.GetComponent<OcaController>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        //~~~~~~~~~~~~~~~~~~~~~~~\\

    }

    // Update is called once per frame
    void Update()
    {
        //Calculates the distance between Oca and the burrow
        distanceToOca = Vector3.Distance(ocaObject.transform.position, gameObject.transform.position);
        if(distanceToOca < maxDistance)
        {
            //Displays a text prompt to show players how to use the burrow
            gameController.DisplayBurrowHint();
            if (Input.GetAxis("Interact") == 1)
            {
                if (ocaController.isActiveCharacter)
                {
                    //Sets Oca's position to the position of the linked vent with a small X offset to prevent multiple inputs
                    float xOffset = connectedBurrow.transform.position.x - posXOffset;
                    ocaObject.transform.position = new Vector3(xOffset, connectedBurrow.transform.position.y);
                }
            }
        }
        else
        {
            //Hides the hint about how to travel through the burrows when the player is out of range
            gameController.HideBurrowHint();
        }
    }
}
