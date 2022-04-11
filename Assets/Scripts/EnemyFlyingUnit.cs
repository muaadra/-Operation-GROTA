using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class is for the enemy flying units
/// it inherits from EnemyUnit
/// </summary>

public class EnemyFlyingUnit : EnemyUnit
{
    public float elevation = 9;
    private Vector3 flightTarget;
    private bool flying;
    private float flightStoppingDistance = 0.5f;
    public bool autoRandomFly = true;

    public override void Start() {
        base.Start();

        //set elevation of unit
        transform.position = new Vector3(transform.position.x, elevation,
            transform.position.z);

        if (autoRandomFly) { //if this units flyes randomly
            Vector3 randPos = getRandomDistination();
            randPos.y = elevation;
            moveToTarget(randPos);
        }
    }

    /// <summary>
    /// override attack method
    /// </summary>
    public override void attack() {
        //attack if within attack distance
        bool isWithinAttackDist = Vector3.Distance(transform.position,
            enemyTarget.transform.position) <= attackDistance;
        if (enemyTarget != null
            && !isWithinAttackDist) {//keep follwing target
            moveToTarget(enemyTarget.transform.position);

        } else if (isWithinAttackDist) {
            playAnimation("Attack");
            base.attack();
        }
    }

    /// <summary>
    /// moves this unit to target position
    /// </summary>
    /// <param name="newPos"></param>
    public void moveToTarget(Vector3 newPos) {
        if (isDead()) return;

        flightTarget = new Vector3(newPos.x, transform.position.y, newPos.z);
        flying = true;
    }

    public virtual void Update() {
        if (flying) {
            //look at travel position
            if (!isAttacking()) {
                Vector3 relativePos = flightTarget - transform.position;
                Quaternion rot = Quaternion.LookRotation(relativePos);
                float rotSpeed = moveSpeed / 5;
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotSpeed);
            }

            //move to target
            float speed = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, flightTarget, speed);

            //stop when near target
            if (Vector3.Distance(transform.position, flightTarget) <= flightStoppingDistance) {
                if (enemyTarget == null) {
                    Vector3 randPos = getRandomDistination();
                    randPos.y = elevation;
                    moveToTarget(randPos);
                }
            }
        }
    }



}
