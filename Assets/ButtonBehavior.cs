using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonBehavior : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject victoryScreen;
    public GameObject gameGuide;
    public GameObject endlessPage;
    public TMP_Text highScore;

    //Gameobjects for the LOCKED BUTTON and PASSWORD functions
    public GameObject inputBox;
    public InputField inputField;   
    public GameObject lockedButt; 

    //Difficulty selection buttons
    public GameObject diffText;
    public GameObject easyMode;
    public GameObject mediumMode;
    public GameObject hardMode;


    private const String password = "5142519";

    void Start(){
        if (PlayerPrefs.GetInt("unlocked", 0) == 1){
            lockedButt.SetActive(false);
        }
    }

    void Update()
    {
        //This object needs to be set active from ANOTHER currently active script.
        //If this script is inactive, it can't run anything.
    }

    //Method that activates the 3 difficulty buttons.
    public void DifficultySelection(){
        diffText.SetActive(true);
        easyMode.SetActive(true);
        mediumMode.SetActive(true);
        hardMode.SetActive(true);
    }

    //A method that starts(or restarts) the first level when one of the difficulty buttons is pressed
    /* param gameDiff - "easy" if easyMode button pressed
     * param gameDiff - "medium" if mediumMode button pressed
     * param gameDiff - "hard" if hardMode button pressed
     */
    public void StartGame(string gameDiff){
        //Set the game's difficulty!
        switch (gameDiff){
            case "easy":
                EasyMode();
                break;
            case "medium":
                MediumMode();
                break;
            case "hard":
                HardMode();
                break;
            default:
                break;
        }

        Debug.Log(gameDiff);
        diffText.SetActive(false);
        easyMode.SetActive(false);
        mediumMode.SetActive(false);
        hardMode.SetActive(false);
        LevelManager.currLevel = 0;
        LevelManager.score = 0;
        LevelManager.currTutorial = 0;
        gameObject.SetActive(false);
    }

    //Helper Function that changes game values to easy mode.
    void EasyMode(){
        ObjectController.DIFF_MULT_SPEED = 0.85f;
        ObjectController.DIFF_MULT_POINTS = 1.2f;
        LevelManager.bossHealth = 5;
        PlayerHealth.health = 20;
    }

    //Helper Function that changes game values to medium mode.
    void MediumMode(){
        ObjectController.DIFF_MULT_SPEED = 1.0f;
        ObjectController.DIFF_MULT_POINTS = 1.0f;
        LevelManager.bossHealth = 7;
        PlayerHealth.health = 15;
    }

    //Helper Function that changes game values to hard mode.
    void HardMode(){
        ObjectController.DIFF_MULT_SPEED = 1.1f;
        ObjectController.DIFF_MULT_POINTS = 1.0f;
        LevelManager.bossHealth = 10;
        PlayerHealth.health = 10;
    }

    //A method that activates the Game Guide canvas. It should show a series of
    //images that explain how the game is played with 2 buttons: one to scroll forward
    //and one to go backward. (And one to go back to the main menu)
    public void toGameGuide(){
        Debug.Log("To Game Guide!");
        gameGuide.SetActive(true);
        mainMenu.SetActive(false);
    }

    //A method that runs when the Main Menu button is pressed
    //Reset current Level to -1(no level), bossHealth to 10
    public void toMainMenu(){
        Debug.Log("To Main Menu!");
        LevelManager.currLevel = -1;
        ObjectController.bossDying = false;
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    //A method that runs when the Endless Mode button on the main menu is pressed
    //This method takes the player to the Endless mode page, which has the 
    //toEndlessMode button as well as the current high score.
    public void toEndlessPage(){
        highScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        endlessPage.SetActive(true);
        gameObject.SetActive(false);
    }

    //A method that runs when the Endless Mode button is pressed
    //This method should start level 10.
    //This method should also set the victory screen to inactive.
    public void toEndlessMode(){
        ObjectController.DIFF_MULT_SPEED = 1.0f;
        ObjectController.DIFF_MULT_POINTS = 1.0f;
        LevelManager.currLevel = 10;
        LevelManager.score = 0;
        PlayerHealth.health = 10;
        LevelManager.canSpawn = true;
        gameObject.SetActive(false);
    }

    public void lockedButton(){
        //Prompt the user with an input field and text asking for the password.
        //If the correct password is entered into the input field, SetActive(false) the lockedButton.
        inputField.text = "";
        inputField.ActivateInputField();
        inputBox.SetActive(true);
    }

    public void lockedButtonPassword(string s){
        if(String.Equals(s.Trim(), password)){
            Debug.Log("Open Sesame!");
            //Remove the lockedButton permanently.
            PlayerPrefs.SetInt("unlocked", 1);
            lockedButt.SetActive(false);
            inputBox.SetActive(false);
        }
        inputField.text = "";
        inputField.ActivateInputField();
    }
}
