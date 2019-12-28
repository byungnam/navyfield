using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirplaneControl : Photon.MonoBehaviour, IPunObservable {


    public GameObject[] explosions;

    private float health;
    private float maxHealth;

    private bool isAlive;

    private float knot = 0.51444f;

    private float target_velocity_level = 1;
    public float max_velocity = 180f;
    private float current_velocity;
    private float target_velocity;
    public float acceleration = 30f;
    private Vector3 delta_position = Vector3.zero;

    private float target_rudder_angle_level;
    public float max_rudder_angle = 15f;
    private float current_rudder_angle;
    private float target_rudder_angle;
    public float rudder_angle_acceleration = 5f;
    private Vector3 delta_rotation = Vector3.zero;

    private Vector3 autopilot_destination;
    private bool autopilot;

    private float altitude;
    private float target_altitude = 1000;


    // Network
    private float n_current_velocity;
    private float n_current_rudder_angle;
    private float n_health;

    private float snatch_value = 0.5f;


    void Awake() {
        
    }

    void Start() {
        if (photonView.isMine) {
            isAlive = true;
        }
        else {

        }
        PhotonPlayer owner = photonView.owner;
        altitude = transform.position.y;
    }

    void Update() {
        if (photonView.isMine) {
            

            UpdateMovement();
        }
        else {
            UpdateNetworkedMovement();
        }
    }

    private void UpdateMovement() {
        if (isAlive) {
            KeyboardInputs();
            MouseInputs();
            Autopilot();
        }
        else {
            // target_velocity_level = 0;
            // target_rudder_angle_level = 0;
        }
        CalculateCurrentVelocity();
        CalculateCurrentRudderAngle();
        CalculateDelta();
        Move();
    }

    private void UpdateNetworkedMovement() {
        current_velocity = Mathf.MoveTowards(current_velocity, n_current_velocity, Time.deltaTime * acceleration);
        current_rudder_angle = Mathf.MoveTowardsAngle(current_rudder_angle, n_current_rudder_angle, Time.deltaTime * rudder_angle_acceleration);
        CalculateDelta();
        Move();
    }

    private void CalculateCurrentVelocity() {
        target_velocity = target_velocity_level * max_velocity;
        if (target_velocity - current_velocity > 0) {
            current_velocity += knot * acceleration * Time.deltaTime;
        }
        else if (target_velocity - current_velocity < 0) {
            current_velocity -= knot * acceleration * Time.deltaTime;
        }
        if (Mathf.Abs(target_velocity - current_velocity) < snatch_value) {
            current_velocity = target_velocity;
        }
    }

    private void CalculateCurrentRudderAngle() {
        target_rudder_angle = target_rudder_angle_level * max_rudder_angle;
        //current_rudder_angle = Mathf.LerpUnclamped(0, max_rudder_angle, current_rudder_angle_level);
        current_rudder_angle += Mathf.Sign(target_rudder_angle - current_rudder_angle) * rudder_angle_acceleration * Time.deltaTime;
        if (Mathf.Abs(target_rudder_angle - current_rudder_angle) < snatch_value) {
            current_rudder_angle = target_rudder_angle;
        }
    }

    private void CalculateDelta() {
        delta_position = new Vector3(0, 0, current_velocity * Time.deltaTime);
        delta_rotation = new Vector3(0, current_rudder_angle * (current_velocity / max_velocity) * Time.deltaTime, 0);
    }


    private void KeyboardInputs() {
        if (Input.GetKeyDown(KeyCode.Semicolon)) {
            
        }
    }
    
    private void MouseInputs() {
        if (Input.GetMouseButtonDown(1)) {
            autopilot_destination = GetMouseToWorldPosition() + new Vector3(0, target_altitude, 0);
            autopilot = true;
        }
    }

    private Vector3 GetMouseToWorldPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane world = new Plane(new Vector3(0, 1, 0), new Vector3(0, 0, 0));
        float distance;
        world.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    private void Autopilot() {
        if (autopilot) {
            float distance = (autopilot_destination - transform.position).magnitude;
            if (distance < 50f) {
                autopilot = false;
                target_rudder_angle_level = Mathf.Sign(current_rudder_angle);
            }
        }
        else {
            if (Mathf.Abs(current_rudder_angle) < 0.05) {
                current_rudder_angle = 0f;
            }
        }
        
        if (autopilot) {
            float angle_between_forward_dest = Vector3.Angle(transform.forward, autopilot_destination - transform.position);
            float angle_dir = AngleDir(transform.forward, autopilot_destination - transform.position, transform.up);
            float breaking_time = current_rudder_angle / rudder_angle_acceleration;
            float breaking_distance = breaking_time * current_rudder_angle / 2f;

            if (breaking_distance >= angle_between_forward_dest) {
                target_rudder_angle_level = 0f;
            }
            else {
                target_rudder_angle_level = Mathf.Clamp((angle_between_forward_dest / max_rudder_angle) * angle_dir, -1, 1);
            }
            // Debug.Log("angle: " + angle_between_forward_dest * angle_dir + "\nbreaking_time: " + breaking_time + "\nbreaking_distance: " + breaking_distance + "\nrudder_angle_level: " + target_rudder_angle_level);
        }
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

    void CenterCamera() {
        float rad = Vector3.Angle(new Vector3(0, -1, 0), Camera.main.transform.forward) * Mathf.Deg2Rad;
        Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z - Mathf.Tan(rad) * Camera.main.transform.position.y);
        
    }

    void Move() {
        if (delta_position.sqrMagnitude != 0) {
            transform.Translate(delta_position);
        }
        if (delta_rotation.sqrMagnitude != 0) {
            transform.Rotate(delta_rotation);
        }
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log("Ship Collision " + collision.gameObject);
        if (collision.gameObject.tag == "Shell") {
            if (photonView.isMine) {
                ChangeHp(collision.gameObject.GetComponent<Shell>().damage);
            }
        }
    }

    public void ChangeHp(float diff) {
        if (diff < 0) {
            health += Mathf.Min(diff, health);
        }
        else {
            health += Mathf.Min(diff, maxHealth - health);
        }
        /*
        healthBar.fillAmount = health / maxHealth;
        healthText.text = (int) health + "/" + (int) maxHealth;
        */
        if (health <= 0) {
            Sunk();
        }
    }
    
    private void Sunk() {
        isAlive = false;
        StartCoroutine(CreateExplosion());
        Destroy(transform.gameObject, 5);
    }

    IEnumerator CreateExplosion() {
        for (int i = 0; i < 20; i++) {
            for (int j = 0; j < UnityEngine.Random.Range(1, 3); j++) {
                GameObject e = explosions[UnityEngine.Random.Range(0, 2)];
                Vector3 p = transform.forward * UnityEngine.Random.Range(-70, 100) +
                transform.right * UnityEngine.Random.Range(-10, 10);
                GameObject explosion = Instantiate(e, transform.position + p, Quaternion.identity);
                float scale = UnityEngine.Random.Range(1f, 10f);
                explosion.transform.localScale = new Vector3(scale, scale, scale);
                Destroy(explosion, 5f);
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.05f));
        }
    }
    
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext(health);
            /*
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(current_velocity);
            stream.SendNext(current_rudder_angle);
            */

            //stream.SendNext(target_velocity_level);
            //stream.SendNext(current_velocity_level);
            //stream.SendNext(max_velocity);
            stream.SendNext(current_velocity);
            //stream.SendNext(target_velocity);
            //stream.SendNext(acceleration);

            //stream.SendNext(target_rudder_angle_level);
            //stream.SendNext(current_rudder_angle_level);
            //stream.SendNext(max_rudder_angle);
            stream.SendNext(current_rudder_angle);
            //stream.SendNext(target_rudder_angle);
            //stream.SendNext(rudder_angle_acceleration);
        }
        else {
            n_health = (float) stream.ReceiveNext();
            /*
            Vector3 recv_position = (Vector3) stream.ReceiveNext();
            Quaternion recv_rotation = (Quaternion) stream.ReceiveNext();

            float recv_current_velocity = (float) stream.ReceiveNext();
            float recv_current_rudder_angle = (float) stream.ReceiveNext();
            */

            /*
            float recv_target_velocity_level = (float) stream.ReceiveNext();
            float recv_current_velocity_level = (float) stream.ReceiveNext();
            float recv_max_velocity = (float) stream.ReceiveNext();
            float recv_current_velocity = (float) stream.ReceiveNext();
            float recv_target_velocity = (float) stream.ReceiveNext();
            float recv_acceleration = (float) stream.ReceiveNext();
            
            float recv_target_rudder_angle_level = (float) stream.ReceiveNext(); 
            float recv_current_rudder_angle_level = (float) stream.ReceiveNext(); 
            float recv_max_rudder_angle = (float) stream.ReceiveNext();
            float recv_current_rudder_angle = (float) stream.ReceiveNext(); 
            float recv_target_rudder_angle = (float) stream.ReceiveNext();
            float recv_rudder_angle_acceleration = (float) stream.ReceiveNext();
            */

            //target_velocity_level = (float) stream.ReceiveNext();
            //current_velocity_level = (float) stream.ReceiveNext();
            //max_velocity = (float) stream.ReceiveNext();
            n_current_velocity = (float) stream.ReceiveNext();
            //target_velocity = (float) stream.ReceiveNext();
            //acceleration = (float) stream.ReceiveNext();

            //target_rudder_angle_level = (float) stream.ReceiveNext();
            //current_rudder_angle_level = (float) stream.ReceiveNext();
            //max_rudder_angle = (float) stream.ReceiveNext();
            n_current_rudder_angle = (float) stream.ReceiveNext();
            //target_rudder_angle = (float) stream.ReceiveNext();
            //rudder_angle_acceleration = (float) stream.ReceiveNext();
           
        }
    }
}
