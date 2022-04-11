using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for the UI "Build button" used to open the "build" menu
/// </summary>

public class BuildButton : MonoBehaviour
{
    public GameObject buildingMenu; // the "build" menu object


    private void Start() {
        LevelManager.manager.buildBttn = this; //so other objects can refrence this calass
    }

    /// <summary>
    /// called when the user clicks on the button
    /// </summary>
    public void OnClick() {
        showBuildingMenu(!buildingMenu.activeSelf);
        MusicManager.manager.playClick(0); //play click sound
    }

    /// <summary>
    /// show building menu
    /// </summary>
    public void showBuildingMenu(bool show) {
        //a flag to block interaction with the 3d world while interacting with the UI
        LevelManager.manager.UIActive = show; 

        buildingMenu.SetActive(show);
    }
}

