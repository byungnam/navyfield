using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockCamera : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            
        }
    }
}
