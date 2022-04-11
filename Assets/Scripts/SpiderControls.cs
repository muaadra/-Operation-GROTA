using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// controls the enemy's spiders
/// </summary>

public class SpiderControls : EnemyUnit
{

    private bool moving;
    private NavMeshAgent agent;
    private Vector3 targPos;

    public override void Start() {
        base.Start();

        //referencing
        agent = GetComponent<NavMeshAgent>();
        moveToTarget(getRandomDistination());
    }

    /// <summary>
    /// moves the object to a given target position
    /// </summary>
    /// <param name="targetPos"></param> the target position
    public void moveToTarget(Vector3 targetPos) {
        targPos = targetPos;
        if (isDead()) return;

        agent.enabled = true;
        agent.SetDestination(targetPos);//using naveMesh to move

        playAnimation("Run");
        
        moving = true; //flag to to start moving in update method

    }

    /// <summary>
    /// overrides attack method, check base.attack()
    /// this modifies the attack so it starts at the stopping distance
    /// rather than attack distance
    /// </summary>
    public override void attack() {
        bool isWithinAttackDist = Vector3.Distance(transform.position, 
            enemyTarget.transform.position) <= agent.stoppingDistance + 0.1f;

        //attack if within distance, otherwise keep following target
        if (enemyTarget != null && !isWithinAttackDist) {
            moveToTarget(enemyTarget.transform.position);

        } else if(isWithinAttackDist) {
            playAnimation("Attack");
            base.attack();
        }
    }


    public void Update() {
        if (isDead()) return;

        //moving object to target
        if (moving) {
            //agent.remainingDistance doesn't always give correct distance,
            //so compare it to v3 distance, pick the greater
            float remainingDist = agent.remainingDistance;
            float v3Dist = Vector3.Distance(transform.position, targPos);
            if (v3Dist > remainingDist) {
                remainingDist = v3Dist;
            }

            if (remainingDist - 0.1f <= agent.stoppingDistance) {
                if (enemyTarget == null) {
                    moveToTarget(getRandomDistination()); //move to random target
                }    
            }
        }

    }

}
