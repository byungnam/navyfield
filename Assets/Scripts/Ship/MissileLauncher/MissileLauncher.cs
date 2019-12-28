using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : Photon.MonoBehaviour {

    private float fire_delay = 1f;
    private float remaining_fire_delay = 0f;

    
    void Start () {
		
	}
	
	void Update () {
        if (remaining_fire_delay > 0) {
            remaining_fire_delay -= Time.deltaTime;
            if (remaining_fire_delay < 0) {
                remaining_fire_delay = 0;
            }
        }

        if (Input.GetKey(KeyCode.Q)) {
            Fire(MouseControl.MouseToWorld());
        }
    }


    /*
    // currently not using
    public void Fire_(Vector3 destination) {
        if (remaining_fire_delay == 0) {
            remaining_fire_delay = fire_delay;
            foreach (Transform silo in transform) {
                if (silo.name != "MissileSilo") {
                    continue;
                }

                GameObject asm = Instantiate(Resources.Load<GameObject>("MissileLaunchers/Missiles/Anti-Ship Missile"), transform.position, Quaternion.Euler(270, 0, 0));
                Missile m = asm.GetComponent<Missile>();
                m.SetDestination(destination);
                Physics.IgnoreCollision(asm.GetComponent<Collider>(), GetComponentInParent<Collider>());
            }
            
        }
    }
    */

    public void Fire(Vector3 destination) {
        if (remaining_fire_delay == 0) {
            remaining_fire_delay = fire_delay;
            StartCoroutine(_Fire(destination));
            photonView.RPC("RPCPlayFireSound", PhotonTargets.All);
        }
    }

    [PunRPC]
    private void RPCPlayFireSound() {
        GetComponent<AudioSource>().Play();
    }

    private IEnumerator _Fire(Vector3 destination) {
        foreach (Transform silo in transform) {
            if (silo.name != "MissileSilo") {
                continue;
            }
            GameObject asm = Instantiate(Resources.Load<GameObject>("MissileLaunchers/Missiles/Anti-Ship Missile"), transform.position, Quaternion.Euler(270, 0, 0));
            Missile m = asm.GetComponent<Missile>();
            m.SetDestination(destination);
            Physics.IgnoreCollision(asm.GetComponent<Collider>(), GetComponentInParent<Collider>());
            yield return new WaitForSeconds(0.5f);
        }
    }

}
