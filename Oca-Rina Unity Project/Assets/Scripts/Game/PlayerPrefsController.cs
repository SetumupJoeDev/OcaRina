using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{
    //Integers\\
    private int currentSceneIndex;
    public int hubWorldStonieState;
    //~~~~~~~~~\\

    //Stonie state arrays\\
    public int[] levelOneStonieState;
    public int[] levelTwoStonieState;
    public int[] levelThreeStonieState;
   //~~~~~~~~~~~~~~~~~~~~~\\

    //Stickie state arrays\\
    public int[] levelOneStickieState;
    public int[] levelTwoStickieState;
    public int[] levelThreeStickieState;
    //~~~~~~~~~~~~~~~~~~~~~\\

    //Collectable ID references\\
    private string stonieReference;
    private string stickieReference;
    //~~~~~~~~~~~~~~~~~~~~~~~~~~\\

    // Start is called before the first frame update
    void Start()
    {
        //Assigns the length of the Stonie state arrays
        levelOneStonieState = new int[5];
        levelTwoStonieState = new int[5];
        levelThreeStonieState = new int[5];
        //Assigns the length of the Stickie state arrays
        levelOneStickieState = new int[50];
        levelTwoStickieState = new int[50];
        levelThreeStickieState = new int[50];
        //Calls the function to determine which collectables in each level should be loaded
        AssignStatesOnStartup();
    }

    public int GetStonieCount()
    {
        return PlayerPrefs.GetInt("stonieCount");
    }

    public int GetStickieCount()
    {
        return PlayerPrefs.GetInt("stickieCount");
    }

    public string GetWallState(string levelName)
    {
        return PlayerPrefs.GetString(levelName);
    }

    public void UpdateWallState(string levelName)
    {
        PlayerPrefs.SetString(levelName, "Broken");
    }

    public void AssignStatesOnStartup()
    {
        hubWorldStonieState = PlayerPrefs.GetInt("Stonie010");

        for(int i = 0; i < levelOneStonieState.Length; i++)
        {
            stonieReference = "stonie02" + i;
            levelOneStonieState[i] = PlayerPrefs.GetInt(stonieReference);           //Gets the collected state of each collectable in each level from the saved PlayerPrefs and assigns the returned
            stonieReference = "stonie03" + i;                                       //value to the appropriate position in the array dependant on the value of i
            levelTwoStonieState[i] = PlayerPrefs.GetInt(stonieReference);
            stonieReference = "stonie04" + i;
            levelThreeStonieState[i] = PlayerPrefs.GetInt(stonieReference);
        }
        for(int j = 0; j < levelOneStickieState.Length; j++)
        {
            stickieReference = "stickie02" + j;
            levelOneStickieState[j] = PlayerPrefs.GetInt(stickieReference);
            stickieReference = "stickie03" + j;
            levelTwoStickieState[j] = PlayerPrefs.GetInt(stickieReference);
            stickieReference = "stickie04" + j;
            levelThreeStickieState[j] = PlayerPrefs.GetInt(stickieReference);
        }
    }

    public void AssignStonieState(int stonieID, int stonieState)
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (currentSceneIndex)
        {
            case 1:
                hubWorldStonieState = stonieState;
                stonieReference = "stonie01" + stonieID;
                break;                                          //Changes the value of the collected stonie in the relevant array to prevent it from being instantiated next time the level is loaded
            case 2:
                levelOneStonieState[stonieID] = stonieState;
                stonieReference = "stonie02" + stonieID;
                break;
            case 3:
                levelTwoStonieState[stonieID] = stonieState;
                stonieReference = "stonie03" + stonieID;
                break;
            case 4:
                levelThreeStonieState[stonieID] = stonieState;
                stonieReference = "stonie04" + stonieID;
                break;
        }
        PlayerPrefs.SetInt(stonieReference, 1);
        PlayerPrefs.Save();
    }

    public void SaveCollectableCount(int stonieCount, int stickieCount)
    {
        PlayerPrefs.SetInt("totalStonies", stonieCount);
        PlayerPrefs.SetInt("totalStickies", stickieCount);
    }
    
    public void AssignStickieState(int stickieID, int stickieState)
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (currentSceneIndex)
        {
            case 2:
                levelOneStickieState[stickieID] = stickieState;
                stickieReference = "stickie02" + stickieID;
                break;                                          //Changes the value of the collected stonie in the relevant array to prevent it from being instantiated next time the level is loaded
            case 3:
                levelTwoStickieState[stickieID] = stickieState;
                stickieReference = "stickie03" + stickieID;
                break;
            case 4:
                levelThreeStickieState[stickieID] = stickieState;
                stickieReference = "stickie04" + stickieID;
                break;
        }
        PlayerPrefs.SetInt(stickieReference, 1);
        PlayerPrefs.Save();
    }

}
