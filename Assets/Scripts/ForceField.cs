using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for the forcefield in level 3
/// </summary>
public class ForceField : MonoBehaviour
{
    public static ForceField forceField;
    public GameObject destroyIndicator;
    public ForceFieldGenerator[] forceFieldGens;

    private void Start() {
        forceField = this;//making this instance accesible to other objects
    }
        
    private void Update() {
        
        //check if all force field genrators have EMP vehicles near by
        //if yes, disable the force field
        bool forceFieldDown = true;
        foreach (ForceFieldGenerator forceFieldG in forceFieldGens) {
            if (forceFieldG.EMPV == null) {
                forceFieldDown = false;
            }
        }

        //inform player when forcefield is down
        //and destroy the genrators
        if (forceFieldDown) {
            destroyIndicator.SetActive(true);
            MessagingManager.manager.showToast("ForceField destroyed!", 3);

            //destroy genrators
            foreach (ForceFieldGenerator forceFieldG in forceFieldGens) {
                forceFieldG.gameObject.SetActive(false);
                forceFieldG.destroyThis();
            }
        }
    }

}
