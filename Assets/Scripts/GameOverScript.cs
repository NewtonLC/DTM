using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    public GameObject victoryCanvas;        //Displays the Victory screen when the player beats the game
    public GameObject deathCanvas;          //Displays the Death Screen when the payer dies
    public GameObject endlessDeathCanvas;   //Displays a specific Screen for when the player dies on endless mode

    // Start is called before the first frame update
    // On Start, check LevelManager.gameState and put up the corresponding canvas.
    void Start()
    {
        switch(LevelManager.gameState){
            case "Victory - Standard":
                victoryCanvas.SetActive(true);
                break;
            case "Death - Standard":
                deathCanvas.SetActive(true);
                break;
            case "Death - Endless":
                endlessDeathCanvas.SetActive(true);
                break;
        }
    }
}
