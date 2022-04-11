using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is for any structure built by the player
/// </summary>

public class PlayerStructure : Unit, UIMessageSender {

    public int powerRequirment = 20; //how much power does it need to operate
    [HideInInspector] public int bldgLevel = 1; //the level of the building
    [HideInInspector] public PowerPlant powerSource;//the power plant that provides the power
    [Tooltip ("Max items this building can handle in one order")] 
    public int capacity = 20; //how many units it can create/manufacture
    public int timeToUpgrade = 3; //time it take for an upgrade
    public int energyToOperate = 5; //x units of energy
    public int costOfUpgradeTolevel2 = 500;
    public int costOfUpgradeTolevel3 = 800;
    public Transform musterPoint; //where the units go after manufactured
    public Transform creationPos; //location where the unit tb built by this bldg is instantiated
    public List<GameObject> productionItemsPrefs = new List<GameObject>();
    private int itemsBeingProcessed;
    private List<PlayerUnit> unitsInProcess = new List<PlayerUnit>();
    private MessagingManager mssgManager;

    public virtual void Start() {
        gameObject.layer = LevelManager.manager.playerLayer;
        mssgManager = MessagingManager.manager;
        initialHealth = health;
    }

    /// <summary>
    /// when player clicks on a building, show its stats on UI
    /// </summary>
    public virtual void OnMouseUp() {
        if (!enabled) return; //return if still under costruction

        LevelManager.manager.showBuildingStats(this);
    }

    /// <summary>
    /// get how many units this buillding can still manufacture
    /// </summary>
    /// <returns></returns>
    public int getRemainingCapacity() {
        return capacity - itemsBeingProcessed;
    }

    /// <summary>
    /// create/instantiate a unit
    /// </summary>
    /// <param name="unit"></param> what unit to create
    public void create(PlayerUnit unit) {
        //check if there is a power source plugged to this building
        if (powerSource == null) {
            mssgManager.showToast("a nearby Power Plant is required for operation", 2);
            return;
        }

        //check if the player has money
        if (!PlayerStats.stats.canSpend(unit.unitCost)) { //check if player has enough funds
            mssgManager.showToast(MessagingManager.InsufficientFunds,2);
            return;
        }

        PlayerStats.stats.adjustFund(-unit.unitCost);//take cost

        //create unit
        GameObject newUnit = Instantiate(unit.gameObject, LevelManager.manager.armyParent);
        newUnit.transform.position = creationPos.position;
        newUnit.SetActive(false);
        PlayerUnit nUniSC = newUnit.GetComponent<PlayerUnit>();
        unitsInProcess.Add(nUniSC);
    }

    private void Update() {

        //update time for units in process of dispatching
        if(unitsInProcess.Count > 0) {
            PlayerUnit topUnit = unitsInProcess[0];
            topUnit.RemainigTDispatch -= Time.deltaTime;

            //if unit is ready, dispatch
            if (topUnit.RemainigTDispatch <= 0) {
                topUnit.gameObject.SetActive(true);
                topUnit.moveTo(musterPoint.position);
                unitsInProcess.RemoveAt(0);
            }
        }

     }

    public override void attack() {
        //can't attack
    }

    public override void FixedUpdate() {
        disableObject();
    }

    public override void playAnimation(string name) {
        //add structure animations, if any
    }

    public List<PlayerUnit> getUnitsInProcess() {
        return unitsInProcess;
    }

    //called if player wants to upgrade this building
    public void upgradeAbuildingOnCLick() {
        //check if reacjed max level
        if (bldgLevel >= 3) {
            mssgManager.showToast("Can not upgrade, this building is level 3", 2);
            return;
        }

        //otherwise show message to confirm upgrade
        mssgManager.confirmBuildMssg.showUpgradeConfirm(this, this);
    }

    public void messageResponse(bool response) {
        if (response) {//if player accepted to upgrade
            if (bldgLevel == 1) {
                //check if has money
                if (!PlayerStats.stats.canSpend(costOfUpgradeTolevel2)) {
                    mssgManager.showToast(MessagingManager.InsufficientFunds, 2);
                    return;
                }

                PlayerStats.stats.adjustFund(-costOfUpgradeTolevel2);
                bldgLevel++;
            } else {
                //check if has money
                if (!PlayerStats.stats.canSpend(costOfUpgradeTolevel3)) {
                    mssgManager.showToast(MessagingManager.InsufficientFunds, 2);
                    return;
                }

                PlayerStats.stats.adjustFund(-costOfUpgradeTolevel3);
                bldgLevel++;
            }

            //start the upgrade
            mssgManager.showToast("Upgrade to level "+ bldgLevel + " is underway", 2);
            GetComponent<BuildingConstructionPhase>().startUpgradePhase();
        }
    }

    public float getIntialHealth() {
        return initialHealth;
    }
}
