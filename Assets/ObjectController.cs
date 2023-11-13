using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
 * Shoots spawned objects towards the player
 */
public class ObjectController : MonoBehaviour
{
    public float speed; //speed the object shoots towards the player
    public GameObject Player;
    public int damage;
    public int points;
    private bool bossFlee;  //Indicates whether the boss is fleeing or attacking
    public static bool bossDying; //Indicates whether the boss death animation is playing

    //Difficulty Multiplier affects the speed at which enemies move and the points that they give.
    //Easy: 0.9, 10% decrease in speed
    //Normal: 1.0, no change
    //Hard: 1.1, 10% increase in speed
    public static float DIFF_MULT_SPEED;

    //Difficulty Multiplier affects the speed at which enemies move and the points that they give.
    //Easy: 1.2, 20% increase in points
    //Normal: 1.0, no change
    //Hard: 1.0, no change
    public static float DIFF_MULT_POINTS;

    private float actualSpeed; //takes in potential modifiers of the speed variable

    //Animator for the boss slime
    public Animator bossAnimator;

    //Leaf slime --> 1
    //Drop slime --> 2
    //Ember slime --> 3
    //Earth slime --> 4
    //Wind slime --> 5
    public int enemyID;

    void Start(){
        actualSpeed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // need to keep a reference to player
        Transform player = Player.transform;

        Vector3 directionTo = player.position - transform.position;
        if (!bossDying){
            // Moves the enemy towards the player every physics frame
            // direction = target - origin;
            if (enemyID == 6 && bossFlee){
                transform.position -= directionTo.normalized * actualSpeed * DIFF_MULT_SPEED;
            }
            else{
                transform.position += directionTo.normalized * actualSpeed * DIFF_MULT_SPEED;
            }
        }

        //If the boss is dying, destroy all non-boss enemies.
        if (bossDying && enemyID != 6){
            Destroy(gameObject);
        }

        //If the enemy reaches the player...
        float d = directionTo.magnitude;
        if (d < 1f) {
            Attack();
        }

        //If the player casts a spell...
        if (PlayerInput.input != 0){
            PlayerAttack(PlayerInput.input);
        }

        //Enemy speed modifiers, may vary depending on the level
        if (LevelManager.currLevel == 3){
            SpeedModify(0.1f);
        }
        if (LevelManager.currLevel == 4){
            if(enemyID == 1 || enemyID == 2){
                SpeedModify(0.2f);
            }
        }
        if (LevelManager.currLevel == 6){
            SpeedModify(0.4f);
        }
        if (LevelManager.currLevel == 7){
            SpeedModify(-0.1f);
        }
        if (LevelManager.currLevel == 10){
            SpeedModify(LevelManager.speedMultiplier);
        }

        //If this instance of the boss has fled, destroy it
        if (transform.position.x > 15){
            bossFlee = false;
            Destroy(gameObject);
        }

        //If the level ends, destroy the enemy
        if (!LevelManager.canSpawn){
            Destroy(gameObject);
        }
    }

    //The enemy attacks the player
    void Attack(){
        //Lower player's health
        if (enemyID != 6 || enemyID == 6 && !bossFlee){
            PlayerHealth.health -= damage;
        }
        //If the enemy is the Volt Slime(Boss), daze the player and flee.
        if (enemyID == 6){
            PlayerInput.totalDazedTime = 0;
            BossFlee();
        }
        else{
            Destroy(gameObject);
        }
    }

    //When the player attacks...
    //Destroy the enemies whose ID matches the spell
    //Give the player points corresponding to the enemy ID * Difficulty Multiplier
    void PlayerAttack(int spell){
        if (spell == enemyID){
            LevelManager.score += (int)Math.Round(points * DIFF_MULT_POINTS);
            if (enemyID == 6 && !bossFlee){
                if (LevelManager.bossHealth <= 1){
                    BossDeath();
                }
                else{
                    BossFlee();
                }
                LevelManager.bossHealth -= 1;
            }
            else if (enemyID != 6){
                Destroy(gameObject);
            }
        }
    }

    //TODO: Add sounds?
    void BossFlee(){
        SoundPlayer.soundID = "BossFlee";
        bossFlee = true;
    }

    //TODO: Add sounds?
    void BossDeath(){
        SoundPlayer.soundID = "BossDeath";
        bossDying = true;
        bossAnimator.SetBool("bossDead", true);
        Destroy(gameObject, 2.5f);
    }

    //Multiplies the enemy's speed by the given percentage
    //If given 0.1f, then enemy gets a 10% speed boost.
    void SpeedModify(float percent){
        actualSpeed = speed * (1 + percent);
    }
}
