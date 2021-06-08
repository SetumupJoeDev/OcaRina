using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanPush : MonoBehaviour
{

    public int fanPower;

    private RinaController rinaController;

    // Start is called before the first frame update
    void Start()
    {
        rinaController = GameObject.FindGameObjectWithTag("Rina").GetComponent<RinaController>();
        fanPower = 200;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Rina")
        {
            Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();      //Sets Rina's upward force to the value of fanPower to move her upwards into the air
            rigidbody.velocity = new Vector2(0f, fanPower * Time.deltaTime);
        }
    }
}
