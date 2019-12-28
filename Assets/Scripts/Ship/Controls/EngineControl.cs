using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineControl : MonoBehaviour
{
    public float target_velocity_level = 0;
    public float max_velocity = 100;
    public float velocity = 0;
    public float acceleration = 0;

    public Vector3 delta_position = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.F)) {
            target_velocity_level += 0.25f;
            target_velocity_level = Mathf.Clamp(target_velocity_level, -0.25f, 1f);
        }
        if (Input.GetKeyDown(KeyCode.V)) {
            target_velocity_level -= 0.25f;
            target_velocity_level = Mathf.Clamp(target_velocity_level, -0.25f, 1f);
        }
    }
}
