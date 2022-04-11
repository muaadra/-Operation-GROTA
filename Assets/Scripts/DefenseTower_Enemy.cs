using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class is for the enemy defence towers or walls, it inherits from EnemyUnit
/// </summary>
public class DefenseTower_Enemy : EnemyUnit
{
    public Transform head;

    public override void Start() {
        base.Start();
        theHeadThatLooksAt = head;
    }

    public override void attack() {
        if (hitPower <= 0) return; // if this is an enemy defence wall

        attackParticleEffect.SetActive(true);
        base.attack();
    }

    public override void playAnimation(string name) {
        //no animation
    }

}
