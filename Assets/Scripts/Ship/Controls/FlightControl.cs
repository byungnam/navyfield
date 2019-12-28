using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightControl : Photon.MonoBehaviour, IControllable, IRTSSelectable {

    public int flight_num;
    private float target_height = 500f;
    private GlobalControl global_control;

    public Controllable controllable;
    public RTSSelectable rtsselectable;

    public float snatch_value;

    public float target_velocity_level = 0;
    public float max_velocity = 0;
    public float velocity = 0;
    public float acceleration = 0;
    public Vector3 delta_position = Vector3.zero;

    public float target_rudder_angle_level = 0;
    public float max_rudder_angle = 0;
    public float rudder_angle = 0;
    public float rudder_angle_acceleration = 0;
    public Vector3 delta_rotation = Vector3.zero;

    public float destination_approx_distance = 50;

    public Vector3 destination;
    public bool is_destination_set;

    protected LineRenderer line;

    public bool in_air;


    public bool IsSelected { get => ((IRTSSelectable) rtsselectable).IsSelected; set => ((IRTSSelectable) rtsselectable).IsSelected = value; }

    public float CalculateCurrentRudderAngle(float target_rudder_angle_level, float max_rudder_angle, float current_rudder_angle, float rudder_angle_acceleration, float snatch_value) {
        return ((IControllable) controllable).CalculateCurrentRudderAngle(target_rudder_angle_level, max_rudder_angle, current_rudder_angle, rudder_angle_acceleration, snatch_value);
    }

    public float CalculateCurrentVelocity(float target_velocity_level, float max_velocity, float current_velocity, float acceleration, float snatch_value) {
        return ((IControllable) controllable).CalculateCurrentVelocity(target_velocity_level, max_velocity, current_velocity, acceleration, snatch_value);
    }

    public (Vector3, Vector3) CalculateDelta(float current_velocity, float current_rudder_angle, float max_velocity) {
        return ((IControllable) controllable).CalculateDelta(current_velocity, current_rudder_angle, max_velocity);
    }

    public float CalculateTargetRudderLevel(Transform transform, float current_velocity, float current_rudder_angle, float max_rudder_angle, float rudder_angle_acceleration, Vector3 destination, float snatch_value) {
        return ((IControllable) controllable).CalculateTargetRudderLevel(transform, current_velocity, current_rudder_angle, max_rudder_angle, rudder_angle_acceleration, destination, snatch_value);
    }

    public float GetBreakDistance(float current_velocity, float acceleration) {
        return ((IControllable) controllable).GetBreakDistance(current_velocity, acceleration);
    }

    public void Move(Transform transform, Vector3 delta_position, Vector3 delta_rotation) {
        ((IControllable) controllable).Move(transform, delta_position, delta_rotation);
    }

    public void OnEnable() {
        if (photonView.isMine) {
            ((IRTSSelectable) rtsselectable).OnEnable();
        }
    }

    public void OnDisable() {
        if (photonView.isMine) {
            ((IRTSSelectable) rtsselectable).OnDisable();
        }
    }

    public Vector3 GetMouseToWorldPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane world = new Plane(new Vector3(0, 1, 0), new Vector3(0, 0, 0));
        float distance;
        world.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    private void MakeWaypointLine() {
        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = 3;
        line.startWidth = 3f;
        line.endWidth = 3f;
        line.material.SetColor("_Color", Color.yellow);
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.useWorldSpace = true;
        line.transform.parent = transform;
    }


    void Awake() {
        if (photonView.isMine) {
            global_control = GameObject.Find("GlobalControl").GetComponent<GlobalControl>();
            global_control.flight_controls.Add(flight_num, this);
            MakeWaypointLine();
        }
        controllable = new Controllable();
        rtsselectable = new RTSSelectable(transform);
        target_velocity_level = 1;

        
        in_air = false;
        is_destination_set = false;

    }

    void Start() {
        if (photonView.isMine) {
            IsSelected = false;
            destination = transform.position;
        }
    }


    void Update() {
        /*
        transform.position = new Vector3(transform.position.x,
            Mathf.MoveTowards(transform.position.y, target_height, Time.deltaTime * max_velocity),
            transform.position.z);
        */

        if (photonView.isMine) {
            line.SetPosition(0, transform.position);

            if (!is_destination_set) {
                line.enabled = false;
                line.SetPosition(1, transform.position);
            }


            if (IsSelected) {
                if (Input.GetMouseButtonDown(1)) {
                    is_destination_set = true;
                    destination = GetMouseToWorldPosition();
                    destination.y = target_height;
                    controllable.destination_reached = false;
                    line.SetPosition(1, destination);
                    line.SetPosition(2, new Vector3(destination.x, 1, destination.z));
                    line.enabled = true;
                }
            }
            if ((transform.position - destination).magnitude < destination_approx_distance) {
                // controllable.destination_reached = true;
                // line.enabled = false;
            }
        }


        if (in_air) {
            velocity = CalculateCurrentVelocity(target_velocity_level, max_velocity, velocity, acceleration, snatch_value);
            target_rudder_angle_level = CalculateTargetRudderLevel(transform, velocity, rudder_angle, max_rudder_angle, rudder_angle_acceleration, destination, snatch_value);
            rudder_angle = CalculateCurrentRudderAngle(target_rudder_angle_level, max_rudder_angle, rudder_angle, rudder_angle_acceleration, snatch_value);
            (delta_position, delta_rotation) = CalculateDelta(velocity, rudder_angle, max_velocity);
            if (Mathf.Abs(transform.position.y - target_height) > snatch_value) {
                delta_position.y = 1;
            }

            Move(transform, delta_position, delta_rotation);
        }
    }

    /*
    private void SelectAirplaneGroup(int group) {
        is_airplane_control_mode = true;
        group = group - 1;
        for (int i = 0; i < flight_decks.Length; i++) {
            if (i == group) {
                if (flight_decks[i].in_air) {
                    flight_decks[i].flight.IsSelected = true;
                }
            }
            else {
                if (flight_decks[i].in_air) {
                    flight_decks[i].flight.IsSelected = false;
                }
            }
        }
    }
    */



}
