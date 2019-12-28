using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : Photon.MonoBehaviour {

    public GameObject airplane;
    public List<GameObject> airplanes;
    public int num_planes;
    public float prepare_time;
    public bool in_air;

    public int flight_num;
    public float preparing;
    public bool prepared;

    private float next_time;
    

    // Use this for initialization
    void Start () {
        airplanes = new List<GameObject>();
        if (photonView.isMine) {
            prepare_time = num_planes * airplane.GetComponent<Airplane>().prepare_time;
            prepared = false;
            next_time = 0;
        }
    }

    // Update is called once per frame
    void Update() {
        preparing += Time.deltaTime;

        if (!prepared && Time.time >= next_time && airplanes.Count < num_planes) {
            AddAirplane(airplane);
            next_time += airplane.GetComponent<Airplane>().prepare_time;
        }
        if (preparing > prepare_time) {
            preparing = prepare_time;
            if (airplanes.Count == num_planes) {
                prepared = true;
            }
        }
    }

    public void AddAirplane(GameObject airplane) {
        GameObject airplane_go = Instantiate<GameObject>(airplane, transform);
        airplanes.Add(airplane_go);
        airplane.transform.localPosition = new Vector3(10f * Random.Range(-1f, 1f), 0, 10f * Random.Range(-1f, 1f));
    }
}
