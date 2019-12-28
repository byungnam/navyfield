using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour {

    public GameObject canvas;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseOver() {
        canvas.SetActive(true);
    }

    private void OnMouseExit() {
        canvas.SetActive(false);
    }
}
