using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The power plant structure. It check all nearby buildings and plug itself
/// to the power source field of that building, the other buildings cannot
/// operated if there power source is null
/// </summary>
public class PowerPlant: PlayerStructure
{
    public float powerRadius = 30;
    public int powerCapacity = 50;
    private int lastInfrastructureChangeID;
    public List<MyBuilding> myBuildings = new List<MyBuilding>();
    private LevelManager manager;

    public override void Start() {
        base.Start();
        manager = LevelManager.manager;
        providePower();
    }

    /// <summary>
    /// check all nearby buildings and plug myself to the power source field 
    /// of that building, the other buildings cannot operated if there power source is null
    /// </summary>
    private void providePower() {

        //get all buildings
        List<Transform> structs = new List<Transform>();
        structs.AddRange(LevelManager.manager.getbuildings());
        structs.Sort(new TransformDistComparer(transform)); //sort by distance
        
        //to keep track of last infrastructure changes
        lastInfrastructureChangeID = LevelManager.manager.infrastructureChangeID; //keep track of changes to infra

        //add this power plants to nearby buildings that need power
        Transform[] tr = LevelManager.manager.getbuildings().ToArray();
        foreach (Transform str in tr) {
            PlayerStructure strCS = str.GetComponent<PlayerStructure>();

            //if I have enough power, then plug this instance into the other bulding
            if ((powerCapacity - strCS.powerRequirment) >= 0 &&
                Vector3.Distance(transform.position, transform.position) <= powerRadius
                && strCS != this
                && strCS.powerSource == null) {
                powerCapacity -= strCS.powerRequirment;
                strCS.powerSource = this;
                myBuildings.Add(new MyBuilding(strCS.transform, strCS.powerRequirment));//adding the building my power grid
            }
        }
    }

    public void Update() {
        if (manager.infrastructureChangeID != lastInfrastructureChangeID) {
            lastInfrastructureChangeID = manager.infrastructureChangeID;
            updatePowerCapacity(); //change who you provide power to based on new infras.
        }
    }


    /// <summary>
    /// if there is a change to infrastructure, update the power grid
    /// </summary>
    private void updatePowerCapacity() {
        List<MyBuilding> destryedBuildings = new List<MyBuilding>();

        foreach (MyBuilding str in myBuildings) {
            if (str.building == null) { //building was destroyed
                powerCapacity += str.powerRequirment; //get power back
                destryedBuildings.Add(str);
            }
        }
        
        //remove destroyed buildings from my grid buildings
        foreach (MyBuilding str in destryedBuildings) {
            myBuildings.Remove(str);
        }

        providePower(); //see if there are new building to provide power to
    }

    //because building may be destroyed, so we need to keep track of it power
    public class MyBuilding {
        public Transform building;
        public int powerRequirment;

        public MyBuilding(Transform building, int powerRequirment) {
            this.building = building;
            this.powerRequirment = powerRequirment;
        }

    }
}