using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoom : Photon.PunBehaviour {
    public GameObject player_list_content;
    public Text player_name_text;
    
    // Use this for initialization
    void Start () {
        ShowPlayersInRoom();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void ShowPlayersInRoom() {
        Debug.Log("BB : " + PhotonNetwork.room.PlayerCount);
        foreach (PhotonPlayer player in PhotonNetwork.playerList) {
            Text player_name_entity = Instantiate<Text>(player_name_text, player_list_content.transform);
            player_name_entity.text = player.NickName;
            if (player.IsMasterClient) {
                player_name_entity.color = new Color(255, 255, 0);
            }
        }
    }

    public override void OnLeftRoom() {
        PhotonNetwork.LoadLevel("Lobby");
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer other) {
        Text player_name_entity = Instantiate<Text>(player_name_text, player_list_content.transform);
        player_name_entity.text = other.NickName;
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other) {
        string master = null;
        if (other.IsMasterClient) {
            foreach (PhotonPlayer player in PhotonNetwork.playerList) {
                if (player.IsMasterClient) {
                    master = player.NickName;
                }
            }
        }
        foreach (Text player_name_entity in player_list_content.GetComponentsInChildren<Text>()) {
            if (player_name_entity.text == other.NickName) {
                Destroy(player_name_entity.gameObject);
            }
            if (master != null && player_name_entity.text == master) {
                player_name_entity.color = new Color(255, 255, 0);
            }
        }
    }

    public void Go() {
        PhotonNetwork.LoadLevel("Ocean");
    }
}
