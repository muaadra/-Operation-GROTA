using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class shows the units in a building which are in process
/// </summary>

public class UI_BuildingStats_UnitInProcessItem : MonoBehaviour
{
    public Image progress;
    private PlayerUnit myUnit;

    /// <summary>
    /// update the progress of the unit in process
    /// </summary>
    /// <param name="unit"></param>
    public void updateItem(PlayerUnit unit) {
        myUnit = unit;
        progress.fillAmount = (myUnit.RemainigTDispatch / myUnit.timeToDispatch);
        GetComponent<Image>().sprite = myUnit.thumbnail;
        gameObject.SetActive(true);
    }

}
