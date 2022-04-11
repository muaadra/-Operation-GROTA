using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI: thsi shows the stats of a building when the player clicks on it.
/// stats such as, how many units it is processing, its health, its level...etc 
/// </summary>

public class UI_BuildingStats : MonoBehaviour
{
    public Text bldgName;
    public Text health;
    public Text level;
    public Text remainingCapacity;
    public GameObject statsWindow;
    public Transform UIItemsParent;
    public Transform ItemsParent_unitProcessing;
    public GameObject unitsInProsseingWindow;
    public GameObject noItemsTxt;
    private GameObject UIItemPrefab;
    private PlayerStructure buildingOnStats; 
    List<GameObject> UIItemsOnDisplay = new List<GameObject>();


    private void Start() {
        LevelManager.manager.buildingStats = this;
        UIItemPrefab = UIItemsParent.GetChild(0).gameObject;
    }

    /// <summary>
    /// show or hide the stat window of the building
    /// </summary>
    /// <param name="bldg"></param>
    /// <param name="show"></param>
    public void showStats(PlayerStructure bldg, bool show) {
        if (show) {
            //fill UI with building info
            buildingOnStats = bldg;
            bldgName.text = bldg.unitName;
            health.text = ((bldg.getHealth() / (float)bldg.getIntialHealth())*100) + "%";
            level.text = "" + bldg.bldgLevel;
            remainingCapacity.text = "" + bldg.getRemainingCapacity();

            //create items
            createWindowItems(bldg);

            statsWindow.SetActive(true);
        } else {
            buildingOnStats = null;
            statsWindow.SetActive(false);
        }
    }

    /// <summary>
    /// creates UI items of what the building can manufacture
    /// </summary>
    /// <param name="bldg"></param>
    private void createWindowItems(PlayerStructure bldg) {
        //activate the pref
        UIItemPrefab.SetActive(true);

        //delete prevouse building info
        foreach (GameObject item in UIItemsOnDisplay) {
            Destroy(item);
        }

        //create new Items
        UIItemsOnDisplay = new List<GameObject>();
        foreach (GameObject item in bldg.productionItemsPrefs) {
            GameObject UIItm = Instantiate(UIItemPrefab, UIItemsParent);
            UIItemsOnDisplay.Add(UIItm);
            //pass the unit script to the UI element, so it shows the details when 
            //hovered
            UIItm.GetComponent<UI_BuildingStats_Item>().setUnit(item.GetComponent<PlayerUnit>(),bldg);
        }
        //disable the UI item prefab
        UIItemPrefab.SetActive(false);

        //if there are no items the building can manufacture, show no items message
        if (bldg.productionItemsPrefs.Count == 0) {
            noItemsTxt.SetActive(true);
            return;
        } else {
            noItemsTxt.SetActive(false);
        }
    }

    /// <summary>
    /// hid stats when player clicks outside the stats UI
    /// </summary>
    public void statsWindowClickedOutSide() {
        showStats(null, false);
    }

    /// <summary>
    /// when player clicks on upgrade button
    /// </summary>
    public void upgradeAbuildingOnCLick() {
        MusicManager.manager.playClick(0);
        buildingOnStats.upgradeAbuildingOnCLick();
        showStats(null, false);
    }

    private void Update() {
        //show units being processed
        if (buildingOnStats != null) {
            //get the units being processed from the structure processing them
            List<PlayerUnit> units = buildingOnStats.getUnitsInProcess();
            if (units.Count == 0) {
                unitsInProsseingWindow.SetActive(false);
                return;
            } else {
                unitsInProsseingWindow.SetActive(true);
            }
            
            int i = 0;
            for (; i < units.Count; i++) {
                if (ItemsParent_unitProcessing.childCount <= i) {
                    //create new Ui item
                    GameObject UIpref = ItemsParent_unitProcessing.GetChild(0).gameObject;
                    GameObject UIItm = Instantiate(UIpref, ItemsParent_unitProcessing);
                    UIItm.GetComponent<UI_BuildingStats_UnitInProcessItem>().updateItem(units[i]);
                } else {
                    ItemsParent_unitProcessing.GetChild(i)
                        .GetComponent<UI_BuildingStats_UnitInProcessItem>().updateItem(units[i]);
                }
            }

            //disable remaining UI items
            int childCount = ItemsParent_unitProcessing.childCount;
            for (; i < childCount; i++) {
                ItemsParent_unitProcessing.GetChild(i).gameObject.SetActive(false);
            }

        }
    }


}
