using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is for the Harvester. It detects nearby resources and starts
/// harvesting automatically
/// </summary>
public class Harvester : PlayerUnit
{
    public float harvestFreq = 5; //(every x seconds) collect 1 unit in how many x seconds
    private float harvestingDistance; // the stopping distance
    private List<Transform> crystalsInRange = new List<Transform>(); //the range is the collider raduis
    public Transform rangeIndicator;
    public MoneyAnimi moneyAnimi; //the money animation, to indicate the harvester is collecting
    private float range;
    private float harvestTimer;
    private Vector3 autoTarg;

    public override void Start() {
        base.Start();
        range = rangeIndicator.localScale.x/2; //range is based on 'physical' cylinder object
        harvestTimer = harvestFreq;
        harvestingDistance = GetComponent<NavMeshAgent>().stoppingDistance;
    }

    /// <summary>
    /// when the harvester arrives at distination, it detects if crystals 
    /// in range and starts harvesting
    /// </summary>
    public override void arrivedAtDistination() {
        print("arrive");
        getCrystalsInRange();
    }

    private new void Update() {
        base.Update();

        //if there are crystals in range, start the harvest by reducing
        //the crystal counts every x seconds
        if (crystalsInRange.Count > 0) {
            if ((harvestTimer% harvestFreq) <= 0) {
                Crystal cr = crystalsInRange[0].GetComponent<Crystal>();
                if (Vector3.Distance(cr.transform.position,transform.position) > harvestingDistance) {
                    print("11");
                    crystalsInRange.RemoveAt(0);
                } else {
                    print("22");
                    cr.updateCount(-1);
                    PlayerStats.stats.adjustFund(cr.costPerCrystal); //add funds to player
                    moneyAnimi.showMoneyAnimi(cr.costPerCrystal);
                    harvestTimer = harvestFreq;
                    if (cr.crystalCount <= 0) { //no more crystals
                        autoTarg.x = float.PositiveInfinity;
                        getCrystalsInRange();
                    }
                }

            }
            harvestTimer -= Time.deltaTime;
        }
    }


    /// <summary>
    /// gets all crystal in range
    /// </summary>
    private void getCrystalsInRange() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

        //get the crystal from hitColliders
        crystalsInRange.Clear();
        foreach (Collider col in hitColliders) {
            if (col.GetComponent<Crystal>() != null) {
                crystalsInRange.Add(col.transform);
            }
        }

        //sort crystals location by dist
        crystalsInRange.Sort(new TransformDistComparer(transform));

        //adjust harvester position to crystals
        if (crystalsInRange.Count > 0
            //so not call this this method again, if the harvestor is just adjusting itself
            && Vector3.Distance(autoTarg, getMoveTarget()) > 0.25f) {
            autoTarg = crystalsInRange[0].transform.position;
            moveTo(autoTarg);
        }
    }

    public override void FixedUpdate() {
        //only use the disableObject function from base
        disableObject();
    }


}
