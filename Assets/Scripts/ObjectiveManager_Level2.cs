using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class shows all objectives for level 2
/// In this class, you can add/remove objectives and add methods to 
/// check if an objective has been completed
/// </summary>

public class ObjectiveManager_Level2 : MonoBehaviour {
    public GameObject ObjectivesWindow;
    private LevelManager manager;
    public GameObject UIObjectiveItemTemplate;
    private List<Objective> objectives = new List<Objective>();
    private List<UI_Objective_Item> objectivesUIItems = new List<UI_Objective_Item>();
    private int lastInfrastructureChangeID = -1;
    private int lastUnitChangeID = -1;
    public delegate void checkIfCompleted(Objective obj);
    public GameObject flashingBG;


    void Start() {
        manager = LevelManager.manager;
        manager.objectivesFlashingBg = flashingBG;

        //add objectives
        //objective
        objectives.Add(new Objective("Reach the wall and destroy it","",0) {
            checkIfObjectiveIsCompleted = objective1,
            showCount = false
        });


        //create UI objective Items
        createUIItems();

    }

    /// <summary>
    /// create a UI item in the "objectives" menu/list
    /// </summary>
    private void createUIItems() {
        foreach (Objective item in objectives) {
            GameObject uiItem = Instantiate(UIObjectiveItemTemplate,
                UIObjectiveItemTemplate.transform.parent);
            uiItem.GetComponent<UI_Objective_Item>().myObjective = item;
            objectivesUIItems.Add(uiItem.GetComponent<UI_Objective_Item>());
        }

        //disable template item
        UIObjectiveItemTemplate.SetActive(false);
    }


    //this is to check if objective is completed
    public void objective1(Objective obj) {
        //do not need to check for this level, since player will just 
        //go to next level once wall is destroyed
    }


    /// <summary>
    /// when the 'objectives' button is clicked
    /// </summary>
    public void objectiveBttnClicked() {
        flashingBG.SetActive(false);
        manager.UIActive = !ObjectivesWindow.activeSelf;
        ObjectivesWindow.SetActive(!ObjectivesWindow.activeSelf);
        MusicManager.manager.playClick(0);
    }

    // Update is called once per frame
    void Update() {
        //update objectives if only there has been change to the infrastructure
        //or unit count
        if (lastInfrastructureChangeID != manager.infrastructureChangeID
            || lastUnitChangeID != manager.unitsChangeID) {
            lastInfrastructureChangeID = manager.infrastructureChangeID;
            lastUnitChangeID = manager.unitsChangeID;

            foreach (UI_Objective_Item UIItem in objectivesUIItems) {
                UIItem.updateUIItem();
            }

            //check if all objectives completed
            bool completed = true;
            foreach (Objective objtv in objectives) {
                if (objtv.completed == false) {
                    completed = false;
                    break;
                }
            }

            if (completed) {
                print("*****All objectives have been completed****");
                MessagingManager.manager.storyMssgs1.showStory2();
            }
        }

    }
}
