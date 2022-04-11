using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class starts the heat wave timer in level 1, it also manages 
/// the heat wave and its damage  
/// </summary>
public class HeatTimer : MonoBehaviour
{
    public float timerInSeconds; //when is the next heat wave 
    private Text timerTxt;
    public GameObject heatVFX; //the heat wave visual effects
    private Transform mainCam;

    private float timer;
    private float endHeatTimer;
    // Start is called before the first frame update
    void Start()
    {
        //referencing 
        timerTxt = GetComponent<Text>();
        mainCam = LevelManager.manager.mainCam.transform;

        //setup timerTxt text
        timerTxt.text = string.Format("{0:00}:{1:00}:{2:00}",
                  ((int)timerInSeconds / 3600), ((int)timerInSeconds / 60), (int)timerInSeconds % 60);
        
        //move vfx close to cam
        heatVFX.transform.position = mainCam.position + mainCam.transform.forward * 2;

    }


    // Update is called once per frame
    void Update()
    {
        //show timer and launch damage when timer == 0
        if (timer > 0) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                timer = 0;
                launchHeatDamge();
            }

            timerTxt.text = string.Format("{0:00}:{1:00}:{2:00}",
                ((int)timer/3600) ,((int)timer/60) , (int)timer%60);
           
        }

        //if heat is in effect, set timer for it to last
        // after endHeatTimer seconds, reset the timer
        if (endHeatTimer > 0) {
            endHeatTimer -= Time.deltaTime;
            if (endHeatTimer <= 0) {
                heatVFX.SetActive(false);
            }

            timer = timerInSeconds;
        }
        
    }

    /// <summary>
    /// This method destroys all units except building with level 3
    /// </summary>
    private void launchHeatDamge() {
        //get all units and hit them with very hi damage
        PlayerUnit[] units = LevelManager.manager.getUnits().ToArray();
        foreach (PlayerUnit unit in units) {
            unit.receiveDamage(99999);
        }

        //get all units and hit them with very hi damage
        Transform[] tr = LevelManager.manager.getbuildings().ToArray();
        foreach (Transform unit in tr) {
            PlayerStructure unt = unit.GetComponent<PlayerStructure>();
            if (unt.bldgLevel != 3) {
                unit.GetComponent<Unit>().receiveDamage(99999);
            }
            
        }
        
        //bring heat cfx close to cam
        heatVFX.transform.position = mainCam.position + mainCam.transform.forward * 2;
        heatVFX.SetActive(true);
        endHeatTimer = 5;
        
        //notify player of the heat
        MessagingManager.manager.showToast("Heat Wave in effect!",3);
    }

    /// <summary>
    /// to start the heatwave
    /// </summary>
    public void startTimer() {
        timer = timerInSeconds;
    }
}
