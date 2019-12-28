using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
    protected LineRenderer line;

    public float destination_approx_distance = 50;

    public Vector3 destination;



    public float target_rudder_angle_level = 0;
    public float max_rudder_angle = 0;
    public float rudder_angle = 0;
    public float rudder_angle_acceleration = 0;
    public Vector3 delta_rotation = Vector3.zero;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            destination = MouseControl.MouseToWorld();
            line.SetPosition(1, destination);
            line.enabled = true;
        }

        if ((transform.position - destination).magnitude < destination_approx_distance) {
            line.enabled = false;
        }
    }

    


    private void MakeWaypointLine() {
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = 5f;
        line.endWidth = 5f;
        line.material.SetColor("_Color", Color.white);
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.useWorldSpace = true;
        line.transform.parent = transform;
    }

   
}
