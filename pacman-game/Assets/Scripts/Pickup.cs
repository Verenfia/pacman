using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pickup : MonoBehaviour
{
    [SerializeField] float score;
    [SerializeField] bool powerPellet = false;

    GameManager instance;
    void Start() {
        instance = GameManager.instance;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            instance.AddScore(score);
            if(powerPellet){
                //enemy scared
            }
            Destroy(gameObject);
        }
    }
}
