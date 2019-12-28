using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    public float prepare_time;

    private LineRenderer line;

    private void MakeAltitudeLine() {
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = 3f;
        line.endWidth = 3f;
        line.material.SetColor("_Color", Color.green);
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.useWorldSpace = true;
        line.transform.parent = transform;
        
    }

    void Awake() {
        MakeAltitudeLine();

    }
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, new Vector3(transform.position.x, 0, transform.position.z));
    }
}
