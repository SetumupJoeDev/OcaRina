using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    //Game Logic Controllers\\
    private OcaController ocaController;
    //~~~~~~~~~~~~~~~~~~~~~~~\\
    
    // Start is called before the first frame update
    void Start()
    {
        //Game Logic Controllers\\
        ocaController = GameObject.FindGameObjectWithTag("Oca").GetComponent<OcaController>();
        //~~~~~~~~~~~~~~~~~~~~~~~\\
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
            ocaController.isGrounded = true;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
            ocaController.isGrounded = true;
    }
}
