using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents an item in the "build" menu. when the player
/// clicks on the "build" button, a menu shows with items (structures)
/// that can be built
/// </summary>
public class BuildMenuItem : MonoBehaviour, UIMessageSender
{
    public Text nameText; //name of item
    public Image thumbnail;
    public GameObject prefab;
    [HideInInspector] public PlayerStructure structure;
    private MessagingManager messagingManager;

    private void Start() {
        //referencing
        messagingManager = MessagingManager.manager;
        structure = prefab.GetComponent<PlayerStructure>();
        nameText.text = structure.unitName;
        thumbnail.sprite = structure.thumbnail;
    }

    /// <summary>
    /// when a player clicks on a structure (an item), show a message to 
    /// confirm the build
    /// </summary>
    public void OnClick() {            
        messagingManager.confirmBuildMssg.showMessage(this, structure);
        MusicManager.manager.playClick(0);
    }

    /// <summary>
    /// the items asks the MessagingManager to show a confirmation message.
    /// This method is a call back method from MessagingManager class, to 
    /// tell the item if the player has confirmed the build
    /// </summary>
    /// <param name="response"></param>
    public void messageResponse(bool response) {
        if (response) { //if player responds positively
            //check if has funds
            if (!PlayerStats.stats.canSpend(structure.unitCost)) {
                messagingManager.showToast(MessagingManager.InsufficientFunds, 2);
                return;
            }

            LevelManager.manager.showBuildingMenu(false); //ask the manager to close build menu
            //start building
            PlayerStats.stats.adjustFund(-structure.unitCost);
            Instantiate(prefab);
        }
    }

}
