using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for the wall at the end of level 2
/// </summary>

public class WallToNexus_Level2 : MonoBehaviour
{
    public EnemyUnit wallCS;
    private float initHealth;
    public Transform progressBarPos;
    private MeshRenderer bodyMesh;
    [HideInInspector] public ProgressBar progBar;
    private LevelManager manager;
    private float deathTimer = 0.75f;

    private void Start() {
        //referencing
        manager = LevelManager.manager;
        initHealth = wallCS.getHealth();
        progBar = Instantiate(manager.healthBarsTemplate, manager.healthBarsTemplate.transform.parent)
            .GetComponent<ProgressBar>();
        bodyMesh = GetComponent<MeshRenderer>();

        //show progress bar
        progBar.show(true, progressBarPos);
    }

    // Update is called once per frame
    void Update()
    {
        //check health
        if (wallCS.getHealth() < initHealth) {
            float ratio = wallCS.getHealth() / initHealth;
            if (ratio < 0) {
                ratio = 0;
                bodyMesh.enabled = false;
            }
            progBar.setBar(ratio);

            if (wallCS.isDead()) {
                //make disapear after x seconds, just enough time for
                //child explosions
                deathTimer -= Time.deltaTime;
                if (deathTimer <= 0) {
                    progBar.show(false, null);
                    MessagingManager.manager.storyMssgs2.showStory2();
                    gameObject.SetActive(false);
                }
            } 
            
        }
        
    }
}
