using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// when clicking on a building, the info show with items that can be created in that 
/// building. This class represent an item in the UI to be clicked so the building can create it
/// </summary>

public class UI_BuildingStats_Item : MonoBehaviour
{
    public Text nmbrOfUnitsBeingProccsd;
    public GameObject unitInfoWindow;
    public UI_BuildingStats_UnitInfo sideUnitInfo;
    private PlayerStructure bldg;
    private PlayerUnit myUnit;
    private int processingCount;
    public Image progress;

    /// <summary>
    /// assign the unit to this UI item
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="structure"></param>
    public void setUnit(PlayerUnit unit, PlayerStructure structure) {
        bldg = structure;
        myUnit = unit;
        GetComponent<Image>().sprite = unit.thumbnail;
    }

    /// <summary>
    /// asks the building to create the unit assosiated with this UI item
    /// </summary>
    public void create() {
        MusicManager.manager.playClick(2);
        bldg.create(myUnit);
    }

    /// <summary>
    /// when mouse is above the UI item, show info about it
    /// </summary>
    public void mouseEnter(bool enter) {
        MusicManager.manager.playClick(1);
        unitInfoWindow.SetActive(enter);

        if (enter) {
            sideUnitInfo.showInfo(myUnit);
        }
    }

    private void Update() {
        //show how many units of this items is being processed
        if(bldg != null) {
            processingCount = 0;
            List <PlayerUnit> unitsInProcess = bldg.getUnitsInProcess();
            for (int i = 0; i < unitsInProcess.Count; i++ ) {
                if (unitsInProcess[i].unitName == myUnit.unitName) {
                    processingCount++;
                }
            }

            nmbrOfUnitsBeingProccsd.text = "X" + processingCount;
     
        }
    }


}
