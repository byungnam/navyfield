using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : Photon.PunBehaviour {

    public GameObject room_list_content;
    public Button room_button;
    public Button create_room_button;
    public InputField room_name_input_field;

    private List<Button> _rooms;
    private List<string> _removed_rooms;
    private List<string> _players;
    private int nextUpdate = 1;
    private int next_roomno = 0;

    private List<string> room_names;
    private List<string> previous_room_names;

    // Use this for initialization
    void Start() {
        // bool lobby = PhotonNetwork.JoinLobby();
        create_room_button.onClick.AddListener(() => { CreateRoom(room_name_input_field.text); });
        previous_room_names = new List<string>();
        Debug.Log("lobby joined");
        OnReceivedRoomListUpdate();
    }

    // Update is called once per frame
    void Update() {
       
    }
        

    public void CreateRoom(string room_name) {
        Debug.Log("Create room " + room_name);
        RoomOptions ops = new RoomOptions();
        ops.MaxPlayers = 8;
        
        if (PhotonNetwork.CreateRoom(room_name, ops, null)) {
            Debug.Log("SUCCESS");
        } else {
            Debug.Log("FAILED");
        }
    }

    public void JoinRoom(string room_name) {
        Debug.Log("Joined room " + room_name);
        PhotonNetwork.JoinRoom(room_name);
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("WaitingRoom");
    }
    
    public override void OnReceivedRoomListUpdate() {
        room_names = new List<string>();
        foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {
            room_names.Add(room.Name);
            int room_idx = previous_room_names.FindIndex(x => x == room.Name);
            if (room_idx == -1) {
                Button room_entity = Instantiate<Button>(room_button, room_list_content.transform);
                Text[] texts = room_entity.GetComponentsInChildren<Text>();
                texts[0].text = ""; // + next_roomno;
                // next_roomno++;
                texts[1].text = room.Name;
                room_entity.onClick.AddListener(() => { JoinRoom(texts[1].text); });
            } else {
                previous_room_names.RemoveAt(room_idx);
            }
        }
        
        foreach (Button room_entity in room_list_content.GetComponentsInChildren<Button>()) {
            foreach (Text room_name_text in room_entity.GetComponentsInChildren<Text>()) {
                if (previous_room_names.FindIndex(x => x == room_name_text.text) >= 0) {
                    Destroy(room_entity.gameObject);
                }
            }
        }
        Debug.Log("room list updated");

        previous_room_names = room_names;
    }

}
