using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shell : CameraFollowable {

    public GameObject explosion;
    public GameObject water_impact;

    public float radius = 20f;
    public float force = 700f;
    public float damage = 2000;
    public ShipControl ship_control;


    private LineRenderer line;
    private List<string> collision_tag_list;
    
    private GameObject drop_point;
    private Text position_text;

    private Turret fired_turret;
    private bool is_followed;

    private AudioHandler aHandler;
    private AudioSource audio_source;

    void Start() {
        aHandler = GameObject.Find("AudioHandler").GetComponent<AudioHandler>();
        Destroy(gameObject, 120);

        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = 1f;
        line.endWidth = 1f;
        line.startColor = Color.green;
        line.endColor = Color.green;
        line.material.SetColor("_Color", Color.green);
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.useWorldSpace = true;
        line.transform.parent = transform;


        collision_tag_list = new List<string>();
        collision_tag_list.Add("Terrain");
        collision_tag_list.Add("Ship");
        collision_tag_list.Add("Turret");

        TrailRenderer r = GetComponentInChildren<TrailRenderer>();
        r.autodestruct = true;
        drop_point = Resources.Load<GameObject>("Turrets/Effects/DropPoint");
        audio_source = GetComponent<AudioSource>();
    }

    void Update() {
        line.SetPositions(new Vector3[] { transform.position, new Vector3(transform.position.x, 0, transform.position.z) });
        transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log("shell collision " + collision.gameObject.name + " " + collision.gameObject.tag);
        
        if (collision_tag_list.Contains(collision.gameObject.tag)) {
            Explode(collision.gameObject.tag);
            // GameObject drop_point_instance = PhotonNetwork.Instantiate("Turrets/Effects/DropPoint", transform.position, Quaternion.identity, 0);
            if (fired_turret.photonView.isMine) {
                GameObject drop_point_instance = Instantiate(drop_point, transform.position, Quaternion.identity);
                if (fired_turret != null) {
                    drop_point_instance.transform.SetParent(fired_turret.transform, true);
                    fired_turret.drop_points.Add(drop_point_instance);
                }
            }
        }
    }

    public void SetFiredTurret(Turret t) {
        fired_turret = t;
    }

    public Turret GetFiredTurret() {
        return fired_turret;
    }

    void Explode(string tag) {
        GameObject exp = null;
        if (tag == "Terrain") {
            exp = Instantiate(water_impact, transform.position, Quaternion.identity);
            aHandler.PlayAudio(exp.GetComponent<AudioSource>());
        }
        else if (tag == "Ship" || tag == "Turret") {
            exp = Instantiate(explosion, transform.position, Quaternion.identity);
            aHandler.PlayAudio(exp.GetComponent<AudioSource>());
        }
        // DisableFollowCam();
        // fired_turret.RemoveFiredShells(gameObject);
        // ship_control.RemoveFiredShells(gameObject);
        Destroy(gameObject);
    }

    /*
    void DisableFollowCam() {
        if (is_followed) {
            ship_control.DisableFollowCam();
        }
    }
    */

    public bool GetIsFollowed() {
        return is_followed;
    }

    public void SetIsFollowed(bool followed) {
        is_followed = followed;
    }

    void OnDestroy() {
        TrailRenderer r = GetComponentInChildren<TrailRenderer>();
        r.transform.parent = null;
    }
}
