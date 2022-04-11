using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class represents the crystal resources to be collected
/// </summary>
public class Crystal : MonoBehaviour
{
    public int crystalCount = 10; //how many crystals in this resource
    public int costPerCrystal = 10; //how musch is each crystal

    /// <summary>
    /// update the crystal count when being collected, and set object to 
    /// inactive when nothing left 
    /// </summary>
    /// <param name="amount"></param>
    public void updateCount(int amount) {
        crystalCount = crystalCount + amount;

        if (crystalCount <= 0) {
            gameObject.SetActive(false);
        }
    }
}
