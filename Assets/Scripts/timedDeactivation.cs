using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// attch this to any object so it deactivates after x seconds
/// </summary>

public class timedDeactivation : MonoBehaviour
{
    public float timeToDeactivate = 2; //after how many seconds to deactivate
    private float timer;

    private void OnEnable() {
        timer = timeToDeactivate;
    }

    // Update is called once per frame
    void Update()
    {
        //start timer
        if (timer >= 0) {
            timer -= Time.deltaTime;
            if (timer < 0) { 
                gameObject.SetActive(false);
            }
        }
        
    }
}
