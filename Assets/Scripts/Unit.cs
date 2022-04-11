using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This is the main class for all units (Player's and Enemy's units).
/// This class has the common functions shared between all units like 
/// scanForEnemy() and receiveDamage(), a sub class can override these methods
/// </summary>


public abstract class Unit : MonoBehaviour
{
    public string unitName;
    public string about;
    public int unitCost = 500;
    public float timeToDispatch = 10;
    public float health = 100;
    public float hitPower = 5;
    public float attackDistance = 7;
    public float reloadTime = 1;
    public float moveSpeed;
    public Sprite thumbnail;
    public GameObject deathExplosion; //vfx when destroyed
    public GameObject bodyMesh;
    public bool usesColliderForEnemyDetection = false; //set true for flying units
    [HideInInspector] public bool dead;
    public Collider[] neighbors;
    public GameObject attackParticleEffect;
    public GameObject damageIndicator;
    [HideInInspector] public int enemyMaskLayer;
    [HideInInspector] public int enemyLayer;
    private bool attacking;
    [HideInInspector] public float destroyTimer;
    public Animator animator;
    public Transform theHeadThatLooksAt; //the part that faces the attacker
    private AudioSource audioSource;
    public AudioClip attackSFX;
    [HideInInspector] public ProgressBar healthBar;
    [HideInInspector] public float initialHealth;

    public virtual void Awake() {
        //referencing
        audioSource = GetComponent<AudioSource>();

        //set which part of the body looks at the target when attacking
        if (theHeadThatLooksAt == null) {
            theHeadThatLooksAt = transform;
        }

        if (animator == null) {
            animator = GetComponent<Animator>();
        }

        //set movement speed
        if (GetComponent<NavMeshAgent>()!= null) {
            GetComponent<NavMeshAgent>().speed = moveSpeed;
        }

        initialHealth = health;
    }


    /// <summary>
    /// whe this unit is hit, recieve a damage from the other offensive unit
    /// </summary>
    /// <param name="damage"></param> the amount of damage caused to this unit
    public virtual void receiveDamage(float damage) {
        if (isDead()) return;

        health = health - damage; //reduce heath
        damageIndicator.SetActive(true); //show damage vfx

        if (health <= 0) {
            health = 0;
            destroyThis();
        }

        if (healthBar != null) {
            healthBar.setBar(health / initialHealth);
        }
    }


    private int rayCastAngle;
    private int rayCastAngStep = 3;
    private bool rayScanCycleCompleted;
    [HideInInspector] public Transform enemyTarget;
    private float hitTimer;
    /// <summary>
    /// timing the attacks
    /// </summary>
    public virtual void FixedUpdate() {
        //check if there is enemy around
        if (enemyTarget == null &&
            (enemyTarget = scanForEnemy()) != null) {
            //start attack
            hitTimer = 0;
            attacking = true;
        }

        if (attacking && hitTimer <= 0) {
            hitTimer = reloadTime;
            attack();
        }

        if (attacking) {
            theHeadThatLooksAt.LookAt(enemyTarget);
        }

        hitTimer -= Time.deltaTime;


        //destroy this object in x sec after death
        disableObject();
    }

    /// <summary>
    /// scan nearby area for enemy. ground units use a ray cast for detecting 
    /// other ground units, so if enemy is behind a wall, it is not detected.
    /// for air units a sphere collider is used
    /// </summary>
    /// <returns></returns>
    private Transform scanForEnemy() {
        //using overlapping collider
        if (usesColliderForEnemyDetection) {
           return scanForEnemyUsingOverlapSphere(false);
        }

        //using raycast for ground enemys, and overlapping cllider for 
        //enemy flying units
        Transform targ = scanForEnemyUsingRayCast();
        if (targ != null) {
            return targ;
        }

        //if no ground threat check air
        if (targ == null && rayScanCycleCompleted) {
            return scanForEnemyUsingOverlapSphere(true);
        }

        return null;
    }

    /// <summary>
    /// called when a nearby enemy target is detected to start an attack
    /// </summary>
    public virtual void attack() {
        if (dead) return;

        Unit targ = enemyTarget.GetComponent<Unit>();

        //stop attack if target is dead
        if (targ.isDead() || targ == null) {
            stopAttack();
            return;
        }

        targ.receiveDamage(hitPower); //send damage to target
        audioSource.PlayOneShot(attackSFX);//play attack sound fx

        //if the enemy is dead or is far, stop attack
        if (targ.isDead() || 
            Vector3.Distance(transform.position, enemyTarget.position) > attackDistance) {
            stopAttack();
        }
    }

