using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for the mindhive (the nexus) automated movements and attacks
/// That nexus follows a defined path, when a player unit is detected, it attacks 
/// using 2 types of attacks; attack0 and attack1 (see relevant methods) 
/// </summary>
public class NexusAI : EnemyFlyingUnit
{
    public float attack0Damage = 150;
    public float attack1Damage = 100;
    public Transform pathsParent; //the transform that follows the path
    public Transform nexusBody;
    public GameObject forceField;
    private float perctTravel = 20;
    public float speed = 0.5f;
    public GameObject attack0VFX;
    public GameObject attack1VFX;
    public Transform healthBarPos;
    public AudioClip nexusDeathExplosionSFX;
    public GameObject deathWave;
    private int pathIdx;
    private PlayerUnit target;
    private Vector3 pathInitialPos; //the starting point of the path
    private float attackTimer;
    private int attackId;
    private float stoppingDist = 15;
    private float attack0Timer;
    private Vector3 targPos;
    private float nexusDestroyTimer;
    private static NexusAI nexus;

    public override void Start() { 
        base.Start();
        nexus = this; //make it accessible to other objects

        perctTravel = 0;

        //get intial path pos
        pathInitialPos = pathsParent.GetChild(pathIdx).
            GetComponent<Path>().pathPoints[0].position;

        //set layer to default until forcefield is down
        gameObject.layer = 0;

        //create a progress bar
        healthBar = LevelManager.manager.createAProgressBar(healthBarPos);
    }

    public override void Update() {
        //start Nexus actions when forcefield is down
        if (forceField.activeSelf == false) {
            gameObject.layer = LevelManager.manager.enemyLayer;

            //if there no targets in range, then follow the path, otherwise attack
            if (getPlayerUnitInRange() == null) {
                followPath();
            } else {
                movetoTarget();
                attack();
            }
        }

    }

    /// <summary>
    /// this method moves the nexus to near target position 
    /// </summary>
    private void movetoTarget() {
        //move to target
        Vector3 targPos = target.transform.position;
        theHeadThatLooksAt.LookAt(targPos); //look at the target

        //stop when stoppingDist is reached
        if (Vector3.Distance(nexusBody.position, targPos) <= stoppingDist) {
            return;
        }

        //move to targ
        Vector3 newPos = Vector3.MoveTowards(nexusBody.position, targPos, speed);
        newPos.y = nexusBody.position.y;
        nexusBody.position = newPos;
    }

    /// <summary>
    /// this method overrides base attack method
    /// nexus uses 2 types of attacks, attack0 is performed twice in a row
    /// then attac1
    /// </summary>
    public override void attack() {
        //if within attack distance, attack
        if (Vector3.Distance(nexusBody.position, target.transform.position)
            <= attackDistance) {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0) {
                //attack 0 is performed twice
                if (attackId < 2) {
                    attck0();
                } else {
                    attack1();
                }


                //there are 2 types of attacks for nexus
                //attack 0 is performed twice
                attackId = (attackId + 1) % 3;
            }        
        }

