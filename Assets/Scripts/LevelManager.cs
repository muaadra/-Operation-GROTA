using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the manager of the scene/level. The manager holds a list of the
/// units in the scene, some of the common functions use by objects in the scene,
/// and references to some of the important objects in the scene so 
/// they can be access via this manager
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager manager;
    public Camera mainCam;
    public Camera UICam;
    public Transform playerStructuresParent; // the parent of all player building
    public Transform armyParent; // the parent of all player units
    public GameObject healthBarsTemplate; //reference to UI health bar template
    public Transform placementDust; //reference to commonly used fx
    public Canvas UICanvas;
    //selectables are the objects that can be selected by the player
    [HideInInspector] public List<Transform> selectables = new List<Transform>();
    [HideInInspector] public BuildButton buildBttn;
    [HideInInspector] public UI_BuildingStats buildingStats;
    [HideInInspector] public bool UIActive; //flag to tell if UI is covering screen
    [HideInInspector] public int groundLayer;
    [HideInInspector] public int playerLayer;
    [HideInInspector] public int enemyLayer;
    [HideInInspector] public GameObject objectivesFlashingBg; //reference to fx
    [HideInInspector] public SettingsMenu settingsMenu;
    private List<Transform> buildings = new List<Transform>();
    private List<PlayerUnit> playerUnits = new List<PlayerUnit>();
    public int infrastructureChangeID; //and id to let other know if there has been changes to buildings array
    public int unitsChangeID; //id to let other know if there has been changes to units array


    private void Awake() {
        //PlayerPrefs.DeleteAll();

        manager = this; //make this instant static
        groundLayer = LayerMask.NameToLayer("Ground");
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    /// <summary>
    /// show/hide building menu
    /// </summary>
    /// <param name="show"></param>
    public void showBuildingMenu(bool show) {
        buildBttn.showBuildingMenu(show);
    }



    /// <summary>
    /// get the world "ground" point where the player clicked usinf a ray from cam
    /// </summary>
    RaycastHit objHit;
    public Vector3 getGroundMousePositon() {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out objHit, 500, 1 << LayerMask.NameToLayer("Ground"))) {
            return objHit.point;
        }
        return new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
    }

    /// <summary>
    /// move to the next level in the game
    /// </summary>
    public void goToNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    /// <summary>
    /// when plyer clicks on a building, tell UI to show Building stats
    /// </summary>
    /// <param name="bldg"></param> the building that was clicked
    public void showBuildingStats(PlayerStructure bldg) {
        buildingStats.showStats(bldg,true);
    }

    /// <summary>
    /// to keep track of all buildings
    /// </summary>
    /// <param name="bldg"></param> the building to be added
    public void addBuilding(Transform bldg) {
        infrastructureChangeID++;
        buildings.Add(bldg);
    }

    /// <summary>
    /// to keep track of all buildings
    /// </summary>
    /// <param name="bldg"></param> the building to be removed
    public void removeBuilding(Transform bldg) {
        infrastructureChangeID++;
        buildings.Add(bldg);
    }

    /// <summary>
    /// to keep track of all units
    /// </summary>
    /// <param name="unit"></param> the unit
    public void addPlayerUnit(PlayerUnit unit) {
        unitsChangeID++;
        playerUnits.Add(unit);
    }

    /// <summary>
    /// to keep track of all units
    /// </summary>
    /// <param name="unit"></param> the unit
    public void removePlayerUnit(PlayerUnit unit) {
        unitsChangeID++;
        playerUnits.Remove(unit);
    }

    public List<Transform> getbuildings() {
        return buildings;
    }
    public List<PlayerUnit> getUnits() {
        return playerUnits;
    }

    /// <summary>
    /// create a progress bar on the UI for a given transform 
    /// </summary>
    /// <param name="progressBarPos"></param> the transform the progress bar must follow
    /// <returns></returns> returns ProgressBar
    public ProgressBar createAProgressBar(Transform progressBarPos) {
        ProgressBar Pb = Instantiate(healthBarsTemplate, healthBarsTemplate.transform.parent)
            .GetComponent<ProgressBar>();

        Pb.show(true, progressBarPos);
        return Pb;   
    }

}
