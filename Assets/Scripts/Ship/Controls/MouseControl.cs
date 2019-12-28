using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    static Plane ground = new Plane(Vector3.up, 0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector3 MouseToWorld() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distToGround = -1f;
        ground.Raycast(ray, out distToGround);
        Vector3 worldPos = ray.GetPoint(distToGround);
        return worldPos;
    }
}
