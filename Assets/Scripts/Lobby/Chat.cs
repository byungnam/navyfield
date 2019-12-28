using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon.Chat;
using ExitGames.Client.Photon;

public class Chat : IChatClientListener {

    public ChatClient chatClient;
    public string chatAppId;
    public string chatAppVersion = "0.1.0";

    public void DebugReturn(DebugLevel level, string message) {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state) {
        throw new System.NotImplementedException();
    }

    public void OnConnected() {
        throw new System.NotImplementedException();
    }

    public void OnDisconnected() {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages) {
        throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName) {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results) {
        throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels) {
        throw new System.NotImplementedException();
    }


    // Use this for initialization
    void Start () {
        // In the C# SDKs, the callbacks are defined in the `IChatClientListener` interface.
        // In the demos, we instantiate and use the ChatClient class to implement the IChatClientListener interface.

        chatClient = new ChatClient(this);
        // Set your favourite region. "EU", "US", and "ASIA" are currently supported.
        chatClient.ChatRegion = "EU";
        chatClient.Connect(chatAppId, chatAppVersion, new ExitGames.Client.Photon.Chat.AuthenticationValues());
        PlayerPrefs.GetString("PlayerName");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
