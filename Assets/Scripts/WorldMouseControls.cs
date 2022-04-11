using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the mouse controls of the game.
/// player units selection and control
/// </summary>

public class WorldMouseControls : MonoBehaviour
{
    public Camera mainCam;
    public RectTransform selectionBox;
    public float clickRaduis = 1;
    private Vector2 mousePos1;
    private Vector2 mousePos2;
    private LevelManager manager;
    public List<Transform> selectedObjects = new List<Transform>();
    private bool isSelecting;
    private bool actionAfterSelectionCompleted;
    public Texture2D cursorMove;

    void Start()
    {
        //referencing
        manager = LevelManager.manager;
    }

    // Update is called once per frame
    void Update()
    {
        //check if user is interacting with UI
        if (manager.UIActive) {
            return;
        }

        // when mouse is clicked, show selection box
        if (Input.GetMouseButtonDown(0)) {
            mousePos1 = Input.mousePosition;
            checkIfSelectOrDoAction();
        }

        // when left mouse button is clicked, deselect
        if (Input.GetMouseButtonDown(1)) {
            if (selectedObjects.Count > 0) { 
                deselectObjects();
                actionAfterSelectionCompleted = true; //no action after selection
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }

        // when mouse is held down, resize selection box
        if (isSelecting) {
            mousePos2 = Input.mousePosition;

            startSelectingObjects();

            resizeSelectionBox();
        }

        // when mouse is up, hide selection box
        if (Input.GetMouseButtonUp(0)) {
            showSelectionBox(false);
            isSelecting = false;

            //check if nothing is selected using box, and the player meant to 
            //only click on a single object
            //get what is directly selected
            if (selectedObjects.Count == 0 && !actionAfterSelectionCompleted) {
                GameObject underMouse = shootRay();
                if (underMouse.GetComponent(typeof(PlayerUnit)) != null) {
                    selectedObjects.Add(underMouse.transform);
                    underMouse.GetComponent<PlayerUnit>().selected(true);
                }
            }
            actionAfterSelectionCompleted = false;

            //change cursor
            if (selectedObjects.Count > 0) {
                Cursor.SetCursor(cursorMove, Vector2.zero, CursorMode.Auto);
            } else {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
    }

    /// <summary>
    /// when player right click, check if there are selected units -> move to target
    /// otherwise it is intial click, show selection box
    /// </summary>
    private void checkIfSelectOrDoAction() {
        //check if there are already selected objects
        if (selectedObjects.Count > 0) {
            moveObectToNewLocatin();
            deselectObjects();
            actionAfterSelectionCompleted = true;
        } else {
            showSelectionBox(true);
            isSelecting = true;
        }
    }

    /// <summary>
    /// show a UI selection box when the player click and drag to boxselect
    /// </summary>
    /// <param name="show"></param>
    private void showSelectionBox(bool show) {

        if (show) {
            //make select box go to mouse pos
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = 1;
            selectionBox.position = manager.UICam.ScreenToWorldPoint(screenPoint);

            selectionBox.transform.gameObject.SetActive(true);
        } else {
            selectionBox.transform.gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// resizing the selection box
    /// </summary>
    private void resizeSelectionBox() {
        //make select box go to mouse pos
        Vector3 currMousePos = Input.mousePosition;
        float width = currMousePos.x - mousePos1.x;
        float height = mousePos1.y - currMousePos.y;

        //if player selects from bottom to up direction, image doesn't accept negative width or height
        Vector3 selectBoxAngle = new Vector3();
        if (width < 0) {
            selectBoxAngle.y = 180;
            width = width * -1;
        } else {
            selectBoxAngle.y = 0;
        }

        if (height < 0) {
            selectBoxAngle.x = 180;
            height = height * -1;
        } else {
            selectBoxAngle.x = 0;
        }

        //set angle and size of selection box
        selectionBox.eulerAngles = selectBoxAngle;
        selectionBox.sizeDelta = new Vector2(width,height);
    }

    /// <summary>
    /// get the units inside the selection box and mark them as selected
    /// </summary>
    private void startSelectingObjects() {

        //get the corners of the box, regardless of selection direction
        Vector2 leftPoint = mousePos1;
        Vector2 rightPoint = mousePos2;

        if (leftPoint.x > rightPoint.x) {
            leftPoint = mousePos2;
            rightPoint = mousePos1;
        }

        Vector2 topPoint = mousePos1;
        Vector2 bottomPoint = mousePos2;

        if (bottomPoint.y > topPoint.y) {
            topPoint = mousePos2;
            bottomPoint = mousePos1;
        }

        //deselect prevously selected units
        deselectObjects();

        //select the units within the selection box
        selectedObjects = new List<Transform>();

        foreach (Transform tr in manager.selectables) {
            if (tr != null) {
                Vector2 worldToScreen = mainCam.WorldToScreenPoint(tr.position);
                if (worldToScreen.y <= topPoint.y && worldToScreen.y >= bottomPoint.y
                    && worldToScreen.x <= rightPoint.x && worldToScreen.x >= leftPoint.x) {
                    if (!selectedObjects.Contains(tr)) {
                        selectedObjects.Add(tr);
                        tr.GetComponent<PlayerUnit>().selected(true);
                    }

                }
            }
        }

    }

    /// <summary>
    /// deselects any selected units
    /// </summary>
    private void deselectObjects() {
        //remove any prevouse object selection
        foreach (Transform tr in manager.selectables) {
            if (tr != null) {
                tr.GetComponent<PlayerUnit>().selected(false);
            }
        }
        selectedObjects = new List<Transform>();
    }

    /// <summary>
    /// ask the selected units to move to target position
    /// </summary>
    private void moveObectToNewLocatin() {
        //get all selected units and ask them to move to target position
        Vector3 grountMoveToPos = getGroundClickPoint();
        if (grountMoveToPos.x != float.NegativeInfinity) {
            foreach (Transform tr in selectedObjects) {
                if (tr != null) {
                    tr.GetComponent<PlayerUnit>().moveTo(grountMoveToPos);
                }
            }
        }

    }

    /// <summary>
    /// get the world point where the player clicked
    /// </summary>
    RaycastHit objHit;
    private Vector3 getGroundClickPoint() {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out objHit, 500, 1 << manager.groundLayer)) {
            return objHit.point;
        }
        return new Vector3(float.NegativeInfinity, 0, 0);
    }


    /// <summary>
    /// shoot ray from mouse and get the object it hits
    /// </summary>
    private GameObject shootRay() {
        RaycastHit objHit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out objHit)) {
            return objHit.transform.gameObject;
        }
        return null;
    }
}
