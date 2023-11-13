using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandScript : MonoBehaviour
{
    //Different Wand sprites 
    private SpriteRenderer spriteRenderer;
    public Sprite inactive;
    public Sprite flameActive;
    public Sprite bloomActive;
    public Sprite soakActive;
    public Sprite gustActive;
    public Sprite shockActive;
    public Sprite earthquakeActive;
    public Sprite failed;

    //Input values that correspond with wand sprites
    private const int FLAME_ACTIVE = 1;
    private const int BLOOM_ACTIVE = 2;
    private const int SOAK_ACTIVE = 3;
    private const int GUST_ACTIVE = 4;
    private const int SHOCK_ACTIVE = 5;
    private const int EARTHQUAKE_ACTIVE = 6;
    private const int FAILED = 99;

    //Variables to determine whether the wand is active.
    private float wandActiveTime = 1.0f;
    private float totalWandActive;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (totalWandActive > wandActiveTime){
            spriteRenderer.sprite = inactive;
        }

        //If player's input isn't -1(inactive) or 0(no spell), activate the wand.
        if (PlayerInput.input != -1 && PlayerInput.input != 0){
            totalWandActive = 0;

            switch(PlayerInput.input){
            case FLAME_ACTIVE:
                spriteRenderer.sprite = flameActive;
                break;
            case BLOOM_ACTIVE:
                spriteRenderer.sprite = bloomActive;
                break;
            case SOAK_ACTIVE:
                spriteRenderer.sprite = soakActive;
                break;
            case GUST_ACTIVE:
                spriteRenderer.sprite = gustActive;
                break;
            case SHOCK_ACTIVE:
                spriteRenderer.sprite = shockActive;
                break;
            case EARTHQUAKE_ACTIVE:
                spriteRenderer.sprite = earthquakeActive;
                break;
            case FAILED:
                totalWandActive = -2;
                spriteRenderer.sprite = failed;
                break;
            default:
                break;
            }
        }

        //Mirrors the player's movement to the left once the player reaches the boss level
        if (LevelManager.currLevel == 8 && transform.position.x > -6.3){
            transform.position += new Vector3(-0.1f, 0, 0);
        }
        else if (LevelManager.currLevel != 8){
            transform.position = new Vector3(1.7f, -0.7f, 0);   //Wand's starting position is this
        }

        totalWandActive += Time.deltaTime;
    }
}
