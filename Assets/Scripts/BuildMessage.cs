using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this class represent the "confimation of build (or upgrade)" message.
/// player clicks on build>building>"confimation of build"
/// </summary>
public class BuildMessage : MonoBehaviour
{
    public Text title;
    public Text buildingName;
    public Text about;
    public Text cost;
    public Text buildBttnTxt;
    public Image thumbnail;
    private UIMessageSender messgSender;

    /// <summary>
    /// this method handles filling the UI with the building info and show the 
    /// message once info is filled
    /// </summary>
    /// <param name="sender"></param> the class the asked the message to show 
    /// <param name="bldg"></param> the building which the message will show info about
    public void showMessage(UIMessageSender sender, PlayerStructure bldg) {
        messgSender = sender; //for call back

        //fill info
        title.text = "Build";
        buildingName.text = bldg.unitName;
        about.text = bldg.about;
        cost.text = "Cost: $" + bldg.unitCost;
        buildBttnTxt.text = "Build";
        thumbnail.sprite = bldg.thumbnail;

        //show message
        gameObject.SetActive(true);
    }

    /// <summary>
    /// this method handles filling the UI with the building info and show the 
    /// upgrade confirmation message once info is filled
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="bldg"></param>
    public void showUpgradeConfirm(UIMessageSender sender, PlayerStructure bldg) {
        messgSender = sender; //for call back

        //fill info
        title.text = "Upgrade to level " + (bldg.bldgLevel+1);
        buildingName.text = bldg.unitName;
        about.text = bldg.about;
        buildBttnTxt.text = "Upgrade";
        thumbnail.sprite = bldg.thumbnail;

        if (bldg.bldgLevel == 1) {
            cost.text = "Cost: $" + bldg.costOfUpgradeTolevel2;
        } else {
            cost.text = "Cost: $" + bldg.costOfUpgradeTolevel3;
        }

        //show message
        gameObject.SetActive(true);

    }

    /// <summary>
    /// if player clicks "OK" on the message, let the sender know
    /// </summary>
    public void OK() {
        messgSender.messageResponse(true);
        gameObject.SetActive(false);
        MusicManager.manager.playClick(2); //play a click sound
    }

    /// <summary>
    /// if player clicks "cancel" on the message, let the sender know
    /// </summary>
    public void cancel() {
        messgSender.messageResponse(false);
        gameObject.SetActive(false);
        MusicManager.manager.playClick(0); //play a click sound
    }
}
