using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is for the river in level 2. It blocks the units from crossing the river
/// </summary>

public class RiverBlock_Level2 : MonoBehaviour, UIMessageSender
{
    public GameObject newBridge;
    public GameObject oldBridge;
    public GameObject block;

    private PlayerStructure bridge;
    private MessagingManager mssgManager;
    private float constTimer;

    /// <summary>
    /// when player clicks on the floating "info" button by the bridge
    /// show a message to start building the new bridge
    /// </summary>
    private void OnMouseDown() {
        //confirm building a bridge
        mssgManager = MessagingManager.manager;
        bridge = newBridge.GetComponent<PlayerStructure>();
        mssgManager.confirmBuildMssg.showMessage(this, bridge);
    }

    /// <summary>
    /// if player agrees to build the bridge, start construction and remove the 
    /// block
    /// </summary>
    /// <param name="response"></param>
    public void messageResponse(bool response) {
        if (response) {
            //check if has money
            if (!PlayerStats.stats.canSpend(bridge.unitCost)) {
                mssgManager.showToast(MessagingManager.InsufficientFunds, 2);
                return;
            }

            //take the cost
            PlayerStats.stats.adjustFund(-bridge.unitCost);
            BuildingConstructionPhase bldngCons = bridge.GetComponent<BuildingConstructionPhase>();
            bldngCons.fixedLocation = true;
            bridge.gameObject.SetActive(true);
            constTimer = bridge.timeToDispatch;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;

        }
    }

    private void Update() {
        if (constTimer > 0) {
            constTimer -= Time.deltaTime;

            if (constTimer <= 0) {
                MessagingManager.manager.showToast("Bridge construction complete!", 3);
                block.SetActive(false);
                oldBridge.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

}
