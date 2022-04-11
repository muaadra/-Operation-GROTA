using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// shows info about a unit a building can instantiate, part of building stats display
/// </summary>

public class UI_BuildingStats_UnitInfo : MonoBehaviour
{
    public Text unitName;
    public Text unitCost;
    public Text timeToManufacture;
    public Text about;
    public Text attackDistance;
    public Text hitPower;
    public Text hitRate;

    /// <summary>
    /// show unit info on the building stats
    /// </summary>
    /// <param name="unit"></param>
    public void showInfo(PlayerUnit unit) {
        //fill UI with unit info
        unitName.text = unit.unitName;
        about.text = unit.about;
        unitCost.text = "$" + unit.unitCost;
        timeToManufacture.text = unit.timeToDispatch + "sec.";
        unitName.text = unit.unitName;

        //if the unit is of an offensive type
        if (unit.GetComponent<SoldierControls>()) {
            SoldierControls unitS = unit.GetComponent<SoldierControls>();
            attackDistance.text = unitS.attackDistance + "m";
            hitPower.text = unitS.hitPower + "";
            hitRate.text = unitS.reloadTime + "/s";
        }
    }
}
