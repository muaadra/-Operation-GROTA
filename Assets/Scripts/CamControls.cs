using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class handles camera controlls. zoom in/out, translate, rotate, 
/// and the boundries of which the camera must stay within in the game world
/// </summary>
public class CamControls : MonoBehaviour
{
    public Vector3 upRightLimit; //the upper right boundry
    public Vector3 bottomLeftLimit; //the bottom left boundry
    private Camera mainCam;
    private Vector3 initCamPos;
    private Vector3 initClick;
    private Vector3 initCamAngle;

    private float zoomAmount;
    private float maxZoom = 5; // max limit of zoom in/out 
    private float zoomSpeed = 5;
    private Vector3 camPosAtStart;

    void Start()
    {
        //referencing
        mainCam = LevelManager.manager.mainCam;
        camPosAtStart = mainCam.transform.position; 
    }


    void Update()
    {
        //mouse wheel used for zoom in/out
        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            zoomAmount += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            zoomAmount = Mathf.Clamp(zoomAmount, maxZoom * -1, maxZoom); //setting zoom limit
            Vector3 camNewPos = mainCam.transform.position;
            camNewPos.y = camPosAtStart.y + zoomAmount *-1;
            mainCam.transform.position = camNewPos;
        }


        //middle button is used to translate the camera
        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Space)) {
            initCamPos = ScreenToWorldPosition(); //get mouse pos in the world
        }

        if (Input.GetMouseButton(2) || Input.GetKey(KeyCode.Space)) {
            Vector3 mousePos = Input.mousePosition;
            //if player mouse goes off screen
            if (mousePos.y > Screen.height || mousePos.y < 0
                || mousePos.x > Screen.width || mousePos.x < 0) {
                return;
            }

            Vector3 direction = initCamPos - ScreenToWorldPosition();

            //if player mouse goes off world boudries
            if (upRightLimit.x - (mainCam.transform.position + direction).x < 1f
                || (mainCam.transform.position + direction).x - bottomLeftLimit.x < 1f) {
                direction.x = 0;
            }
            if ((mainCam.transform.position + direction).z - upRightLimit.z< 1f
                ||bottomLeftLimit.z - (mainCam.transform.position + direction).z < 1f) {
                direction.z = 0;
            }

            //move can in the direction the mouse moves
            mainCam.transform.position += direction;

        }

        //right click is used for cam rotation, 
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftAlt)) {
            initCamPos = ScreenToWorldPosition(); //get mouse pos in the world
            initClick = Input.mousePosition;
            initCamAngle = mainCam.transform.eulerAngles;
        }

        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftAlt)) {
            float mouseMoveDist = initClick.x - Input.mousePosition.x;
            float percentageMove = mouseMoveDist / Screen.width;
            Vector3 camAngle = mainCam.transform.localEulerAngles;
            camAngle.y = initCamAngle.y + percentageMove * 90 * -1; //set angle opposit of mouse direction
            mainCam.transform.localEulerAngles = camAngle; //change cam angle
        }


    }

    /// <summary>
    /// traslates mose position on screen to world location by shooting
    /// a ray from to ground and returning the point of intersection 
    /// </summary>
    /// <returns></returns>
    private Vector3 ScreenToWorldPosition() {
        Ray mousePos = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, 0);
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }


}