    /// <summary>
    /// end the attack on target unit
    /// </summary>
    private void stopAttack() {
        enemyTarget = null;
        attacking = false;
        attackParticleEffect.SetActive(false);
    }

    /// <summary>
    /// Scans for enemy targets using rays, static objects like defence towers can override
    /// this method and implement a diffrent scaning mechanism, just return the target
    /// </summary>
    /// <returns></returns>
    public virtual Transform scanForEnemyUsingRayCast() {
 
        //this creates an ray for the player unit to scan enemys around to a specific
        // distance the ray rotates to scan
        RaycastHit hit;
        rayCastAngle = (rayCastAngle + rayCastAngStep) % 360;
        Vector3 scanAngle = (Quaternion.Euler(5, rayCastAngle, 0) * new Vector3(0, 0, 1));
        Vector3 rayPos = new Vector3(transform.position.x, transform.position.y + 1,
        transform.position.z);
        //ray 1
        if (Physics.Raycast(rayPos, scanAngle, out hit, Mathf.Infinity, enemyMaskLayer)) {
            //check if enemy was detected at attackDistance
            if (Vector3.Distance(transform.position, hit.transform.position) 
                <= attackDistance) {
                return hit.transform;
            }
        }
        //Debug.DrawRay(rayPos, scanAngle * attackDistance, Color.red);

        //ray 2
        rayCastAngle = (rayCastAngle + rayCastAngStep) % 360;
        scanAngle = (Quaternion.Euler(-5, rayCastAngle, 0) * new Vector3(0, 0, 1));
        if (Physics.Raycast(rayPos, scanAngle, out hit, Mathf.Infinity, enemyMaskLayer)) {
            //check if enemy was detected at attackDistance

            if (Vector3.Distance(transform.position, hit.transform.position)
                <= attackDistance) {
                return hit.transform;
            }
        }

        //Debug.DrawRay(rayPos, scanAngle * attackDistance, Color.red);
        if (rayCastAngle == 360 - (2 * rayCastAngStep)) {
            rayScanCycleCompleted = true;
        } else {
            rayScanCycleCompleted = false;
        }


        return enemyTarget;
    }

    /// <summary>
    /// scan for nearby enemies using Physics.OverlapSphere. this is used
    /// for ground units to detect air units, or for air units to detect any type
    /// of units
    /// </summary>
    /// <param name="getAirUnits"></param>get the nearby air target units only
    /// <returns></returns> returns null if no target units found
    public Transform scanForEnemyUsingOverlapSphere(bool getAirUnits) {
        //get nearby targets
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackDistance);
        
        foreach (Collider col in hitColliders) {
            Unit targUnit = col.gameObject.GetComponent<Unit>();

            //check if its an enemy unit and within attack distance
            if (targUnit != null && targUnit.gameObject.layer == enemyLayer) {
                bool isWithinAttack = Vector3.Distance(transform.position, targUnit.transform.position)
                < attackDistance;
                if (!getAirUnits && isWithinAttack) {
                    return targUnit.transform;
                } else if(targUnit is EnemyFlyingUnit) {
                    return targUnit.transform ;
                }
                
            }
        }

        return null;
    }


    /// <summary>
    /// play an animation based on its name
    /// </summary>
    /// <param name="name"></param> name of animation
    public virtual void playAnimation(string name) {
        //reset
        animator.SetBool("Run", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Die", false);
        animator.SetBool("Idle", false);
        //set
        animator.SetBool(name, true);

    }

    /// <summary>
    /// set of actions to perform when destroying this object
    /// </summary>
    public virtual void destroyThis() {
        dead = true;
        destroyTimer = 0.5f;
        bodyMesh.SetActive(false);
        damageIndicator.SetActive(false);
        deathExplosion.SetActive(true);

        //play destroy sound effect
        audioSource.spatialBlend = 0.8f;
        audioSource.PlayOneShot(MusicManager.manager.dieExplosion);
    }

    /// <summary>
    /// disable this object in x sec based on destroyTimer
    /// </summary>
    public virtual void disableObject() {
        if (destroyTimer > 0) {
            destroyTimer -= Time.deltaTime;
            if (destroyTimer <= 0) {
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// is the unit currently attacking
    /// </summary>
    /// <returns></returns>
    public bool isAttacking() {
        return attacking;
    }

    public float getHealth() {
        return health;
    }

    public bool isDead() {
        return dead;
    }
}
