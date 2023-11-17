//Script located in TutorialCanvas

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Necessary for accessing inputField
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public InputField inputField;      //Used to activate the inputField upon level start
    public TMP_Text scoreCounter;      //Displays the player's current score
    public TMP_Text nextLevel;         //Displays the difference between the score and the next level requirement
    public TMP_Text bossHPCounter;     //Displays the Volt Slime's current HP
    public TMP_Text endlessDeathText;  //Message that displays when losing on Endless Mode
    public GameObject victoryCanvas;   //Displays the Victory screen when the player beats the game
    public GameObject deathCanvas;     //Displays the Death Screen when the payer dies
    public GameObject endlessDeathCanvas;  //Displays a specific Screen for when the player dies on endless mode
    public GameObject restartButton;

    static public int currLevel = -1;
    static public int currTutorial = 0;     //Keeps track of the current tutorial to display
    public GameObject[] tutorials;

    private const int LEVEL_1_SCORE = 50;
    private const int LEVEL_2_SCORE = 150;
    private const int LEVEL_3_SCORE = 350;
    private const int LEVEL_4_SCORE = 650;
    private const int LEVEL_5_SCORE = 1200;
    private const int LEVEL_6_SCORE = 1800;
    private const int LEVEL_7_SCORE = 3000;
    private const int LEVEL_8_SCORE = 4000;
    static public int bossHealth = 10;
    private bool bossDying;

    //Level 10(Endless Level) info
    static public float speedMultiplier;    //Necessary to make enemies slowly gain speed in the endless level

    static public int score;
    static public bool canSpawn; //Switch variable to determine whether or not enemies can spawn

    //Variable to determine what state the game is in. States:
    /*
        "Playing - Standard" - Game is active, standard mode. Enemies are moving and spells are active.
        "Playing - Endless" - Game is active, endless mode. Enemies are moving and spells are active. Transitions between other Endless states.
        "Death - Standard" - Player's health reaches 0, standard mode. The game should switch to the GameOverScene, which displays the death screen.
        "Death - Endless" - Player's health reaches 0, endless mode. The game should switch to the GameOverScene, which displays the endless death screen and the score.
        "Paused - Standard" - Game is paused. Enemies remain in place and the standard pause menu shows up.
        "Paused - Endless" - Game is paused. Enemies remain in place and the endless pause menu shows up.
        "Intermission - Standard" - This is the break in between rounds when the tutorials show up. Standard mode only.
        "Victory - Standard" - Boss is dead, player moves to GameOverScene, victory screen.
        "Wait" - This is when the game needs to wait for a few seconds for a cutscene to play or something. Enemies don't move.
        "Menu" - Menu screen, switch to MainMenuScene.
    */
    static public string gameState;
    
    private int tutorialActive;  //0 = tutorial isn't active. # = # more tutorials to display
    private int levelReq;

    void Start(){
        if (currLevel == 0 && currTutorial == 0){               // When currLevel and currTutorial are 0, the game starts. Show the first tutorial.
            Tutorial(3);                                        // (Before game start, currLevel is set to -1)
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        UpdateScore();
        UpdateLevelReq();

        //When the player dies, pause for 1 second, then activate death screen
        if(PlayerHealth.health < 1){
            //TODO: Set the player sprite to be the knocked down sprite, then wait a second before starting the Death screen.
            StartCoroutine(PlayerDies());
        }

        if(currLevel == 10){
            speedMultiplier += 0.000005f;
        }
    }

    /**
     * Processes click inputs for tutorials
     */
    void GetInput()
    {
        if(Input.GetMouseButtonDown(0)){
            //Deactivate the tutorial and recall Tutorial()
            if (tutorialActive != 0){
                foreach (GameObject tutorial in tutorials){
                    tutorial.SetActive(false);
                }
                currTutorial++;
                Tutorial(tutorialActive-1);
            }
        }
    }

    void UpdateScore(){
        if(currLevel != 8){
            if (currLevel != 10){
                nextLevel.gameObject.SetActive(true);
            }
            scoreCounter.gameObject.SetActive(true);
            bossHPCounter.gameObject.SetActive(false);
        }
        scoreCounter.text = "Score: " + score.ToString();
        bossHPCounter.text = "Boss HP: " + bossHealth.ToString();
        //When the score reaches a certain threshold, end the level.
        if (score >= LEVEL_1_SCORE && currLevel == 0){
            EndLevel();
            Tutorial(3);
        }
        if (score >= LEVEL_2_SCORE && currLevel == 1){
            EndLevel();
            Tutorial(3);
        }
        if (score >= LEVEL_3_SCORE && currLevel == 2){
            EndLevel();
            Tutorial(2);
        }
        if (score >= LEVEL_4_SCORE && currLevel == 3){
            EndLevel();
            Tutorial(4);
        }
        if (score >= LEVEL_5_SCORE && currLevel == 4){
            EndLevel();
            Tutorial(3);
        }
        if (score >= LEVEL_6_SCORE && currLevel == 5){
            EndLevel();
            Tutorial(2);
        }
        if (score >= LEVEL_7_SCORE && currLevel == 6){
            EndLevel();
            Tutorial(2);
        }
        if (score >= LEVEL_8_SCORE && currLevel == 7){
            EndLevel();
        }
        if (currLevel == 8 && PlayerInput.inPosition && currTutorial == 22){
            Tutorial(4);
            nextLevel.gameObject.SetActive(false);
            scoreCounter.gameObject.SetActive(false);
            bossHPCounter.gameObject.SetActive(true);
        }
        if (bossHealth < 1 && currLevel == 8 && !bossDying){
            StartCoroutine(bossDead());
            bossDying = true;
        }
    }

    IEnumerator bossDead(){
        gameState = "Wait";
        bossHPCounter.gameObject.SetActive(false);

        //Wait for 4 seconds
        yield return new WaitForSeconds(4);

        EndLevel();
        gameState = "Victory - Standard";
        SceneManager.LoadScene("GameOverScene");
    }

    IEnumerator PlayerDies(){
        gameState = "Wait";
        //TODO: Put player to the knocked down sprite

        yield return new WaitForSeconds(1);

        DeathScreen();
    }

    //Activates the Death Screen. If the level is endless(level 10), deactivate the RESTART button and
    //activate the ENDLESS button. Vice versa otherwise.
    //If the level is endless(level 10), save the score as a high score.
        //Then, activate the EndlessDeath screen.
    //If the level is not endless, deactivate the ENDLESS button and activate the RESTART button.
    void DeathScreen(){
        canSpawn = false;
        if(currLevel == 10){
            if (score > PlayerPrefs.GetInt("HighScore", 0)){
                PlayerPrefs.SetInt("HighScore", score);
                endlessDeathText.text = "New High Score!";
            }
            else{
                endlessDeathText.text = "GAME OVER";
            }

            gameState = "Death - Endless";
        } else {
            gameState = "Death - Standard";
        }
        PlayerHealth.health = 10;
        currLevel = -1;
        SceneManager.LoadScene("GameOverScene");
    }

    //End the level: Stop enemies from spawning
    void EndLevel(){
        canSpawn = false;
        currLevel++;
    }

    //Display the tutorial for that level, then wait for the player to click.
    //This process may repeat several times.
    void Tutorial(int numTutorials){
        gameState = "Intermission - Standard";
        tutorialActive = numTutorials;
        //If numTutorials == 0, then there are no more tutorials to display. Start the next level.
        if (numTutorials == 0){
            StartLevel();
            return;
        }

        tutorials[currTutorial].SetActive(true);
    }

    void StartLevel(){
        gameState = "Playing - Standard";
        canSpawn = true;
        inputField.ActivateInputField();
    }

    void UpdateLevelReq(){
        switch(currLevel){
            case 0:
                levelReq = LEVEL_1_SCORE - score;
                break;
            case 1:
                levelReq = LEVEL_2_SCORE - score;
                break;
            case 2:
                levelReq = LEVEL_3_SCORE - score;
                break;
            case 3:
                levelReq = LEVEL_4_SCORE - score;
                break;
            case 4:
                levelReq = LEVEL_5_SCORE - score;
                break;
            case 5:
                levelReq = LEVEL_6_SCORE - score;
                break;
            case 6:
                levelReq = LEVEL_7_SCORE - score;
                break;
            case 7:
                levelReq = LEVEL_8_SCORE - score;
                break;
            default:
                levelReq = 0;
                break;
        }
        nextLevel.text = "Next Level: " + levelReq.ToString();
    }
}
