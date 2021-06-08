using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{

    public float movementDelayX, movementDelayY;

    private float cameraPosX, cameraPosY;

    private GameObject ocaCharacter, rinaCharacter;

    private OcaController ocaController;

    private Vector2 movementVelocity;

    // Start is called before the first frame update
    void Start()
    {
        //Assigns the Oca GameObject to ocaCharacter
        ocaCharacter  = GameObject.FindGameObjectWithTag("Oca");
        //Assigns the Rina GameObject the rinaCharacter
        rinaCharacter = GameObject.FindGameObjectWithTag("Rina");
        //Assigns the script OcaController to ocaController
        ocaController = ocaCharacter.GetComponent<OcaController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ocaController.isActiveCharacter)
        {
            //If the active character is Oca, the camera follows him with a small delay
            cameraPosX = Mathf.SmoothDamp(transform.position.x, ocaCharacter.transform.position.x, ref movementVelocity.x, movementDelayX);
            cameraPosY = Mathf.SmoothDamp(transform.position.y, ocaCharacter.transform.position.y, ref movementVelocity.y, movementDelayY);
        }
        else if (!ocaController.isActiveCharacter)
        {
            //If the active character is Rina, the camera follows her with a small delay
            cameraPosX = Mathf.SmoothDamp(transform.position.x, rinaCharacter.transform.position.x, ref movementVelocity.x, movementDelayX);
            cameraPosY = Mathf.SmoothDamp(transform.position.y, rinaCharacter.transform.position.y, ref movementVelocity.y, movementDelayY);
        }
        transform.position = new Vector3(cameraPosX, cameraPosY, transform.position.z);
    }
}
