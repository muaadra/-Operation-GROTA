using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// attach this class to any object you want it to keep looking at another object
/// </summary>

public class LookAt : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update

    private void OnEnable() {
        if (target != null) {
            transform.LookAt(target);
        }
    }

    void Start()
    {
        if (target == null) {
            target = LevelManager.manager.mainCam.transform;
            transform.LookAt(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
