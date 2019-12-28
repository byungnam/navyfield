using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipControl : Photon.MonoBehaviour, IPunObservable, IControllable, IRTSSelectable {
    
    public Image healthBar;
    public Text healthText;
    public float maxHealth;
    public float health;
    public Text playerNameText;

    private bool isAlive = true;
    


    // Network
    private List<Quaternion> n_turret_rotation;
    private List<float> n_turret_angle;
    private float n_health;

    public Controllable controllable;
    public RTSSelectable rtsselectable;


    // Network
    protected float n_velocity;
    protected float n_rudder_angle;


    private Text debug_text;

    public bool IsSelected { get => ((IRTSSelectable) rtsselectable).IsSelected; set => ((IRTSSelectable) rtsselectable).IsSelected = value; }
   

    public float snatch_value;

   



    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        controllable = new Controllable();
        rtsselectable = new RTSSelectable(transform);
    }

    void Update() {
        /*
        velocity = CalculateCurrentVelocity(target_velocity_level, max_velocity, velocity, acceleration, snatch_value);
        target_rudder_angle_level = CalculateTargetRudderLevel(transform, velocity, rudder_angle, max_rudder_angle, rudder_angle_acceleration, destination, snatch_value);
        rudder_angle = CalculateCurrentRudderAngle(target_rudder_angle_level, max_rudder_angle, rudder_angle, rudder_angle_acceleration, snatch_value);
        
        (delta_position, delta_rotation) = CalculateDelta(velocity, rudder_angle, max_velocity);
        Move(transform, delta_position, delta_rotation);
        */
    }


    private void ShowDebug() {
        /*
        float cur_rud = Mathf.Round(rudder_angle / max_rudder_angle * 100f);
        string cur_rud_str = "";
        if (cur_rud > snatch_value) {
            cur_rud_str = "우현 " + ((rudder_angle / max_rudder_angle) * 100f).ToString("F1") + "%";
        }
        else if (cur_rud < -snatch_value) {
            cur_rud_str = "좌현 " + ((rudder_angle / max_rudder_angle) * 100f).ToString("F1") + "%";
        }
        else {
            cur_rud_str = "전진";
        }

        float tar_rud = Mathf.Round(target_rudder_angle_level);
        string tar_rud_str = "";
        if (tar_rud > snatch_value) {
            tar_rud_str = "우현 " + (target_rudder_angle_level * 100f).ToString("F1") + "%";
        }
        else if (tar_rud < -snatch_value) {
            tar_rud_str = "좌현 " + (target_rudder_angle_level * 100f).ToString("F1") + "%";
        }
        else {
            tar_rud_str = "전진";
        }

        
        string flight_prepared = "";
        if (flight_decks[0].flight) {
            if (flight_decks[0].flight.prepared) {
                flight_prepared = "준비 완료";
            }
            else {
                flight_prepared = "준비중 (" + (flight_decks[0].flight.prepare_time - flight_decks[0].flight.preparing).ToString("F1") + "초)";
            }
        }
        
        debug_text.text =   "F/V 기관출력 상승/하강(후진)\n"
                            + "A/D 함포 좌/우현으로 조준\n"
                            + "Z/C 함포 좌/우 회전\n"
                            + "S/X 함포 부각 상승/하강\n"
                            + "G 함선을 화면 중앙으로\n"
                              "현재 속도: " + velocity.ToString("F1") + "knot\n"
                            + "기관 출력 목표: " + (target_velocity_level * 100f).ToString("F1") + "%\n"
                            // + "현재 방향타: " + cur_rud_str + "\n"
                            // + "방향타 목표: " + tar_rud_str + "\n"
                            + "함선 좌표: " + Mathf.Round(transform.position.x) + ", " + Mathf.Round(transform.position.z) + "ff\n"
                            + "1비행편대: " + flight_prepared
       ;
       */
        
    }


    








    /*
    private void NetworkSetTurretRotation() {
        int i = 0;
        foreach(Turret t in turrets) {
            t.SetRotation(n_turret_rotation[i]);
            i++;
        }
    }

    private void NetworkSetTurretAngle() {
        int i = 0;
        foreach (Turret t in turrets) {
            t.SetAngle(n_turret_angle[i]);
            i++;
        }
    }
    */






    void OnCollisionEnter(Collision collision) {
        Debug.Log("Ship Collision " + collision.gameObject);
        Debug.Log("Ship Collision tag" + collision.gameObject.tag);

        if (collision.gameObject.tag == "Shell") {
            Debug.Log("shell damage " + collision.gameObject.GetComponent<Shell>().damage);
            changeHp(-collision.gameObject.GetComponent<Shell>().damage);
        }
    }

    public void changeHp(float diff) {
        health = Mathf.Clamp(health + diff, 0, maxHealth);
        healthBar.fillAmount = health / maxHealth;
        healthText.text = (int) health + "/" + (int) maxHealth;

        if (health == 0) {
            isAlive = false;
            Destroy(transform.gameObject, 5);
        }
    }

    
    /*
    public List<GameObject> GetFiredShells() {
        return fired_shells;
    }

    public void AddFiredShells(GameObject o) {
        fired_shells.Add(o);
    }

    public void RemoveFiredShells(GameObject o) {
        fired_shells.Remove(o);
    }
    */

    private void Sunk() {
        isAlive = false;
        // StartCoroutine(CreateExplosion());
        Destroy(transform.gameObject, 5);
    }

    public float CalculateCurrentVelocity(float target_velocity_level, float max_velocity, float current_velocity, float acceleration, float snatch_value) {
        return ((IControllable) controllable).CalculateCurrentVelocity(target_velocity_level, max_velocity, current_velocity, acceleration, snatch_value);
    }

    public float CalculateCurrentRudderAngle(float target_rudder_angle_level, float max_rudder_angle, float current_rudder_angle, float rudder_angle_acceleration, float snatch_value) {
        return ((IControllable) controllable).CalculateCurrentRudderAngle(target_rudder_angle_level, max_rudder_angle, current_rudder_angle, rudder_angle_acceleration, snatch_value);
    }

    public (Vector3, Vector3) CalculateDelta(float current_velocity, float current_rudder_angle, float max_velocity) {
        return ((IControllable) controllable).CalculateDelta(current_velocity, current_rudder_angle, max_velocity);
    }
    
    public float GetBreakDistance(float current_velocity, float acceleration) {
        return ((IControllable) controllable).GetBreakDistance(current_velocity, acceleration);
    }

    public float CalculateTargetRudderLevel(Transform transform, float current_velocity, float current_rudder_angle, float max_rudder_angle, float rudder_angle_acceleration, Vector3 destination, float snatch_value) {
        return ((IControllable) controllable).CalculateTargetRudderLevel(transform, current_velocity, current_rudder_angle, max_rudder_angle, rudder_angle_acceleration, destination, snatch_value);
    }

    public void Move(Transform transform, Vector3 delta_position, Vector3 delta_rotation) {
        ((IControllable) controllable).Move(transform, delta_position, delta_rotation);
    }

    public void OnEnable() {
        ((IRTSSelectable) rtsselectable).OnEnable();
    }

    public void OnDisable() {
        ((IRTSSelectable) rtsselectable).OnDisable();
    }



    
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            /*
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(current_velocity);
            stream.SendNext(current_rudder_angle);
            */

            //stream.SendNext(target_velocity_level);
            //stream.SendNext(current_velocity_level);
            //stream.SendNext(max_velocity);
            // stream.SendNext(velocity);
            //stream.SendNext(target_velocity);
            //stream.SendNext(acceleration);

            //stream.SendNext(target_rudder_angle_level);
            //stream.SendNext(current_rudder_angle_level);
            //stream.SendNext(max_rudder_angle);
            //stream.SendNext(rudder_angle);
            //stream.SendNext(target_rudder_angle);
            //stream.SendNext(rudder_angle_acceleration);
            // foreach (Turret t in turrets) {
            // stream.SendNext(t.transform.rotation);
            // stream.SendNext(t.GetAngle());
            // }
        }
        else {
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
            // velocity = (float) stream.ReceiveNext();
            //target_velocity = (float) stream.ReceiveNext();
            //acceleration = (float) stream.ReceiveNext();

            //target_rudder_angle_level = (float) stream.ReceiveNext();
            //current_rudder_angle_level = (float) stream.ReceiveNext();
            //max_rudder_angle = (float) stream.ReceiveNext();
            //rudder_angle = (float) stream.ReceiveNext();
            //target_rudder_angle = (float) stream.ReceiveNext();
            //rudder_angle_acceleration = (float) stream.ReceiveNext();
            // foreach (Turret t in turrets) {
            //n_turret_rotation.Add((Quaternion) stream.ReceiveNext());
            //n_turret_angle.Add((float) stream.ReceiveNext());

            // t.SetRotation((Quaternion) stream.ReceiveNext());
            // t.SetAngle((float) stream.ReceiveNext());
            // }
            Debug.Log("RECV");
        }
    }
}
