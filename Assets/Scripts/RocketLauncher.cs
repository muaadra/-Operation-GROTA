using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the rocket launcher, this is diffrent from other units mainly because 
/// we need to show the rocket/projectile
/// </summary>

public class RocketLauncher : SoldierControls
{
    public Transform projectile;
    private float perctTravel = 20;
    private Vector3 start;
    private Vector3 initRot;
    private Vector3 end;
    private float speed = 1;
    private Vector3 midPoint;

    public override void Start() {
        base.Start();
        initRot = projectile.localEulerAngles;
    }


    /// <summary>
    /// override the attack method, see base for summary
    /// </summary>
    public override void attack() {

        //play projectile animation
        projectileAnimi();

        base.attack();

        //reset projcetile angle after attack
        if (enemyTarget == null) {
            projectile.localEulerAngles = initRot;
        }
    }

    /// <summary>
    /// projectile animation
    /// </summary>
    private void projectileAnimi() {
        start = projectile.position;
        end = enemyTarget.position;
        midPoint = Vector3.Lerp(start, end, 0.5f);
        midPoint.y = 10; //height
        perctTravel = 0;
    }

    public override void Update() {
        base.Update();
        
        //projectile animation follows a curve
        if (perctTravel < 1) {
            //Bezier Curves
            perctTravel += speed * Time.deltaTime;
            Vector3 p1 = Vector3.Lerp(start, midPoint, perctTravel);
            Vector3 p2 = Vector3.Lerp(midPoint, end, perctTravel);
            
            projectile.position = Vector3.Lerp(p1, p2, perctTravel);

            if (perctTravel + speed * Time.deltaTime >= 1) {
                projectile.localPosition = Vector3.zero; 
            }
        }

    }
}
