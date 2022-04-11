using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for the Helicopter of the player, the only flying unit
/// </summary>
public class Helicopter : SoldierControls
{
    private Vector3 flightTarget;
    private bool flying;
    private float flightStoppingDistance = 0.5f;
    public float heliElevation = 9;

    private void OnEnable() {
        //set elevation of heli
        transform.position = new Vector3(transform.position.x, heliElevation,
            transform.position.z);
    }
 
    /// <summary>
    /// move to target
    /// </summary>
    /// <param name="newPos"></param>
    public override void moveTo(Vector3 newPos) {
        if (isDead()) return;
        //use naveMesh to move
        flightTarget = new Vector3(newPos.x, transform.position.y, newPos.z);
        flying = true;
    }

    public override void Update() {
        base.Update();

        if (flying) {
            //look at travel position
            if (!isAttacking()) {
                Vector3 relativePos = flightTarget - transform.position;
                Quaternion rot = Quaternion.LookRotation(relativePos);
                float rotSpeed = moveSpeed / 3;
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotSpeed);
            }
           
            //move to target
            float speed = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, flightTarget, speed);
            
            //stop when near target
            if (Vector3.Distance(transform.position, flightTarget) <= flightStoppingDistance) {
                flying = false;
            }
        }

        //adjust rotaion so the heli won't look straight down
        Vector3 rotAng = transform.localEulerAngles;
        if (rotAng.x > 35) {
            transform.localEulerAngles = new Vector3(35, rotAng.y, rotAng.z);
        }
    }
}
