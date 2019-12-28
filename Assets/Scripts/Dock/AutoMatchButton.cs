using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AutoMatchButton : Photon.PunBehaviour {

    Button auto_match;
    string _gameVersion = "0.0.1";

    public GameVariables vars;
    public ShipModificationsTab left_tab;

	// Use this for initialization
	void Start () {
        auto_match = transform.GetComponent<Button>();
        auto_match.onClick.AddListener(onClick);

        // #Critical, we must first and foremost connect to Photon Online Server.
        PhotonNetwork.ConnectUsingSettings(_gameVersion);
        PhotonNetwork.automaticallySyncScene = true;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.connectedAndReady && !PhotonNetwork.inRoom)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    void onClick()
    {
        
        PhotonNetwork.LoadLevel("Field");

        // RoomOptions roomOptions = new RoomOptions() { IsVisible = true, MaxPlayers = 0 };
        // PhotonNetwork.JoinOrCreateRoom("Test", roomOptions, null);
        //PhotonNetwork.CreateRoom("Test", roomOptions, null);
        //PhotonNetwork.JoinRoom("Test");
            
    }



    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN");
    }


    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton() was called by PUN");
    }

}