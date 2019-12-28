using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhotonConnect : Photon.PunBehaviour {

    public GameObject[] spawn_points;

	void Start () {
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.ConnectUsingSettings("TEST");
    }
	
	void Update () {
		
	}

    public override void OnJoinedLobby() {
        PhotonNetwork.JoinOrCreateRoom("TEST", new RoomOptions(), new TypedLobby());
        Debug.Log("connected");
    }

    public override void OnJoinedRoom() {
        bool alpha = false;
        
        if (PhotonNetwork.countOfPlayersInRooms % 2 == 1) {
            alpha = true;
        }

        List<Vector3> spwan_point = new List<Vector3>();
        foreach (GameObject sp in spawn_points) {
            if((alpha && sp.name == "AlphaTeam") || (!alpha && sp.name == "BravoTeam")) {
                foreach (Transform spot in sp.transform) {
                    spwan_point.Add(spot.position);
                }
            }
        }
        System.Random r = new System.Random();
        if (alpha) {
            PhotonNetwork.Instantiate("Ships/cruiser", spwan_point[r.Next(spwan_point.Count)], Quaternion.Euler(0, 270, 0), 0);
        } else {
            PhotonNetwork.Instantiate("Ships/cruiser", spwan_point[r.Next(spwan_point.Count)], Quaternion.Euler(0, 90, 0), 0);
        }
    }
}
