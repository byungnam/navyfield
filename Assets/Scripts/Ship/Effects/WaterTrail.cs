using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrail : MonoBehaviour
{
    bool is_trail_on = false;
    public ParticleSystem trail;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (is_trail_on) {
            trail.Play();
            
        }
        else {
            trail.Stop();
            
        }
    }

}
