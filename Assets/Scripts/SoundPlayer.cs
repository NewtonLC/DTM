using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPlayer : MonoBehaviour
{
    //String Switch variable to determine which sound will be played.
    // "" --> No sound
    static public String soundID;

    //Boss sound effects
    public AudioSource BossFleeSound;
    public AudioSource BossDeathSound;

    // Update is called once per frame
    void Update()
    {
        switch(soundID){
            case "BossFlee":
                BossFleeSound.Play();
                break;
            case "BossDeath":
                BossDeathSound.Play();
                break;
            default:
                break;
        }
        soundID = "";
    }
}
