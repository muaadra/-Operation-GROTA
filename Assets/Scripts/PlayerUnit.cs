using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// this class represent the offensive player units
/// </summary>

public abstract class PlayerUnit : Unit
{
  
    [HideInInspector] public float RemainigTDispatch;
    public GameObject selectionIndicator;
    private NavMeshAgent agent;
    public bool moving;
    private Vector3 targPos;


    public override void Awake() {
        base.Awake();

        //referencing
        RemainigTDispatch = timeToDispatch;
        agent = GetComponent<NavMeshAgent>();
    }


    public virtual void Start() {
        //setting up this object layer and target layer
        gameObject.layer = LevelManager.manager.playerLayer;
        enemyMaskLayer = LayerMask.GetMask("Enemy");
        enemyLayer = LayerMask.NameToLayer("Enemy");

        LevelManager.manager.selectables.Add(transform); //make this obj selectable
        LevelManager.manager.addPlayerUnit(this); //to keep track of all units in the world
    }

    /// <summary>
    /// move to target position
    /// </summary>
    /// <param name="newPos"></param> target position
    public virtual void moveTo(Vector3 newPos) {
        if (isDead()) return;
        
        //use naveMesh to move
        targPos = newPos;
        agent.SetDestination(newPos);
        playAnimation("Run");
        moving = true;
    }


    /// <summary>
    /// what happens when this object is selected
    /// </summary>
    /// <param name="selected"></param>
    public void selected(bool selected) {
        if (selected) {
            selectionIndicator.SetActive(true);
        } else {
            selectionIndicator.SetActive(false);
        }
    }

    public virtual void Update() {
        if (moving) {
            //agent.remainingDistance doesn't always give correct distance, so compare it 
            //to v3 distance, pick the greater
            float remainingDist = agent.remainingDistance;
            float v3Dist = Vector3.Distance(transform.position, targPos);
            if (v3Dist > remainingDist) {
                remainingDist = v3Dist;
            }
            if (remainingDist <= agent.stoppingDistance) {
                playAnimation("Idle");
                moving = false;
                arrivedAtDistination();
            }
        }

    }

    /// <summary>
    /// what happens when unit arrives at distination
    /// </summary>
    public virtual void arrivedAtDistination() {
        //for subclasses, doesn't have to be implemented
    }

    /// <summary>
    /// return the target location
    /// </summary>
    public Vector3 getMoveTarget() {
        return targPos;
    }

    /// <summary>
    /// set of cleaning actions to perform when destroying this object
    /// </summary>
    public override void destroyThis() {
        base.destroyThis();
        moving = false;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null && agent.enabled) {
            GetComponent<NavMeshAgent>().isStopped = true;
        }   
        LevelManager.manager.removePlayerUnit(this);
        LevelManager.manager.selectables.Remove(transform);
    }

    


}