        //if attack0 is active, play its animation
        if (attack0Timer > 0) {
            attck0Animation(); 
        }

    }

    /// <summary>
    /// animation for attack0, it moves the ball of energy to target position
    /// </summary>
    private void attck0Animation() {
            attack0Timer -= Time.deltaTime;
            float shootSpeed = 1f;
            attack0VFX.transform.position =
                Vector3.MoveTowards(attack0VFX.transform.position,targPos,
                attack0Timer * shootSpeed);    
    }

    /// <summary>
    /// 1st type of attack is a targeted attack using energy ball
    /// </summary>
    private void attck0() {
        attackTimer = 5; //time it takes to reload from this attack

        //reset VFX
        attack0VFX.SetActive(false);
        attack0VFX.SetActive(true);

        //play shooting animation
        targPos = target.transform.position;
        attack0VFX.transform.position = nexusBody.transform.position;
        attack0Timer = 1;
        animator.SetTrigger("Attack0P");

        //send damage
        target.receiveDamage(attack0Damage);
        //check if targ is dead
        if (target.isDead()) {
            target = null;
            rejoinPath(); //go back to  following the path
        }
    }

    /// <summary>
    /// 2nd type of attack is an area attack where all targets within range recieve
    /// damage
    /// </summary>
    private void attack1() {
        attackTimer = 8; //time it takes to reload from this attack

        //launch attack
        List<PlayerUnit> targets = getAllPlayerUnitsInRange();
        foreach (PlayerUnit unit in targets) {
            //send damage
            unit.receiveDamage(attack1Damage);
        }

        //show VFX
        Vector3 vfxPos = nexusBody.transform.position;
        vfxPos.y = attack1VFX.transform.position.y;
        attack1VFX.transform.position = vfxPos;
        attack1VFX.SetActive(false);
        attack1VFX.SetActive(true);
        animator.SetTrigger("Attack1");

        if (target.isDead()) {
            target = null;
            rejoinPath(); //go back to  following the path
        }
    }

    /// <summary>
    /// this method makes the nexus follow a defined path. See Scene window in 
    /// editor for the drawn path
    /// </summary>
    private void followPath() {
        //traverse throw all points under "pathsParent"
        if (perctTravel < 1) {
            perctTravel += speed * Time.deltaTime;
            
            //get the path to follow
            Path pathCS = pathsParent.GetChild(pathIdx).GetComponent<Path>();

            //based in point of the pathCS, smooth the movment using
            //Bezier Curve, so get the next position by calling getPosInPath
            //to get the next point in the curve
            Vector3 nextPos = getPosInPath(perctTravel, pathCS.pathPoints);
            nextPos.y = nexusBody.position.y;

            theHeadThatLooksAt.LookAt(nextPos); //look at next pos
            nexusBody.position = nextPos; //move to next pos

            //when the last point in the path is reached, get the next path
            //or loop to the starting path
            if (perctTravel + speed * Time.deltaTime >= 1) {
                //reset initial path pos to original pos, incase nexus move off path
                pathsParent.GetChild(0).GetComponent<Path>().pathPoints[0]
                    .position = pathInitialPos;

                //reset perctTravel and move to next path
                perctTravel = 0;
                pathIdx = (pathIdx + 1) % pathsParent.childCount;
            }
        }
    }

    /// <summary>
    /// after nexus is done attacking, rejoin the movement path
    /// </summary>
    private void rejoinPath() {
        //move initial path pos to nexus pos
        pathsParent.GetChild(0).GetComponent<Path>().pathPoints[0]
                    .position = nexusBody.position;
        perctTravel = 0;
        pathIdx = 0;
    }


    /// <summary>
    /// get one target units within range
    /// </summary>
    public PlayerUnit getPlayerUnitInRange() {
        if (target != null) return target;

        //get a target within Physics.OverlapSphere
        Collider[] hitColliders = Physics.OverlapSphere(nexusBody.position, attackDistance);
        foreach (Collider col in hitColliders) {
            PlayerUnit playerUnit = col.gameObject.GetComponent<PlayerUnit>();
            if (playerUnit != null && !playerUnit.isDead()) {
                target = playerUnit;
                return playerUnit; //return the first target
            }
        }
        return null;
    }


    /// <summary>
    /// get a list of All target units within range
    /// </summary>
    public List<PlayerUnit> getAllPlayerUnitsInRange() {
        List<PlayerUnit> units = new List<PlayerUnit> ();

        //get all targets within Physics.OverlapSphere
        Collider[] hitColliders = Physics.OverlapSphere(nexusBody.position, attackDistance * 0.75f);
        foreach (Collider col in hitColliders) {
            PlayerUnit playerUnit = col.gameObject.GetComponent<PlayerUnit>();
            if (playerUnit != null && !playerUnit.isDead()) {
                units.Add(playerUnit);
            }
        }
        return units;
    }

    /// <summary>
    /// get the next position in a Bezier Curve. This method
    /// follow an equation in https://en.wikipedia.org/wiki/B%C3%A9zier_curve 
    /// (Quadratic B�zier curves)
    /// </summary>
    /// <param name="t"></param> t is precentage along the curve [0,1]
    /// <param name="path"></param> path consists of 4 points that makes the curve
    /// <returns></returns>
    private Vector3 getPosInPath(float t, Transform[] path) {
        //https://en.wikipedia.org/wiki/B%C3%A9zier_curve (Quadratic B�zier curves)
        Vector3 pos = Mathf.Pow(1 - t, 3) * path[0].position
                        + 3 * Mathf.Pow(1 - t, 2) * t * path[1].position
                        + 3 * (1 - t) * Mathf.Pow(t, 2) * path[2].position
                        + Mathf.Pow(t, 3) * path[3].position;
        return pos;
    }

    public override void FixedUpdate() {
        //destroy this object in x sec after death
        disableObject();
    }

    public override void disableObject() {
        base.disableObject();

        //after destrying the nexus, wait few seconds to show the
        //congratulations message
        if (isDead() && destroyTimer <= 0) {
            MessagingManager.manager.storyMssgs3.showFinalStory();
        }
    }

    /// <summary>
    /// events to happen when the nexus is dead
    /// </summary>
    public override void destroyThis() { 
        base.destroyThis();
        destroyTimer = 2f;
        healthBar.gameObject.SetActive(false);
        MusicManager.manager.playOneShot(nexusDeathExplosionSFX,1);
        deathWave.SetActive(true);
    }

    public static bool isNexusDead() {
        return nexus.isDead();
    }
  }
