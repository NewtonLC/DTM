//Script located in Player prefab

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text HPCounter;

    static public int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHP();
    }

    //Update is called once every frame
    void Update()
    {
        UpdateHP();
        if (LevelManager.currLevel == 9){
            HPCounter.gameObject.SetActive(false);
        }
        else{
            HPCounter.gameObject.SetActive(true);
        }
    }

    void UpdateHP(){
        HPCounter.text = "HP: " + health.ToString();
        //If HP <= 0, GAME OVER
    }
}
