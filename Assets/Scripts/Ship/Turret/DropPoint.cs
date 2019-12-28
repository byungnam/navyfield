using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropPoint : MonoBehaviour {

   Text position_text;
    // Use this for initialization
    void Start () {
        position_text = GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (position_text != null) {
            position_text.text = transform.position.ToString();
        }
    }
}
