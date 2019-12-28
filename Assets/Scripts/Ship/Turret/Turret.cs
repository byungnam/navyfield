using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Photon.MonoBehaviour, IPunObservable {

    #region public

    public bool isForward;
    public GameObject muzzle_flash;
    public GameObject muzzle_smoke;
    public GameObject small_dot;
    public GameObject large_dot;
    public float horizontal_rotation_speed;
    public float vertical_rotation_speed;
    public List<GameObject> drop_points;

    #endregion


    #region private

    private GameObject aim_line;
    private GameObject angle_line;
    private float barrel_length = 20;
    private float spread = 0.5f;
    private float fire_delay = 1f;
    private float remaining_fire_delay = 0f;
    private List<GameObject> fired_shells;
    private int muzzle_velocity = 1000;
    private ShipControl ship_control;
    private AudioSource fire_sound;
    private GameObject aim_line_instance;
    private GameObject angle_line_instance;
    private GameObject main_camera;
    private int aim_length = 16000;
    private int text_pos_interval = 1000;
    private int small_dot_pos_interval = 100;
    private int text_min_size = 200;
    private int text_max_size = 1000;
    private float angle;

    private bool isQuickMode = false;
    private int quickModeMultiplier = 5;
    public float turretRotationSpeed = 0.4f;


    #endregion


    #region network

    private Quaternion n_rotation;
    private float n_angle;

    #endregion


    void Awake() {
        if (photonView.isMine) {
            aim_line = Resources.Load<GameObject>("Turrets/AimLine");
            angle_line = Resources.Load<GameObject>("Turrets/AngleLine");

            aim_line_instance = Instantiate(aim_line, transform);
            aim_line_instance.name = "AimLine";
            aim_line_instance.GetComponent<LineRenderer>().SetPositions(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, aim_length) });
            aim_line_instance.GetComponent<LineRenderer>().material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));

            angle_line_instance = Instantiate(angle_line, transform);
            angle_line_instance.name = "AngleLine";
            angle_line_instance.GetComponent<LineRenderer>().SetPositions(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, aim_length) });
            angle_line_instance.GetComponent<LineRenderer>().material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));

            /* draw distance text and cube */
            for (int pos = 1; pos < aim_length; pos++) {
                if (pos % text_pos_interval == 0) {
                    GameObject text_go = new GameObject("text_" + pos);
                    text_go.transform.parent = aim_line_instance.transform;
                    TextMesh text = text_go.AddComponent<TextMesh>();
                    text.text = "" + pos;
                    text.fontSize = 100;
                    text_go.transform.localPosition = new Vector3(0, 0, pos);
                    text_go.transform.Rotate(new Vector3(90, 0, 0));
                    if (!isForward) {
                        text_go.transform.Rotate(new Vector3(0, 0, 180));
                    }
                }
                /*
                else if (pos % large_dot_pos_interval == 0)
                {
                    GameObject large_dot_go = Instantiate(large_dot, aim_line_instance.transform);
                    large_dot_go.transform.localScale = new Vector3(10, 10, 10);
                    large_dot_go.GetComponent<Renderer>().material.color = new Color(0.7f, 0, 0);
                    large_dot_go.transform.localPosition = new Vector3(0, 0, pos);
                }
                */
                else if (pos % small_dot_pos_interval == 0) {
                    GameObject small_dot_go = Instantiate(small_dot, aim_line_instance.transform);
                    small_dot_go.transform.localScale = new Vector3(10, 10, 10);
                    small_dot_go.GetComponent<Renderer>().material.color = new Color(0.3f, 0.7f, 0);
                    small_dot_go.transform.localPosition = new Vector3(0, 0, pos);
                }
            }
            fired_shells = new List<GameObject>();
        }
        drop_points = new List<GameObject>();
    }


    void Start() {
        main_camera = Camera.main.gameObject;

        ship_control = GetComponentInParent<ShipControl>();
        fire_sound = GetComponent<AudioSource>();

    }

    void Update() {
        if (photonView.isMine) {
            if (remaining_fire_delay > 0) {
                remaining_fire_delay -= Time.deltaTime;
                if (remaining_fire_delay < 0) {
                    remaining_fire_delay = 0;
                }
            }

            /* line width and text resize relative to the camera distance */
            // aim_line.widthMultiplier = main_camera.transform.position.y / 500f;
            // angle_line.widthMultiplier = main_camera.transform.position.y / 500f;
            foreach (TextMesh text in GetComponentsInChildren<TextMesh>()) {
                text.fontSize = Mathf.RoundToInt(Mathf.Clamp(main_camera.transform.position.y / 2f, text_min_size, text_max_size));
            }
            RemoveDropPoints();
        } else {
            SetRotation(n_rotation);
            SetAngle(n_angle);
        }

        if (Input.GetKey(KeyCode.Z)) {
            Rotate(isQuickMode ? quickModeMultiplier * -turretRotationSpeed : -turretRotationSpeed);
        }
        if (Input.GetKey(KeyCode.C)) {
            Rotate(isQuickMode ? quickModeMultiplier * turretRotationSpeed : turretRotationSpeed);
        }
        if (Input.GetKey(KeyCode.S)) {
            ChangeAngle(isQuickMode ? quickModeMultiplier * -turretRotationSpeed : -turretRotationSpeed);
        }
        if (Input.GetKey(KeyCode.X)) {
            ChangeAngle(isQuickMode ? quickModeMultiplier * turretRotationSpeed : turretRotationSpeed);
        }
        if (Input.GetKey(KeyCode.A)) {
            GatherTurret(isQuickMode ? quickModeMultiplier * -turretRotationSpeed : -turretRotationSpeed);
        }
        if (Input.GetKey(KeyCode.D)) {
            GatherTurret(isQuickMode ? quickModeMultiplier * turretRotationSpeed : turretRotationSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            isQuickMode = !isQuickMode;
        }
        if (Input.GetKey(KeyCode.LeftControl)) {
            Fire();
        }

        // former_rotation = 
    }

    private void LateUpdate() {
        FixTurretRotation();
    }

    private void FixTurretRotation() {
        transform.LookAt(transform.position + transform.forward);
    }

    void GatherTurret(float delta_rotation) {
        if (isForward) {
            Rotate(delta_rotation);
        }
        else {
            Rotate(-delta_rotation);
        }
    }


    public void ChangeAngle(float delta) {
        foreach (Transform child in transform) {
            if (child.name == "GunBarrel" || child.name == "AngleLine") {
                child.Rotate(delta, 0, 0);
            }
        }
        angle += delta;
    }

    public float GetAngle() {
        return angle;
    }

    public void SetAngle(float a) {
        foreach (Transform child in transform) {
            if (child.name == "GunBarrel" || child.name == "AngleLine") {
                float lerp_angle = Mathf.MoveTowardsAngle(child.localRotation.eulerAngles.x, a, Time.deltaTime * vertical_rotation_speed);
                child.localRotation = Quaternion.Euler(lerp_angle, 0, 0);
            }
        }
    }

    public float GetBarrelLength() {
        return barrel_length;
    }

    public void Rotate(float delta) {
        transform.Rotate(0, delta, 0);
    }

    public void SetRotation(Quaternion r) {
        // transform.rotation = Quaternion.Euler(0, r, 0);

        Quaternion lerp_rotation = Quaternion.RotateTowards(transform.rotation, r, Time.deltaTime * horizontal_rotation_speed);
        transform.rotation = lerp_rotation;
    }

    private void RemoveDropPoints() {
        while (drop_points.Count > 6) {
            GameObject drop_point_instance = drop_points[0];
            Destroy(drop_point_instance);
            drop_points.RemoveAt(0);
        }
    }

    public List<GameObject> GetFiredShells() {
        return fired_shells;
    }

    public void AddFiredShells(GameObject o) {
        fired_shells.Add(o);
    }

    public void RemoveFiredShells(GameObject o) {
        fired_shells.Remove(o);
    }


    public void Fire() {
        if (remaining_fire_delay == 0) {
            remaining_fire_delay = fire_delay;
            foreach (Transform gun_barrel in transform) {
                if (gun_barrel.name != "GunBarrel") {
                    continue;
                }
                Vector3 dispersion = Random.insideUnitCircle * spread;
                photonView.RPC("RPCMakeShell", PhotonTargets.All, gun_barrel.position, gun_barrel.forward, gun_barrel.rotation, dispersion);
            }
            photonView.RPC("RPCPlayFireSound", PhotonTargets.All);
        }
    }

   
    [PunRPC]
    private void RPCMakeShell(Vector3 gun_barrel_position, Vector3 gun_barrel_forward, Quaternion gun_barrel_rotation, Vector3 dispersion) {
        Vector3 muzzle_position = gun_barrel_position + gun_barrel_forward * barrel_length;
        GameObject shell = Instantiate((GameObject) Resources.Load("Turrets/Shells/Shell"), muzzle_position, gun_barrel_rotation);
        Physics.IgnoreCollision(shell.GetComponent<Collider>(), GetComponentInParent<Collider>());
        shell.GetComponent<Shell>().SetFiredTurret(this);
        if (photonView.isMine) {
            fired_shells.Add(shell);
        }
        // ship_control.AddFiredShells(shell);

        shell.transform.Rotate(dispersion, Space.Self);
        Rigidbody rb = shell.GetComponent<Rigidbody>();

        rb.velocity = shell.transform.forward * muzzle_velocity;
        shell.GetComponent<Shell>().ship_control = ship_control;

        GameObject muzzle_flash = Instantiate((GameObject) Resources.Load("Turrets/Effects/MuzzleFlashEffect"), muzzle_position, gun_barrel_rotation);
        muzzle_flash.transform.parent = transform;
        muzzle_flash.transform.Translate(new Vector3(0, 0, 5));
        Destroy(muzzle_flash, 6);

        GameObject muzzle_smoke = Instantiate((GameObject) Resources.Load("Turrets/Effects/MuzzleSmoke"), muzzle_position, gun_barrel_rotation);
        muzzle_smoke.transform.parent = transform;
        Destroy(muzzle_smoke, 10);
    }

    [PunRPC]
    private void RPCPlayFireSound() {
        GetComponent<AudioSource>().Play();
    }


    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext(transform.rotation);
            stream.SendNext(angle);
        } else {
            n_rotation = (Quaternion) stream.ReceiveNext();
            n_angle = (float) stream.ReceiveNext();
            // Debug.Log(n_rotation + " " + n_angle);
        }
    }
}
