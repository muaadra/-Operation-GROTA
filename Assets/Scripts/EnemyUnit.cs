using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is for Enemy units it inhirits most of it funtionality from the class
/// Unit
/// </summary>
public class EnemyUnit : Unit
{
    public float raduisOfMove = 8; //the raduis of the area the obj can move within
    private Vector3 initPos;


    public virtual void Start() {
        //referencing
        gameObject.layer = LevelManager.manager.enemyLayer;
        enemyMaskLayer = LayerMask.GetMask("Player");
        enemyLayer = LayerMask.NameToLayer("Player");
        damageIndicator.GetComponent<SpriteRenderer>().color =
            Color.blue;
        initPos = transform.position;

    }

    /// <summary>
    /// get a random position within a raduis from initial instantiation
    /// </summary>
    /// <returns></returns>
    public Vector3 getRandomDistination() {
        //calc new distination, based on allowed raduis and random angle
        double x = raduisOfMove * Math.Cos(UnityEngine.Random.Range(0, 360) *
            Mathf.Deg2Rad);
        double z = raduisOfMove * Math.Sin(UnityEngine.Random.Range(0, 360) *
            Mathf.Deg2Rad);
        return new Vector3(initPos.x + (float)x, transform.position.y,
            initPos.z + (float)z);
    }


}
