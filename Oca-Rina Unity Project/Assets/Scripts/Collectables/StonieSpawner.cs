using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StonieSpawner : MonoBehaviour
{
    //Strings\\
    private string stonieUniqueID;
    //~~~~~~~~\\

    //Integers\\
    private int currentStonieState;
    public int stoniesInLevel;
    //~~~~~~~~~\\

    //Game Objects\\
    public GameObject[] stonieMarkers;
    public GameObject[] stonieArray;
    public GameObject stoniePrefab;
    //~~~~~~~~~~~~~\\

    //Game Logic Controllers\\
    private Collectable collectableController;
    private PlayerPrefsController playerPrefsController;
    //~~~~~~~~~~~~~~~~~~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        //Game Objects\\
        stonieArray = new GameObject[stoniesInLevel];
        //~~~~~~~~~~~~~\\

        //Game Logic Controllers\\
        playerPrefsController = GameObject.Find("PlayerPrefsController").GetComponent<PlayerPrefsController>();
        //~~~~~~~~~~~~~~~~~~~~~~~\\

        GenerateStonies();
    }

    private void GenerateStonies()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < stonieMarkers.Length; i++)
        {
            switch (currentSceneIndex)
            {
                case 1:
                    stonieUniqueID = "stonie01" + i;
                    Debug.Log("Hub World. Stonie ID:" + stonieUniqueID);
                    currentStonieState = playerPrefsController.hubWorldStonieState;         //Determines the level the player is on using the current scene index and the ID of the current
                    break;                                                                  //collectable to spawn and retrieves its collected state from the Player Prefs controller
                case 2:
                    stonieUniqueID = "stonie02" + i;
                    Debug.Log("Level One. Stonie ID:" + stonieUniqueID);
                    currentStonieState = playerPrefsController.levelOneStonieState[i];
                    break;
                case 3:
                    stonieUniqueID = "stonie03" + i;
                    currentStonieState = playerPrefsController.levelTwoStonieState[i];
                    break;
                case 4:
                    stonieUniqueID = "stonie04" + i;
                    currentStonieState = playerPrefsController.levelThreeStonieState[i];
                    break;
            }
            if (currentStonieState == 0)
            {
                stonieArray[i] = Instantiate(stoniePrefab, stonieMarkers[i].transform.position, Quaternion.identity);       //If the collectable has not previously been collected, it is spawned
                stonieArray[i].transform.parent = gameObject.transform;                                                     //at the relevant marker's position and adds it to the array of stonies
                collectableController = stonieArray[i].GetComponent<Collectable>();
                collectableController.SetID(i);
            }
        }
    }

}
