using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGuideButtons : MonoBehaviour
{
    public GameObject mainMenu;     //Used to go back to the main menu
    public GameObject[] guides;     //A list of the guides that make up the Game Guide
    public GameObject exitButton;   //Allows the user to exit out of the game guide
    public GameObject prevButton;   //Moves to the previous guide
    public GameObject nextButton;   //Moves to the next guide

    private int currGuide;      //Tracks the current active guide.

    public void toNext(){
        //Swap to the next guide in guides
        guides[currGuide].SetActive(false);
        currGuide++;
        guides[currGuide].SetActive(true);
        UpdateButtons();
    }

    public void toPrev(){
        //Swap to the previous guide in guides
        guides[currGuide].SetActive(false);
        currGuide--;
        guides[currGuide].SetActive(true);
        UpdateButtons();
    }

    public void exitGameGuide(){
        //Swap back to the first guide in guides
        guides[currGuide].SetActive(false);
        currGuide = 0;
        guides[currGuide].SetActive(true);
        UpdateButtons();
        //Return to the main menu and close the game guide
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    //Updates the location and visibility of all buttons depending on the current guide
    private void UpdateButtons(){
        //Case 1: First game guide
            //Have the exit button game object located at x = -345
            //Set exit and next button game objects to active
            //Set prev button game object to inactive
        if (currGuide == 0){
            exitButton.transform.position = new Vector3(-9.3f, -4, 0);
            exitButton.SetActive(true);
            nextButton.SetActive(true);
            prevButton.SetActive(false);
        }
        //Case 2: Last game guide
            //Have the exit button game object located at x = 345
            //Set exit and prev button game objects to active
            //Set next button game object to inactive
        else if (currGuide == guides.Length - 1){
            exitButton.transform.position = new Vector3(9.3f, -4, 0);
            exitButton.SetActive(true);
            prevButton.SetActive(true);
            nextButton.SetActive(false);
        }
        //Case 3: Somewhere in the middle of the guide
            //Set exit button game object to inactive
            //Make sure both prev and next buttons are active
        else {
            exitButton.SetActive(false);
            prevButton.SetActive(true);
            nextButton.SetActive(true);
        }
    }
}
