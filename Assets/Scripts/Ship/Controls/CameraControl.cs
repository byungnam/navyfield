using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    private Camera main_camera;
    private Camera shell_camera;


    // Start is called before the first frame update
    void Start()
    {
        main_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        // shell_camera = GameObject.Find("Shell Camera").GetComponent<Camera>();
        // shell_camera.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            CenterCamera();
        }
        /*
        if (Input.GetKeyDown(KeyCode.H)) {
            if (fired_shells.Count > 0) {

                if (!shell_camera.enabled) {
                    fired_shell_enumerator = fired_shells.GetEnumerator();
                    fired_shell_enumerator.MoveNext();
                    EnableFollowCam(fired_shell_enumerator.Current.transform);
                }
                else {
                    DisableFollowCam();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            if (shell_camera.enabled) {
                if (!fired_shell_enumerator.MoveNext()) {
                    fired_shell_enumerator = fired_shells.GetEnumerator();
                    fired_shell_enumerator.MoveNext();
                }
                EnableFollowCam(fired_shell_enumerator.Current.transform);
            }
        }
        */
    }

    void CenterCamera() {
        float rad = Vector3.Angle(new Vector3(0, -1, 0), Camera.main.transform.forward) * Mathf.Deg2Rad;
        main_camera.transform.position = new Vector3(transform.position.x, main_camera.transform.position.y, transform.position.z - Mathf.Tan(rad) * main_camera.transform.position.y);
        // terrainCam.transform.position = new Vector3(transform.position.x, terrainCam.transform.position.y, transform.position.z - Mathf.Tan(rad) * terrainCam.transform.position.y);
    }

    public void EnableFollowCam(Transform target) {
        shell_camera.GetComponent<SmoothFollow>().SetTarget(target);
        shell_camera.enabled = true;
        main_camera.enabled = false;
    }

    public void DisableFollowCam() {
        StartCoroutine(_DisableFollowCam());
    }

    private IEnumerator _DisableFollowCam() {
        yield return new WaitForSeconds(1f);
        shell_camera.enabled = false;
        main_camera.enabled = true;
        shell_camera.transform.position = transform.position;
    }

}
