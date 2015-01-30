﻿using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
    public string sceneName;

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            if(Input.GetButton("Action")){
                Application.LoadLevel(sceneName);
            }
        }
    }
}
