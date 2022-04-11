using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents an item in the Objectives menu/list
/// </summary>

public class UI_Objective_Item : MonoBehaviour
{
    public Objective myObjective;
    public Text description;
    public Text count;
    public Image completedIndicator;
    public Sprite completed;
    public Sprite notCompleted;

    /// <summary>
    /// update info in the objective item
    /// </summary>
    public void updateUIItem() {
        myObjective.checkIfObjectiveIsCompleted(myObjective);

        //update UI item with info
        description.text = myObjective.description;

        if (myObjective.showCount) {
            count.text = myObjective.count + "/" + myObjective.requiredCount;
        } else {
            count.text = "";
        }

        if (myObjective.completed) {
            completedIndicator.sprite = completed;
        } else {
            completedIndicator.sprite = notCompleted;
        }

    }
}
