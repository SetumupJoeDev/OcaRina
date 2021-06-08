using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StickieSpawner : MonoBehaviour
{
    //Strings\\
    private string stickieUniqueID;
    //~~~~~~~~\\

    //Integers\\
    private int currentStickieState;
    //~~~~~~~~~\\

    //Gameobjects\\
    public GameObject[] stickieMarkers;
    public GameObject[] stickieArray;
    public GameObject stickiePrefab;
    //~~~~~~~~~~~~\\

    //Game Logic Controllers\\
    private Collectable collectableController;
    private PlayerPrefsController playerPrefsController;
    //~~~~~~~~~~~~~~~~~~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        stickieArray = new GameObject[50];
        playerPrefsController = GameObject.Find("PlayerPrefsController").GetComponent<PlayerPrefsController>();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < stickieMarkers.Length; i++)
        {
            switch (currentSceneIndex)
            {
                case 2:
                    stickieUniqueID = "stickie02" + i;
                    currentStickieState = playerPrefsController.levelOneStickieState[i];            //Determines the level the player is on using the current scene index and the ID of the current
                    break;                                                                          //collectable to spawn and retrieves its collected state from the Player Prefs controller
                case 3:
                    stickieUniqueID = "stickie03" + i;
                    currentStickieState = playerPrefsController.levelTwoStickieState[i];
                    break;
                case 4:
                    stickieUniqueID = "stickie04" + i;
                    currentStickieState = playerPrefsController.levelThreeStickieState[i];
                    break;
            }
            if (currentStickieState == 0)
            {
                stickieArray[i] = Instantiate(stickiePrefab, stickieMarkers[i].transform.position, Quaternion.identity);        //If the collectable has not previously been collected, it is spawned
                stickieArray[i].transform.parent = gameObject.transform;                                                        //at the relevant marker's position and adds it to the array of stonies
                collectableController = stickieArray[i].GetComponent<Collectable>();
                collectableController.SetID(i);
            }
        }
    }
}
