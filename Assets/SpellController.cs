using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    //Flame spell --> 1
    //Bloom spell --> 2
    //Soak spell --> 3
    //Gust spell --> 4
    //Shock spell --> 5
    //Earthquake spell --> 6
    public int spellID;

    void Awake(){
        //Created empty gameObject to aid in animations.
        GameObject empty = new GameObject("Empty Game Object");
        empty.transform.position = transform.position;
        transform.parent = empty.transform;

        switch(spellID){
            case 1: //The Flame animation lasts 1 second.
                Destroy(gameObject, 1);
                Destroy(empty, 1);
                break;
            case 2: //The Bloom animation lasts 1.5 seconds.
                empty.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0,360));
                Destroy(gameObject, 1.5f);
                Destroy(empty, 1.5f);
                break;
            case 3: //The Soak animation lasts 0.5 seconds.
                Destroy(gameObject, 0.5f);
                Destroy(empty, 0.5f);
                break;
            case 4: //The Gust animation lasts 0.5 seconds.
                if(transform.position.x < 0){
                    empty.transform.localScale = new Vector3(-1, 1, 1);
                }
                Destroy(gameObject, 0.5f);
                Destroy(empty, 0.5f);
                break;
            case 5: //The Shock animation lasts 0.9 seconds.
                Destroy(gameObject, 0.8f);
                Destroy(empty, 0.8f);
                break;
            case 6: //The Earthquake animation lasts 1.5 seconds
                Destroy(gameObject, 1.5f);
                Destroy(empty, 1.5f);
                break;
            case 99:
                Destroy(gameObject, 0.75f);
                Destroy(empty, 0.75f);
                break;
            default:
                break;
        }
    }

    //FixedUpdate is called every frame
    void FixedUpdate()
    {
        
    }
}
