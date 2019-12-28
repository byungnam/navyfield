// Smooth Follow from Standard Assets
// Converted to C# because I fucking hate UnityScript and it's inexistant C# interoperability
// If you have C# code and you want to edit SmoothFollow's vars ingame, use this instead.
using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {

    // The target we are following
    private Transform target;
    // The distance in the x-z plane to the target
    private float distance = 150.0f;
    // the height we want the camera to be above the target
    private float height = 70f;
    // How much we 
    // private float heightDamping = 5.0f;
    private float rotationDamping = 3.0f;

    // Place the script in the Camera-Control group in the component menu
    [AddComponentMenu("Camera-Control/Smooth Follow")]

    float last;

    void FixedUpdate() {
        // Early out if we don't have a target
        if (!target) {
            if (Time.time - last > 1f) {
                GetComponent<Camera>().enabled = false;
            }
            return;
        }
        last = Time.time;
        // Calculate the current rotation angles
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = target.GetComponent<Shell>().GetFiredTurret().transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        //currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        currentHeight = wantedHeight;

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Always look at the target
        transform.LookAt(target);
    }

    public void SetTarget(Transform target) {
        if (this.target != null) {
            this.target.GetComponent<Shell>().SetIsFollowed(false);
        }
        this.target = target;
        this.target.GetComponent<Shell>().SetIsFollowed(true);
    }
}