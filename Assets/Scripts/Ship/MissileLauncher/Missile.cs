using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    private List<Vector3> waypoints;
    private LineRenderer dest_line;

    public float damage = 10000;

    public GameObject explosion;
    public GameObject water_impact;

    public float rotation_speed;
    public float acceleration;

    public float max_velocity;

    float current_velocity;
    float target_velocity;
    float target_velocity_level = 1f;

    public float proximity;

    Vector3 delta_position;
    Vector3 delta_rotation;

    Vector3 destination;

    public float waypoint_distance = 700;
    public float skimming_height = 30;
    public float spread;

    private List<string> collision_tag_list;
    private LineRenderer vertical_position_line;

    private AudioSource sound;

    public int num_waypoint_rise;
    public float num_waypoint_freq_down;

    public float prob_not_evasive_movement;

    void Awake() {
        waypoints = new List<Vector3>();
        waypoints.Add(transform.position);
        // waypoints.Add(transform.position + new Vector3(0, waypoint_distance, 0));
        // waypoints.Add(transform.position + new Vector3(200, 200, 0));
        
        dest_line = GetComponent<LineRenderer>();
        

    }
    void Start () {
        collision_tag_list = new List<string>();
        collision_tag_list.Add("Terrain");
        collision_tag_list.Add("Ship");
        collision_tag_list.Add("Turret");


        GameObject heightLine = new GameObject();
        heightLine.transform.position = transform.position;
        heightLine.transform.parent = transform;
        vertical_position_line = heightLine.AddComponent<LineRenderer>();
        vertical_position_line.startWidth = 1f;
        vertical_position_line.endWidth = 1f;
        vertical_position_line.startColor = Color.green;
        vertical_position_line.endColor = Color.green;
        vertical_position_line.material.SetColor("_Color", Color.green);
        vertical_position_line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        vertical_position_line.receiveShadows = false;
        vertical_position_line.useWorldSpace = true;
        vertical_position_line.transform.parent = transform;

        sound = GetComponentInChildren<AudioSource>();

        Destroy(gameObject, 60);
    }
	
	// Update is called once per frame
	void Update () {
        vertical_position_line.SetPositions(new Vector3[] { transform.position, new Vector3(transform.position.x, 0, transform.position.z) });

        Autopilot();
        CalculateCurrentVelocity();
        CalculateDelta();
        Move();
        FadeOutSound();
    }



    private void FadeOutSound() {
        IEnumerator fadeSound1 = Utils.FadeOut(sound, 5f);
        StartCoroutine(fadeSound1);
        StopCoroutine(fadeSound1);
    }

    public void SetDestination(Vector3 destination) {
        Vector3 xz_direction = destination - transform.position;
        xz_direction.y = 0;

        Vector3 world_peak_position = transform.position + new Vector3(0, waypoint_distance, 0) + waypoint_distance / 2 * xz_direction.normalized;

        /*
        for (int i = 1; i <= num_waypoint_rise; i++) {
            Vector3 dispersion = Random.insideUnitSphere * spread;
            dispersion.y = 0;
            Vector3 ship_to_peak = Vector3.Lerp(transform.position, transform.position + new Vector3(xz_direction.normalized.x * waypoint_distance / 2, waypoint_distance, xz_direction.normalized.z * waypoint_distance / 2), i/(float)num_waypoint_rise);
            waypoints.Add(ship_to_peak + dispersion);
        }
        */


        // waypoints.Add(transform.position + world_peak_position);
        waypoints.Add(world_peak_position);
        waypoints.Add(Vector3.Lerp(world_peak_position, destination, 0.2f));
        // waypoints.Add(transform.position + new Vector3(0, waypoint_distance, 0) + waypoint_distance*2 * xz_direction.normalized);
        // waypoints.Add(transform.position + new Vector3(0, skimming_height, 0) + waypoint_distance * xz_direction.normalized);

        int num_waypoint_down = (int) ((Vector3.Lerp(world_peak_position, destination, 0.2f) - destination).magnitude / (waypoint_distance / num_waypoint_freq_down));
        for (int i = 1; i < num_waypoint_down - 1; i++) {
            Vector3 dispersion = Random.insideUnitSphere * spread;
            dispersion.y = 0;
            if (Random.value < prob_not_evasive_movement) {
                dispersion = Vector3.zero;
            }
            Vector3 peak_to_dest = Vector3.Lerp(Vector3.Lerp(world_peak_position, destination, 0.2f), destination, i/(float)num_waypoint_down);
            
            waypoints.Add(peak_to_dest + dispersion);
        }
        
        waypoints.Add(destination + new Vector3(0, 0, 0));
        dest_line.positionCount = waypoints.Count;
        dest_line.SetPositions(waypoints.ToArray());
        Vector3[] tmp = new Vector3[dest_line.positionCount];
        dest_line.GetPositions(tmp);
        
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log("missile collision " + collision.gameObject.name + " " + collision.gameObject.tag);

        if (collision_tag_list.Contains(collision.gameObject.tag)) {
            Explode(collision.gameObject.tag);
        }
    }

    void Explode(string tag) {
        GameObject exp = null;
        if (tag == "Terrain") {
            exp = Instantiate(water_impact, transform.position, Quaternion.identity);
        }
        else if (tag == "Ship" || tag == "Turret") {
            exp = Instantiate(explosion, transform.position, Quaternion.identity);
           
        }
        Destroy(gameObject);
        
    }

    private void Autopilot() {
        if (waypoints.Count > 0) {
            destination = waypoints[0];
        }
        float distance = (destination - transform.position).magnitude;
        if (distance < proximity) {
            waypoints.Remove(destination);
        }

        float angle_between_forward_dest = Vector3.Angle(transform.forward, destination - transform.position);
        float angle_dir = AngleDir(transform.forward, destination - transform.position, transform.up);
       
    }

    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
        Vector3 right = Vector3.Cross(up, fwd);        // right vector
        float dir = Vector3.Dot(right, targetDir);

        if (dir > 0f) {
            return 1f;
        }
        else if (dir < 0f) {
            return -1f;
        }
        else {
            return 0f;
        }
    }


    private void CalculateCurrentVelocity() {
        target_velocity = target_velocity_level * max_velocity;
        current_velocity += Mathf.Sign(target_velocity - current_velocity) * acceleration * Time.deltaTime;
    }

    private void CalculateDelta() {
        delta_position = new Vector3(0, 0, current_velocity * Time.deltaTime);
        
    }

    void Move() {
        if (delta_position.sqrMagnitude != 0) {
            transform.Translate(delta_position);
        }

        Vector3 r = Vector3.RotateTowards(transform.forward, destination - transform.position, Time.deltaTime * rotation_speed, 0f);
        transform.rotation = Quaternion.LookRotation(r);
    }

    void OnDisable() {
        ParticleSystem r = GetComponentInChildren<ParticleSystem>();
        r.transform.parent = null;
        r.Stop();
    }
}
