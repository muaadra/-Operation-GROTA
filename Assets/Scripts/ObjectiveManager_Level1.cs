using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectiveManager_Level1;

/// <summary>
/// This class shows all objectives for level 1
/// In this class, you can add/remove objectives and add methods to 
/// check if an objective has been completed
/// </summary>

public class ObjectiveManager_Level1 : MonoBehaviour
{
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
        //references
        manager = LevelManager.manager;
        manager.objectivesFlashingBg = flashingBG;

        //add objectives
        //objective
        objectives.Add(new Objective("build a command center",
            "Command Center", 1) {
            checkIfObjectiveIsCompleted = objectiveBuildingCount
        });

        objectives.Add(new Objective("build a power plant to power the command center",
            "Power Plant", 1) {
            checkIfObjectiveIsCompleted = objectiveBuildingCount
        });

        //objective
        objectives.Add(new Objective("build 2 harvesters using the command center",
            "Harvester", 2) {
            checkIfObjectiveIsCompleted = objectiveUnitCount
        });

        //objective
        objectives.Add(new Objective("build a Barracks", "Barrack", 1) {
            checkIfObjectiveIsCompleted = objectiveBuildingCount
        });

        //objective
        objectives.Add(new Objective("train 10 soldiers using the barracks",
            "Soldier", 10) {
            checkIfObjectiveIsCompleted = objectiveUnitCount
        });

        //objective
        objectives.Add(new Objective("train 5 Heavy Gunner using the barracks",
            "Heavy Gunner", 5) {
            checkIfObjectiveIsCompleted = objectiveUnitCount
        });

        //objective
        objectives.Add(new Objective("build a War Factory", "War Factory", 1) {
            checkIfObjectiveIsCompleted = objectiveBuildingCount
        });

        //objective
        objectives.Add(new Objective("build 3 tanks using the war factory",
            "Tank", 3) {
            checkIfObjectiveIsCompleted = objectiveUnitCount
        });

        //objective
        objectives.Add(new Objective("build 2 Rocket Tanks using the war factory",
            "Rocket Tank", 2) {
            checkIfObjectiveIsCompleted = objectiveUnitCount
        });

        //objective
        objectives.Add(new Objective("build 2 Humvees using the war factory",
            "Humvee", 2) {
            checkIfObjectiveIsCompleted = objectiveUnitCount
        });

        //objective
        objectives.Add(new Objective("build 2 Helicopters using the war factory",
            "Helicopter", 2) {
            checkIfObjectiveIsCompleted = objectiveUnitCount
        });

        //objective
        objectives.Add(new Objective("build an EMP Vehicle using the war factory",
            "EMP Vehicle", 1) {
            checkIfObjectiveIsCompleted = objectiveUnitCount
        });

        //objective
        objectives.Add(new Objective("Upgrade all buildings to level3 before the heat wave!",
            "",0) {
            checkIfObjectiveIsCompleted = objectiveBuildingLevel,
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

            //to keep track of ui item
            objectivesUIItems.Add(uiItem.GetComponent<UI_Objective_Item>());
        }

        //disable template item
        UIObjectiveItemTemplate.SetActive(false);
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

    /// <summary>
    /// this is to check if building count objective is completed
    /// </summary>
    /// <param name="obj"></param> the objective to be checked
    public void objectiveBuildingCount(Objective obj) {
        int count = getNumberOfBuildings(obj.unitName);
        obj.count = count;

        if (count >= obj.requiredCount) {
            obj.completed = true;
        } else {
            obj.completed = false;
        }
    }


    /// <summary>
    /// this is to check if level of buildings objective is completed
    /// </summary>
    /// <param name="obj"></param>
    public void objectiveBuildingLevel(Objective obj) {
        //check if all building are level 3
        bool allLevel3 = true;

        Transform[] tr = LevelManager.manager.getbuildings().ToArray();
        foreach (Transform bldng in tr) {
            if (bldng.GetComponent<PlayerStructure>().bldgLevel != 3) {
                allLevel3 = false;
                break;
            }
        }

        if (tr.Length == 0) {
            obj.completed = false;
        } else {
            obj.completed = allLevel3;
        }
        
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
    /// checks how many buildings in the world of a given type
    /// </summary>
    /// <param name="unitName"></param> the name of the unit
    /// <returns></returns> number of all units found
    private int getNumberOfBuildings(string buildingName) {
        int buildingCount = 0;
        Transform[] tr = LevelManager.manager.getbuildings().ToArray();
        foreach (Transform bldng in tr) {
            if (bldng.GetComponent<PlayerStructure>().unitName == buildingName) {
                buildingCount++;
            }
        }
        return buildingCount;
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
    void Update()
    {
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

/// <summary>
/// the objective element/item
/// </summary>
public class Objective {
    public string unitName;
    public string description;
    public bool completed;
    public int requiredCount;
    public int count;
    public bool showCount = true;
    public checkIfCompleted checkIfObjectiveIsCompleted;

    public Objective(string description, string unitName, int count) {
        this.description = description;
        this.unitName = unitName;
        requiredCount = count;
    }
}