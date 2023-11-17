using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public InputField inputField; //defining a reference for an InputField in order to clear it
    static public int input;
    private string[] spells = {"", "flame", "bloom", "soak", "gust", "shock", "earthquake"};

    //Audio sources for the spells
    public AudioSource FlameSound;
    public AudioSource BloomSound;
    public AudioSource SoakSound;
    public AudioSource GustSound;
    public AudioSource ShockSound;
    public AudioSource EarthquakeSound;
    public AudioSource POOFSound;

    //Numbers
    private const int SPELL_FAILED = 99;
    private const int SPELL_INACTIVE = -1;
    
    //Cooldown between spell activations. Should be very low
    private float spellCoolDown = 0.1f;
    private float totalSpellTime;

    //Cooldown for the daze effect
    private float dazedCoolDown = 3f;
    static public float totalDazedTime = 3.1f;   //If totalDazedTime > dazedCoolDown, the player is not dazed. Otherwise, the player is dazed.
    private bool rotated = false;

    //Necessary for making the player look to the right on the boss level
    private SpriteRenderer spriteRenderer;
    public Sprite normalSprite;
    public Sprite bossSprite;
    static public bool inPosition;  //to be used in LevelManager

    void Start(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (totalSpellTime > spellCoolDown){
            input = SPELL_INACTIVE;
            totalSpellTime = 0;
        }

        if (input != SPELL_INACTIVE){
            totalSpellTime += Time.deltaTime;
        }

        if (totalDazedTime > dazedCoolDown && rotated){
            transform.Rotate(new Vector3(0, 0, 90));
            rotated = false;
        }

        if (totalDazedTime <= dazedCoolDown && !rotated){
            transform.Rotate(new Vector3(0, 0, -90));
            rotated = true;
        }

        if (LevelManager.currLevel == 8 && transform.position.x > -8){
            spriteRenderer.sprite = bossSprite;
            transform.position += new Vector3(-0.1f, 0, 0);
        }
        else if (LevelManager.currLevel != 8){
            spriteRenderer.sprite = normalSprite;
            transform.position = new Vector3(0, -0.9f, 0);
        }

        if (transform.position.x <= -8){
            inPosition = true;
        }

        totalDazedTime += Time.deltaTime;
    }

    public void ReadStringInput(string s)
    {
        //String inputs only get processed when the game is active - if gameState starts with "Play"
        if (string.Equals(LevelManager.gameState.Substring(0,4), "Play")){
            if (totalDazedTime <= dazedCoolDown){
                Debug.Log("Uhhhh what?? What did you say?");
            }
            else{
                for(int i = 0;i < spells.Length;i++){
                    if (String.Equals(s.ToLower().Trim(), spells[i]) && input == SPELL_INACTIVE){   // Call player attack function
                        Debug.Log("Casting: " + s + " spell!");

                        //Play a sound depending on the spell activated
                        switch(i){
                            case 1:
                                FlameSound.Play();
                                break;
                            case 2:
                                BloomSound.Play();
                                break;
                            case 3:
                                SoakSound.Play();
                                break;
                            case 4:
                                GustSound.Play();
                                break;
                            case 5:
                                ShockSound.Play();
                                break;
                            case 6:
                                EarthquakeSound.Play();
                                break;
                            default:
                                break;
                        }
                        input = i;
                        break;
                    }
                    else if(input != SPELL_INACTIVE){                                        // Cast another spell before spellCoolDown seconds (0.1 seconds)
                        Debug.Log("I can't cast that fast!!!");
                        break;
                    }
                }
                //If input == -1 at this point, then s does not match any of the valid spells. Set it to SPELL_FAILED
                if (input == SPELL_INACTIVE){
                    Debug.Log("AHHHHHHHH!");
                    input = SPELL_FAILED;
                    //Play a POOF sound
                    POOFSound.Play();
                    totalDazedTime = 0;
                }
            }
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }
}
