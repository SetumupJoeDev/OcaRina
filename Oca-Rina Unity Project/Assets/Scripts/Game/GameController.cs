using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Collectable counts\\
    public int stickieCount;
    public int stonieCount;
    //~~~~~~~~~~~~~~~~~~~\\

    //Player health information\\
    public int playerHealth;
    public int playerLives;
    //~~~~~~~~~~~~~~~~~~~~~~~~~~\\

    //Scene manager information\\
    private int currentSceneIndex;
    //~~~~~~~~~~~~~~~~~~~~~~~~~~\\

    //Audio elements\\
    public AudioClip levelTransitionIn, levelTransitionOut;
    private AudioSource audioSource;
    //~~~~~~~~~~~~~~~\\

    //UI Elements\\
    public Sprite[] healthBarSprites;
    public GameObject hudBars;
    public Canvas uiCanvas;
    public Image healthBar;
    public Text livesCounter, stickieCounter, stonieCounter;
    public Text stonieHoleHint;
    public Text burrowHint;
    public UIElementController healthBarController, collectableCountController, sliderController;
    //~~~~~~~~~~~~\\

    //Game Logic Controllers\\
    private PlayerPrefsController playerPrefsController;
    //~~~~~~~~~~~~~~~~~~~~~~~\\

    //Animation\\
    private Animator levelTransition;
    //~~~~~~~~~~\\


    // Start is called before the first frame update
    void Start()
    {
        //Player Health Information\\
        playerHealth = 3;
        playerLives = 3;
        //~~~~~~~~~~~~~~~~~~~~~~~~~~\\

        //Audio Elements\\
        audioSource = GetComponent<AudioSource>();
        //~~~~~~~~~~~~~~~\\

        //UI Elements\\
        hudBars = GameObject.Find("HUDBars");
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        livesCounter = GameObject.FindGameObjectWithTag("LivesCounter").GetComponent<Text>();
        stickieCounter = GameObject.FindGameObjectWithTag("StickieCounter").GetComponent<Text>();
        stonieCounter = GameObject.FindGameObjectWithTag("StonieCounter").GetComponent<Text>();
        healthBarController = healthBar.GetComponent<UIElementController>();
        collectableCountController = GameObject.FindGameObjectWithTag("CollectableCounter").GetComponent<UIElementController>();
        sliderController = GameObject.Find("RollTimeFrame").GetComponent<UIElementController>();
        stonieHoleHint = GameObject.Find("StonieHoleHint").GetComponent<Text>();
        burrowHint = GameObject.Find("BurrowHint").GetComponent<Text>();
        //~~~~~~~~~~~~\\

        //Game Logic Controllers\\
        playerPrefsController = GameObject.Find("PlayerPrefsController").GetComponent<PlayerPrefsController>();
        //~~~~~~~~~~~~~~~~~~~~~~~\\

        //Animation\\
        levelTransition = GameObject.FindGameObjectWithTag("SceneTransitionPanel").GetComponent<Animator>();
        //~~~~~~~~~~\\
            
        //Hides the HUD so it isn't visible on the main menu
        hudBars.SetActive(false);
        stonieHoleHint.enabled = false;
        burrowHint.enabled = false;
    }

    private void Update()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnApplicationQuit();
        }
    }

    public void DisplayStonieHoleHint(int stoniesRequired)
    {
        stonieHoleHint.text = "Press (interact) to insert Stonies! (" + stoniesRequired + " required)";
        stonieHoleHint.enabled = true;
    }

    public void DisplayBurrowHint()
    {
        burrowHint.enabled = true;
    }

    public void HideBurrowHint()
    {
        burrowHint.enabled = false;
    }

    public void HideStonieHoleHint()
    {
        stonieHoleHint.enabled = false;
    }

    public void GetInitialCounts()
    {
        stonieCount = playerPrefsController.GetStonieCount();
        stickieCount = playerPrefsController.GetStickieCount();
        stonieCounter.text = "x" + stonieCount;
        stickieCounter.text = "x" + stickieCount;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Closing Game");
        Application.Quit();
    }

    IEnumerator WaitToLoad(int levelIndexToLoad)
    {
        //Waits for 2 seconds so that the FadeOut animation can finish before loading a new level and playing the FadeIn animation
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(levelIndexToLoad);
        //Plays the FadeIn sound effect and animation
        audioSource.clip = levelTransitionIn;
        audioSource.Play();
        levelTransition.SetTrigger("FadeIn");
    }

    public void LoseHealth()
    {
        healthBarController.LowerImage();
        if (playerHealth > 1)
        {
            //If the player's health is greater than 0, it is reduced by 1 and the UI's healthbar sprite is changed to reflect the current level of health
            playerHealth--;
            healthBar.sprite = healthBarSprites[playerHealth];
        }
        else
        {
            if (playerLives >= 1)
            {
                //If the player's health is 0, they lose a life and the lives counter is updated to reflect this
                playerLives--;
                livesCounter.text = "x" + playerLives.ToString();
                //Reloads the current scene, placing the player back at the beginning
                LoadNewLevel(currentSceneIndex);
                playerHealth = 3;
                healthBar.sprite = healthBarSprites[playerHealth];
            }
            else
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        LoadMainMenu();
        playerHealth = 3;
        healthBar.sprite = healthBarSprites[playerHealth];  //Loads the game's main menu and resets the player's health and lives
        playerLives = 3;
        livesCounter.text = "x" + playerLives;
    }

    public void UpdateCollectableCount()
    {
        collectableCountController.LowerImage();
        stickieCounter.text = "x" + stickieCount.ToString();        //Updates the number of collectables displayed on the HUD
        stonieCounter.text = "x" + stonieCount.ToString();
        playerPrefsController.SaveCollectableCount(stonieCount, stickieCount);
    }

    public void LoadHubWorld()
    {
        //Moves the UI canvas to be in front of the main menu canvas so that the transitional plane covers the main menu
        uiCanvas.sortingOrder = 1;
        //Plays the audio for transitioning out of the level
        audioSource.clip = levelTransitionOut;
        audioSource.Play();
        //Fades to black to load the next level in the bakground
        levelTransition.SetTrigger("FadeOut");
        //Displays the HUD bars
        hudBars.SetActive(true);
        StartCoroutine(WaitToLoad(1));
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(6);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        //Hides the HUD so it isn't visible on the main menu
        hudBars.SetActive(false);
        stonieHoleHint.enabled = false;
        burrowHint.enabled = false;
    }

    public void LoadNewLevel(int levelIndexToLoad)
    {
        //Plays the FadeOut sound effect and animation
        healthBarController.ResetPosition();
        collectableCountController.ResetPosition();
        sliderController.ResetPosition();
        audioSource.clip = levelTransitionOut;
        audioSource.Play();
        levelTransition.SetTrigger("FadeOut");
        //Starts the level loading coroutine to delay level loading
        StartCoroutine(WaitToLoad(levelIndexToLoad));
    }
}
