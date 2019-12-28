using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShellSpeedText : MonoBehaviour {

    Text text;
    float time;

	void Start () {
        text = GetComponent<Text>();
	}
	
	void Update () {
        /*
        Transform target = GetComponentInParent<SmoothFollow>().target;
        if (target)
        {

            time = time + Time.deltaTime;
            text.text = "speed : " + target.GetComponent<Rigidbody>().velocity.magnitude + "\n"
                + "times travel : " + time;
        }
        else
        {
            if (time != 0)
            {
                time = 0;
            }
        }
        */
    }
}
