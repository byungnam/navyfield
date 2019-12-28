using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightDeck : MonoBehaviour {
   
    private GameObject flight_go;
    public Flight flight;
    public FlightControl flight_control;

    public int flight_num;

    // private GlobalControl global_control;

    void Start () {
        // global_control = GameObject.Find("GlobalControl").GetComponent<GlobalControl>();
    }
	
	void Update () {
        if (flight_go is null) {
            flight_go = Instantiate<GameObject>((GameObject) Resources.Load("FlightDecks/Flights/Flight"), transform.position, transform.rotation, transform);
            Debug.Log(flight_go);
            flight = flight_go.GetComponent<Flight>();
            flight_control = flight_go.GetComponent<FlightControl>();

            flight.flight_num = flight_num;
            flight_control.flight_num = flight_num;

        }

        if (!flight.in_air && flight.prepared) {
            Debug.Log("flight prepared");
            if (flight_control.is_destination_set) {
                Launch();
                flight.transform.parent = null;
            }
        }
    }

    public void Launch() {
        flight.in_air = true;
        flight_control.in_air = true;
    }
}
