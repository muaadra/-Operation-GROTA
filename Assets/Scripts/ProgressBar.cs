using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the progress bar for any object in the scene
/// </summary>


public class ProgressBar : MonoBehaviour
{
    public Transform transToFollow;
    public RectTransform targetCanvas;
    public RectTransform healthBar;
    public RectTransform healthBarBG;
    private Camera mainCam;
    private Vector3 scale;
    private Canvas canvas;

    private void Start() {
        //referencing
        scale = healthBar.sizeDelta;
        mainCam = LevelManager.manager.mainCam;
        healthBarBG = GetComponent<RectTransform>();
        canvas = LevelManager.manager.UICanvas;
    }

    /// <summary>
    /// set the progress bar precentage
    /// </summary>
    /// <param name="precentage"></param>
    public void setBar(float precentage) {
        if (precentage < 0) {
            precentage = 0;
        }

        Vector3 nScale = new Vector3(precentage * scale.x, scale.y, scale.z);
        healthBar.sizeDelta = nScale;
    }

    /// <summary>
    /// show/hide the progress bar
    /// </summary>
    /// <param name="show"></param>
    /// <param name="transformToFollow"></param>
    public void show(bool show, Transform transformToFollow) {
        transToFollow = transformToFollow;
        gameObject.SetActive(show);
        enabled = show;
    }

    void Update() {
        if (!enabled) return;
        Vector2 screenPos = mainCam.WorldToScreenPoint(transToFollow.position);
        screenPos = screenPos / canvas.scaleFactor;

        healthBarBG.anchoredPosition = screenPos;
    }



}
