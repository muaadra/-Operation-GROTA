using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class shows all objectives for level 3
/// In this class, you can add/remove objectives and add methods to 
/// check if an objective has been completed
/// </summary>

public class ObjectiveManager_Level3 : MonoBehaviour {
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
        objectives.Add(new Objective("Build 3 EMP Vehicles: Send each to a forcefield generator",
            "EMP Vehicle", 3) {
            checkIfObjectiveIsCompleted = objectiveUnitCount,
        });

        objectives.Add(new Objective("Destroy The Hive Mind","",0) {
            checkIfObjectiveIsCompleted = objective2,
            showCount = false
        });

        //create UI objective Items
        createUIItems();

    }

    /// <summary>
    /// this is to check if unit count objective is completed
    /// </summary>
    /// <param name="obj"></param> the objective to be checked
    public void objectiveUnitCount(Objective obj) {
        int count = getNumberOfUnits(obj.unitName);
        obj.count = count;
        if (count >= obj.requiredCount) {
            obj.completed = true;
        } else {
            obj.completed = false;
        }
    }

    //this is to check if objective is completed
    public void objective2(Objective obj) {
        obj.completed = NexusAI.isNexusDead();
    }

    /// <summary>
    /// checks how many units in the world of a given type
    /// </summary>
    /// <param name="unitName"></param> the name of the unit
    /// <returns></returns> number of all units found
    private int getNumberOfUnits(string unitName) {
        int unitCount = 0;
        PlayerUnit[] units = LevelManager.manager.getUnits().ToArray();
        foreach (PlayerUnit unit in units) {
            if (unit.unitName == unitName) {
                unitCount++;
            }
        }
        return unitCount;
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
 
        }

    }
}
