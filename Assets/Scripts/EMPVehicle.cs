using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class is for the electromagnetic puls (EMP) vehicle  used in level 3 to disable
/// the forcefield generators
/// it inherits from SoldierControls
/// </summary>
public class EMPVehicle : SoldierControls
{

    public override void Start() {
        base.Start();
        enemyMaskLayer = LayerMask.GetMask("Generator");
        enemyLayer = LayerMask.NameToLayer("Generator");
    }

    private ForceFieldGenerator ForceFieldGen;

    public override void attack() {
        base.attack();
        //the attack for the emp vehicle to to attach it self to
        //nearby ForceFieldGenerator
        if (enemyTarget != null &&
            enemyTarget.GetComponent<ForceFieldGenerator>() != null &&
            ForceField.forceField != null) {
            //get nearby ForceFieldGenerator
            ForceFieldGen = enemyTarget.GetComponent<ForceFieldGenerator>();
        } 
    }

    public override void Update() {
        base.Update();

        //attach this emp class to a nearby ForceFieldGenerator
        if (ForceFieldGen != null) {
            if (Vector3.Distance(transform.position, ForceFieldGen.transform.position) <=
                attackDistance) {
                ForceFieldGen.GetComponent<ForceFieldGenerator>().EMPV = this;
            } else {
                ForceFieldGen.GetComponent<ForceFieldGenerator>().EMPV = null;
                ForceFieldGen = null;
            }
        }
    }
}
