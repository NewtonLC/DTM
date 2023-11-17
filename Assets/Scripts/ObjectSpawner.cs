using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Spawns objects that fly towards the player
 */
public class ObjectSpawner : MonoBehaviour
{
    private const float heightMax = 3;       // how far above y0 the enemy can spawn
    private const float heightMin = -5;      // how far below y0 the enemy can spawn
    private const float xVariation = 12;     // Where on the x-axis the enemy spawns
    public GameObject Prefab;
    public GameObject Player;

    private float totalEnemyTime; // time elapsed since last enemy spawn
    private float spawnerInterval;      // Randomly chooses between 0.7 and 1.3
    private float spawnRate;            // Rate at which the enemy currently spawns
    private int spawnCount;             // # of enemies that spawn at once

    //Enemy spawn rates and amounts per level:
    private int currLevel;
    public int lvl0Spawn;
    public float lvl0SpawnRate;
    public int lvl1Spawn;
    public float lvl1SpawnRate;
    public int lvl2Spawn;
    public float lvl2SpawnRate;
    public int lvl3Spawn;
    public float lvl3SpawnRate;
    public int lvl4Spawn;
    public float lvl4SpawnRate;
    public int lvl5Spawn;
    public float lvl5SpawnRate;
    public int lvl6Spawn;
    public float lvl6SpawnRate;
    public int lvl7Spawn;
    public float lvl7SpawnRate;
    public int lvl8Spawn;
    public float lvl8SpawnRate;

    //Enemy Spawn info for the endless level.
    //On the endless level, enemies start off spawning slowly(1-10 seconds), then randomly spawn faster and faster(caps at 1 second).
    public int lvl10Spawn;  //Manually set all slimes BUT the boss one to 1. The boss = 0
    private float lvl10SpawnRate = 10;

    void Start()
    {
        UpdateSpawnInfo(lvl0SpawnRate, lvl0Spawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (currLevel != LevelManager.currLevel){
            currLevel = LevelManager.currLevel;
            LevelUpdate(currLevel);
        }

        // spawn (spawnCount) enemies if the time reaches the spawner interval and enemies can spawn.
        // If it's level 10(endless level), spawn at different intervals.
        if(totalEnemyTime > spawnerInterval && LevelManager.canSpawn) 
        {
            for (int i = 0;i < spawnCount;i++){
                Spawn(); 
            }
            totalEnemyTime = 0; // reset time so it can climb back up to the spawnerinterval

            if (LevelManager.currLevel == 10){
                spawnerInterval = Random.Range(1, spawnRate);
            }
            else {
                spawnerInterval = spawnRate * Random.Range(0.7f, 1.3f); // reset spawnerInterval TODO: random 0.7-1.3 * spawnRate
            }
        }

        // Time.deltaTime counts time since last draw frame
        // this is important to keeping timing in the Update method consistent
        // NOTE: if you want to do this in FixedUpdate, you need to use Time.fixedDeltaTime
        totalEnemyTime += Time.deltaTime;

        // If enemies cannot spawn, keep totalEnemyTime at 0(so enemies don't instantly spawn on level start)
        if(!LevelManager.canSpawn){
            totalEnemyTime = 0;
        }

        // If it's level 10(endless level), slowly decrease SpawnRate(min: 1).
        if(LevelManager.currLevel == 10){
            if (spawnRate > 1){
                spawnRate -= 0.00001f;
            }
        }
    }

    void LevelUpdate(int level){
        switch(level){
            case 0:
                UpdateSpawnInfo(lvl0SpawnRate, lvl0Spawn);
                break;
            case 1:
                UpdateSpawnInfo(lvl1SpawnRate, lvl1Spawn);
                break;
            case 2:
                UpdateSpawnInfo(lvl2SpawnRate, lvl2Spawn);
                break;
            case 3:
                UpdateSpawnInfo(lvl3SpawnRate, lvl3Spawn);
                break;
            case 4:
                UpdateSpawnInfo(lvl4SpawnRate, lvl4Spawn);
                break;
            case 5:
                UpdateSpawnInfo(lvl5SpawnRate, lvl5Spawn);
                break;
            case 6:
                UpdateSpawnInfo(lvl6SpawnRate, lvl6Spawn);
                break;
            case 7:
                UpdateSpawnInfo(lvl7SpawnRate, lvl7Spawn);
                break;
            case 8:
                UpdateSpawnInfo(lvl8SpawnRate, lvl8Spawn);
                break;
            case 10:
                UpdateSpawnInfo(lvl10SpawnRate, lvl10Spawn);
                break;
            default:
                break;
        }
    }

    void UpdateSpawnInfo(float lvlSpawnRate, int lvlSpawnCount){
        spawnRate = lvlSpawnRate;
        spawnCount = lvlSpawnCount;
        if (LevelManager.currLevel == 10){
            spawnerInterval = Random.Range(1, spawnRate);
        }
        else {
            spawnerInterval = spawnRate * Random.Range(0.7f, 1.3f);
        }
    }

    void Spawn()
    {
        // Randomly decides the spawn position.
        float xPos = xVariation;
        if (Random.value > 0.5f && LevelManager.currLevel != 8){ //Decides whether the enemy spawns on the left or the right
            xPos = -xPos;
        } 
        float height = Random.Range(heightMin, heightMax);

        // sets spawn position and spawns the enemy
        Vector3 SpawnPos = new Vector3(xPos, height, 0);
        GameObject spawnedEnemy = Instantiate(Prefab, SpawnPos, Quaternion.identity);
        if (xPos < 0){ //Flips the sprite if the enemy spawns on the left
            spawnedEnemy.transform.localScale = new Vector3(-1, 1, 1);
        }
        spawnedEnemy.GetComponent<ObjectController>().Player = Player;
    }
}
