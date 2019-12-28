using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OceanGameManager : Photon.MonoBehaviour {

    const string VERSION = "0.0.1";
    public string roomName = "";

    public string playerPrefabName = "cruiser";
    public GameObject alpha_base_spot;
    public GameObject bravo_base_spot;

    public List<Vector3> alpha_spawn_point;
    public List<Vector3> bravo_spawn_point;

    public Hashtable player_in_team;

    // Use this for initialization
    void Start() {
        // PhotonNetwork.ConnectUsingSettings(VERSION);
        foreach (Transform sp in alpha_base_spot.GetComponentsInChildren<Transform>()) {
            alpha_spawn_point.Add(sp.position);
        }
        foreach (Transform sp in bravo_base_spot.GetComponentsInChildren<Transform>()) {
            bravo_spawn_point.Add(sp.position);
        }
        System.Random r = new System.Random();
        PhotonNetwork.Instantiate("cruiser", alpha_spawn_point[r.Next(alpha_spawn_point.Count)], Quaternion.identity, 0);
    }

    // Update is called once per frame
    void Update() {

    }
}
