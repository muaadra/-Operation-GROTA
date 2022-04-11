using UnityEngine;

/// <summary>
/// The forcfield generator
/// This class recieves EMPVehicle object if an EMP vehicle is nearby
/// </summary>
public class ForceFieldGenerator : DefenseTower_Enemy
{
    public EMPVehicle EMPV;



    public override void Start() {
        base.Start();
        gameObject.layer = LayerMask.NameToLayer("Generator");
    }
    public override void receiveDamage(float damage) {
        //do nothing
        //you can't destroy the generator, only disable it using EMP
    }

}
