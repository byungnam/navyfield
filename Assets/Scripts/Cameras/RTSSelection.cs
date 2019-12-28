/*
 * Copyright (c) 2016, Ivo van der Marel
 * Released under MIT License (= free to be used for anything)
 * Enjoy :)
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RTSSelection : MonoBehaviour {

    public static List<RTSSelectable> selectables = new List<RTSSelectable>();

    [Tooltip("Canvas is set automatically if not set in the inspector")]
    public Canvas canvas;
    [Tooltip("The Image that will function as the selection box to select multiple objects at the same time. Without this you can only click to select.")]
    public Image selectionBox;
    [Tooltip("The key to add/remove parts of your selection")]
    public KeyCode copyKey = KeyCode.LeftControl;

    private Vector3 startScreenPos;

    private BoxCollider worldCollider;

    private RectTransform rt;

    private bool isSelecting;

    void Awake() {
        if (canvas == null)
            canvas = FindObjectOfType<Canvas>();

        if (selectionBox != null) {
            //We need to reset anchors and pivot to ensure proper positioning
            rt = selectionBox.GetComponent<RectTransform>();
            rt.pivot = Vector2.one * .5f;
            rt.anchorMin = Vector2.one * .5f;
            rt.anchorMax = Vector2.one * .5f;
            selectionBox.gameObject.SetActive(false);
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            //Shoots a ray into the 3D world starting at our mouseposition
            if (Physics.Raycast(mouseToWorldRay, out hitInfo)) {
                //We check if we clicked on an object with a RTSSelectable component
                FlightControl fc = hitInfo.collider.GetComponentInParent<FlightControl>();
                if (fc != null) {
                    RTSSelectable s = fc.rtsselectable;
                    //While holding the copyKey, we can add and remove objects from our selection
                    if (Input.GetKey(copyKey)) {
                        //Toggle the selection
                        UpdateSelection(s, !s.IsSelected);
                    }
                    else {
                        //If the copyKey was not held, we clear our current selection and select only this unit
                        ClearSelected();
                        UpdateSelection(s, true);
                    }

                    //If we clicked on a RTSSelectable, we don't want to enable our SelectionBox
                    return;
                }
            }

            if (selectionBox == null)
                return;
            //Storing these variables for the selectionBox
            startScreenPos = Input.mousePosition;
            isSelecting = true;
        }

        //If we never set the selectionBox variable in the inspector, we are simply not able to drag the selectionBox to easily select multiple objects. 'Regular' selection should still work
        if (selectionBox == null)
            return;

        //We finished our selection box when the key is released
        if (Input.GetMouseButtonUp(0)) {
            isSelecting = false;
        }

        selectionBox.gameObject.SetActive(isSelecting);

        if (isSelecting) {
            Bounds b = new Bounds();
            //The center of the bounds is inbetween startpos and current pos
            b.center = Vector3.Lerp(startScreenPos, Input.mousePosition, 0.5f);
            //We make the size absolute (negative bounds don't contain anything)
            b.size = new Vector3(Mathf.Abs(startScreenPos.x - Input.mousePosition.x),
                Mathf.Abs(startScreenPos.y - Input.mousePosition.y),
                0);

            //To display our selectionbox image in the same place as our bounds
            rt.position = b.center;
            rt.sizeDelta = canvas.transform.InverseTransformVector(b.size);

            //Looping through all the selectables in our world (automatically added/removed through the RTSSelectable OnEnable/OnDisable)
            foreach (RTSSelectable RTSSelectable in selectables) {
                //If the screenPosition of the worldobject is within our selection bounds, we can add it to our selection
                Vector3 screenPos = Camera.main.WorldToScreenPoint(RTSSelectable.transform.position);
                screenPos.z = 0;
                UpdateSelection(RTSSelectable, (b.Contains(screenPos)));
            }
        }
    }

    /// <summary>
    /// Add or remove a RTSSelectable from our selection
    /// </summary>
    /// <param name="s"></param>
    /// <param name="value"></param>
    void UpdateSelection(RTSSelectable s, bool value) {
        if (s.IsSelected != value)
            s.IsSelected = value;
    }

    /// <summary>
    /// Returns all RTSSelectable objects with isSelected set to true
    /// </summary>
    /// <returns></returns>
    List<RTSSelectable> GetSelected() {
        return new List<RTSSelectable>(selectables.Where(x => x.IsSelected));
    }

    /// <summary>
    /// Clears the full selection
    /// </summary>
    void ClearSelected() {
        selectables.ForEach(x => x.IsSelected = false);
    }

}