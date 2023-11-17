using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviour
{
    private const float heightMax = 3;       // how far above y0 the spell can spawn
    private const float heightMin = -4;      // how far below y0 the spell can spawn
    private const float xMin = -10;     // how far to the left of x0 the spell can spawn
    private const float xMax = 10;     // how far to the right of x0 the spell can spawn

    private bool spellActive;

    //Reference to Player gameobject, used to get the position
    public GameObject player;

    //One Prefab gameobject for each spell
    //TODO: MAKE EACH SPELL SPRITE INTO A PREFAB!
    public GameObject FlamePrefab;           
    public GameObject BloomPrefab;
    public GameObject SoakPrefab;
    public GameObject GustPrefab;
    public GameObject ShockPrefab;
    public GameObject EarthquakePrefab;
    public GameObject PoofPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //If the player input is not -1(No spell) or 0(Empty string) AND a spell is not active
        if (PlayerInput.input != -1 && !spellActive){
            spellActive = true;
            //switch statement that calls the right spell
            switch(PlayerInput.input){
                case 1:
                    Flame();
                    break;
                case 2:
                    Bloom();
                    break;
                case 3:
                    Soak();
                    break;
                case 4:
                    Gust();
                    break;
                case 5:
                    Shock();
                    break;
                case 6:
                    Earthquake();
                    break;
                case 99:
                    POOF();
                    break;
                default:
                    break;
            }
        }

        if (PlayerInput.input == -1){
            spellActive = false;
        }
    }

    //Spawns one instance of the spell.
    //Params:
    //xMin and xMax             - spell spawn position(x axis)
    //heightMin and heightMax   - spell spawn position(y axis)
    //Prefab                    - Which spell is being spawned
    void Spawn(float xMin, float xMax, float heightMin, float heightMax, GameObject Prefab)
    {
        // Randomly decides the spawn position.
        float xPos = Random.Range(xMin, xMax);
        float height = Random.Range(heightMin, heightMax);

        // sets spawn position and spawns the enemy
        Vector3 SpawnPos = new Vector3(xPos, height, 0);
        GameObject spawnedSpell = Instantiate(Prefab, SpawnPos, Quaternion.identity);
    }

    //Spawns several Flame Prefabs in response to "flame" spell activation
    //FLAME: Spawn 10 instances in random spots on the field.
    void Flame(){
        for (int i = 0;i < 10;i++){
            Spawn(xMin, xMax, heightMin, heightMax, FlamePrefab);
        }
        Debug.Log("FLAMEEEEE");
    }

    //Spawns several Bloom Prefabs in response to "bloom" spell activation
    //BLOOM: Spawn 10 instances in random spots on the field.
    void Bloom(){
        for (int i = 0;i < 10;i++){
            Spawn(xMin, xMax, heightMin, heightMax, BloomPrefab);
        }
        Debug.Log("BLOOOOOOM");
    }

    //Spawns several Soak Prefabs in response to "soak" spell activation
    //SOAK: Spawn 10 instances in random spots on the field.
    void Soak(){
        for (int i = 0;i < 10;i++){
            Spawn(xMin, xMax, heightMin, heightMax, SoakPrefab);
        }
        Debug.Log("SOAAAKKKK");
    }

    //Spawns several Gust Prefabs in response to "gust" spell activation
    //GUST: Spawn 5 instances randomly near the right of the player, then on the left
    void Gust(){
        for (int i = 0;i < 5;i++){
            Spawn(0, xMax, heightMin, heightMax, GustPrefab);
        }
        for (int i = 0;i < 5;i++){
            Spawn(xMin, 0, heightMin, heightMax, GustPrefab);
        }
        Debug.Log("GUUUUSSST");
    }
    
    //Spawns several Shock Prefabs in response to "shock" spell activation
    //SHOCK: Spawn 10 instances randomly slightly above the field
    void Shock(){
        for (int i = 0;i < 10;i++){
            Spawn(xMin, xMax, heightMin, heightMax, ShockPrefab);
        }
        Debug.Log("SHOOOCCKK");
    }

    //Spawns one Earthquake Prefab in response to "earthquake" spell activation
    void Earthquake(){
        Spawn(0, 0, -1.2f, -1.2f, EarthquakePrefab);
        Shake.start = true;
        Debug.Log("EARTHQUAKEEE");
    }

    //Spawns POOF Prefab over the player in response to an invalid spell
    void POOF(){
        Spawn(player.transform.position.x, player.transform.position.x, player.transform.position.y, player.transform.position.y, PoofPrefab);
    }
}
