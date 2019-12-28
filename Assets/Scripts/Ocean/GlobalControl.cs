using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalControl : MonoBehaviour
{
    private int flight_selection = -1;

    public ShipControl ship_control;
    public Dictionary<int, FlightControl> flight_controls = new Dictionary<int, FlightControl>();

    // public Ship ship;
    // public Dictionary<int, Flight> flights;

    

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        SelectionControl();
        if (ship_control) {

        }
    }

    private void SelectionControl() {
        if (Input.GetKey(KeyCode.Alpha1)) {
            flight_selection = 0;
        }
        else if (Input.GetKey(KeyCode.Alpha2)) {
            flight_selection = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha3)) {
            flight_selection = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha4)) {
            flight_selection = 3;
        }
        else if (Input.GetKey(KeyCode.Alpha5)) {
            flight_selection = 4;
        }
        else if (Input.GetKey(KeyCode.Alpha6)) {
            flight_selection = 5;
        }
        else if (Input.GetKey(KeyCode.Alpha7)) {
            flight_selection = 6;
        }
        else if (Input.GetKey(KeyCode.Alpha8)) {
            flight_selection = 7;
        }

        if (flight_selection >= 0 && flight_selection < flight_controls.Count) {
            ship_control.IsSelected = false;
            flight_controls[flight_selection].IsSelected = true;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (ship_control != null && flight_controls.Count > 0) {
                ship_control.IsSelected = true;
                for (int i = 0; i < flight_controls.Count; i++) {
                    flight_controls[i].IsSelected = false;
                }
                flight_selection = -1;
            }
        }
    }
}
