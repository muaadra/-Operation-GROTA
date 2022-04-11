using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// when a particle effect animation is completed, the hide function 
/// is called to hide this object
/// </summary>
public class paricleEffectOffEvent : MonoBehaviour
{
    public GameObject hideOther;

    /// <summary>
    ///when a particle effect animation is completed, the hide function 
    /// is called to hide this object
    /// </summary>
    public void hide() {
        if (hideOther != null) {
            hideOther.SetActive(false);
        }

        gameObject.SetActive(false);
    }
}
