using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : Photon.PunBehaviour {

    string _gameVersion = "1";
    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    bool isConnecting;

    public GameObject controlPanel;
    public GameObject progressLabel;
    public InputField nameInputField;
    public Button loginButton;

    // Use this for initialization
    void Start() {
        progressLabel.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    void Awake() {
        PhotonNetwork.logLevel = PhotonLogLevel.Informational;

        PhotonNetwork.autoJoinLobby = true;

        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;
    }

    public void Connect() {
        isConnecting = true;
        progressLabel.SetActive(true);
        nameInputField.interactable = false;
        loginButton.interactable = false;

        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.connected) {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
            PhotonNetwork.JoinLobby();
        }
        else {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }
    }

    public void Test() {
        PhotonNetwork.LoadLevel("TestOnline");
    }

    public override void OnJoinedLobby() {
        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnDisconnectedFromPhoton() {
        Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
        progressLabel.SetActive(false);
        nameInputField.interactable = true;
        loginButton.interactable = true;
    }
}
