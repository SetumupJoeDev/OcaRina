using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    //Game Logic Controllers\\
    private static DontDestroyOnLoadScript dontDestroy;
    //~~~~~~~~~~~~~~~~~~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        //Prevents this game object and all of its children from being destroyed when new scenes are loaded as they are important for the game to work correctly
        DontDestroyOnLoad(gameObject);

        //Prevents multiples of the object this script is attached to, as well as its children, from existing at once
        //Important to ensure the game flow is not disrupted
        if(dontDestroy == null)
        {
            dontDestroy = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
