using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementController : MonoBehaviour
{
    //Animation\\
    public Animator animator;
    //~~~~~~~~~~\\

    //Booleans\\
    private bool isDown;
    //~~~~~~~~~\\

    //Float\\
    [HideInInspector]
    public float secondsDown;
    //~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        //Animation\\
        animator = gameObject.GetComponent<Animator>();
        //~~~~~~~~~~\\
    }

    private void Update()
    {
        if (isDown)
        {
            secondsDown += Time.deltaTime;
            if(secondsDown >= 3f)
            {
                animator.SetTrigger("MoveUp");      //If the UI element is down, a timer is started. If it isn't reset within 
                secondsDown = 0f;                   //3 seconds, the UI element moves back up and the timer is reset
                isDown = false;
            }
        }
    }

    public void ResetPosition()
    {
        if (isDown)
        {
            secondsDown = 3f;                     //Sets the timer value to 3 so that the UI element will move back up
        }
    }

    public void LowerImage()
    {
        if (!isDown)
        {
            animator.SetTrigger("MoveDown");     //If the UI Element is raised, it is lowered and isDown is set to true to start the timer
            isDown = true;
        }
        else
        {
            secondsDown = 0f;                   //If the image is lowered, the timer is reset to 0=
        }
    }

}
