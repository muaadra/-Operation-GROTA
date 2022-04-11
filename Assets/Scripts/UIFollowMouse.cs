using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class makes a UI element follow the mouse
/// </summary>

public class UIFollowMouse : MonoBehaviour
{
    LevelManager manager;
    void Start()
    {
        manager = LevelManager.manager;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 1;
        transform.position = manager.UICam.ScreenToWorldPoint(screenPoint);
    }
}
