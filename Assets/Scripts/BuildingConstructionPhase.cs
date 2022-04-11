using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class handles the construction phase of a building.
/// Handles detecting the appropriate building locations and progress bar
/// </summary>
public class BuildingConstructionPhase : MonoBehaviour
{
    public GameObject plcmntIndicator; //the green/red circle to show if can build here
    private LevelManager manager;
    public Material greenTransparent;
    public Material redTransparent;
    public Transform progressBarPos; //psition of progress bar
    public List<GameObject> levelStars; //stars to show the level of this building, 3 starts = level3
    [HideInInspector] public ProgressBar progBar;
    [HideInInspector] public bool fixedLocation; //to build in any location
    private float placmentRadius; //the area need to construct the building
    private bool constructionStarted; //checks if construction started
    private PlayerStructure structure;
    private float constructionTimer;
    private bool stopFollowMouse;
    private Collider myCollider;
    public GameObject scaffold;
    private float upgradeTimer;


    public void Start()
    {
        //referencing 
        manager = LevelManager.manager;
        myCollider = GetComponent<Collider>();
        structure = GetComponent<PlayerStructure>();
        structure.enabled = false;
        GetComponent<NavMeshObstacle>().enabled = false;
        placmentRadius = plcmntIndicator.transform.localScale.x / 2;
        progBar = Instantiate(manager.healthBarsTemplate, manager.healthBarsTemplate.transform.parent)
            .GetComponent<ProgressBar>();
    }

 
    // Update is called once per frame
    void Update() {
        InitiateConstructionPhase();

        upgradePhase();
    }

    /// <summary>
    /// after a building is instantiated, this method makes the building follow 
    /// the mouse, and if the building is on top of an obstacle, the material of the 
    /// indicator turs res, otherwise green.
    /// Once player right clicks on a suitable location, the building starts the 
    /// progress bar 
    /// </summary>
    private void InitiateConstructionPhase() {
        //if construction hasn't started, means player is still finding 
        //a suitable location
        if (!constructionStarted) {
            //building follow mouse pos
            Vector3 bldgPos = manager.getGroundMousePositon();
            if (bldgPos.x != float.NegativeInfinity && !stopFollowMouse) {
                if (!fixedLocation) {//if based on player selected area, and not fixed location
                    Vector3 buildPos = manager.getGroundMousePositon();
                    transform.position = buildPos;
                }
            }

            // check if mouse is on empty area, and show on indicator
            // if in a suitable location, then start the construction
            if ((isAreaEmpty() && Input.GetMouseButtonDown(0)) || fixedLocation) {
                MessagingManager.manager.showToast("Construction Started!", 2);
                //show dust effect
                manager.placementDust.position = transform.position;
                manager.placementDust.gameObject.SetActive(true);

                MusicManager.manager.playConstructionStarted();//play placement music
                scaffold.SetActive(true);
                constructionTimer = structure.timeToDispatch; //set timer
                plcmntIndicator.SetActive(false); //hide the indicator
                GetComponent<NavMeshObstacle>().enabled = true;
                transform.SetParent(manager.playerStructuresParent);
                constructionStarted = true;
                progBar.show(true, progressBarPos); //show progress bar
                manager.addBuilding(transform);
            } else if (Input.GetMouseButtonDown(0)) { //cancle constr. if left click
                MessagingManager.manager.showToast("Can't build here", 2);
            }

            //cancel construction if player clicked on right mouse, and refund
            if (Input.GetMouseButtonDown(1)) {
                MessagingManager.manager.showToast("Construction was canceled", 2);
                PlayerStats.stats.adjustFund(structure.unitCost); //refund
                Destroy(gameObject);
            }
        } else if (constructionTimer > 0) { //start construction
            constructionTimer -= Time.deltaTime;
            progBar.setBar(1 - (constructionTimer / structure.timeToDispatch));

            //construction completed
            if (constructionTimer <= 0) {
                MusicManager.manager.playConstructionCompleted();
                endConstruction();
            }
        }
    }

    /// <summary>
    /// Check if area being selected for construction is empty of obstacles
    /// </summary>
    /// <returns></returns>
    private bool isAreaEmpty() {
        //get all objects intersecting Physics.OverlapSphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, placmentRadius);

        bool canBuild = true;
        foreach (Collider col in hitColliders) {
            //if there is any object within the building radius that is not 
            //of the layer "ground" then it is an obstacle
            if (col.gameObject.layer != manager.groundLayer
                && col != myCollider) {
                canBuild = false;
            }
        }

        //if there are only 2 colliders: this obj and terrain, then allow to build
        if (canBuild) {
            plcmntIndicator.GetComponent<MeshRenderer>().material = greenTransparent;
            return true;
        } else {
            plcmntIndicator.GetComponent<MeshRenderer>().material = redTransparent;
            return false;
        }
    }

    /// <summary>
    /// called when player accepts an upgrade
    /// </summary>
    public void startUpgradePhase() {
        upgradeTimer = structure.timeToUpgrade;
        progBar.show(true, progressBarPos); //show progress bar
        myCollider.enabled = false; //prevents clicking on the building
        scaffold.SetActive(true);
        enabled = true;
    }

    /// <summary>
    /// called by Update, shows updraged progress
    /// </summary>
    private void upgradePhase() {
        if (upgradeTimer > 0) {
            upgradeTimer -= Time.deltaTime;

            progBar.setBar(1 - (upgradeTimer / structure.timeToUpgrade));//adjust progress bar

            if (upgradeTimer <= 0) {
                endConstruction();

                //update change to infrastructure
                manager.infrastructureChangeID++;
            }
        }
    }

    /// <summary>
    /// handles what happens when construction/upgrades is completed
    /// </summary>
    public void endConstruction() {
        scaffold.SetActive(false);
        structure.enabled = true;
        enabled = false;
        myCollider.enabled = true;
        progBar.show(false, null);

        //show the building level
        levelStars[structure.bldgLevel-1].SetActive(true);
    }
}
