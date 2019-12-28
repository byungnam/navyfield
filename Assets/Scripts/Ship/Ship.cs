using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : Photon.MonoBehaviour
{
    public List<GameObject> explosions;


    public Image healthBar;
    public Text healthText;
    public float maxHealth;
    public float health;


     // public Image playerNameBg;
    public Text playerNameText;

    private ShipControl control;
    


    void Start()
    {
        /*
        healthText.text = (int) maxHealth + "/" + (int) maxHealth;
        playerNameText.text = PhotonNetwork.player.NickName;
        */
        DontDestroyOnLoad(this.gameObject);

        if (photonView.isMine) {
            control = GetComponent<ShipControl>();
            GameObject.Find("GlobalControl").GetComponent<GlobalControl>().ship_control = control;
        }
    }

    void Update() {
        
    }
}
