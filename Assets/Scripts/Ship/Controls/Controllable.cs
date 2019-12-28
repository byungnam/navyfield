using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable {
    float CalculateCurrentVelocity(float target_velocity_level, float max_velocity, float current_velocity, float acceleration, float snatch_value);
    float CalculateCurrentRudderAngle(float target_rudder_angle_level, float max_rudder_angle, float current_rudder_angle, float rudder_angle_acceleration, float snatch_value);
    (Vector3, Vector3) CalculateDelta(float current_velocity, float current_rudder_angle, float max_velocity);
    float GetBreakDistance(float current_velocity, float acceleration);
    float CalculateTargetRudderLevel(Transform transform, float current_velocity, float current_rudder_angle, float max_rudder_angle, float rudder_angle_acceleration, Vector3 destination, float snatch_value);
    void Move(Transform transform, Vector3 delta_position, Vector3 delta_rotation);
}

public class Controllable : IControllable {

    public bool destination_reached = true;

    public float CalculateCurrentVelocity(float target_velocity_level, float max_velocity, float current_velocity, float acceleration, float snatch_value) {
        float target_velocity = target_velocity_level * max_velocity;
        float velocity = 0f;
        if (Mathf.Abs(target_velocity - current_velocity) < snatch_value) {
            velocity = target_velocity;
        }
        else if (target_velocity > current_velocity) {
            velocity = current_velocity + acceleration * Time.deltaTime;
        }
        else if (target_velocity < current_velocity) {
            velocity = current_velocity - acceleration * Time.deltaTime;
        }
        
        return velocity;
    }

    public float CalculateCurrentRudderAngle(float target_rudder_angle_level, float max_rudder_angle, float current_rudder_angle, float rudder_angle_acceleration, float snatch_value) {
        float target_rudder_angle = target_rudder_angle_level * max_rudder_angle;
        float rudder_angle;
        if (Mathf.Abs(target_rudder_angle - current_rudder_angle) < snatch_value) {
            rudder_angle = target_rudder_angle;
        }
        else {
            rudder_angle = current_rudder_angle + Mathf.Sign(target_rudder_angle - current_rudder_angle) * rudder_angle_acceleration * Time.deltaTime;
        }
        
        return rudder_angle;
    }

    public (Vector3, Vector3) CalculateDelta(float current_velocity, float current_rudder_angle, float max_velocity) {
        Vector3 delta_position = new Vector3(0, 0, current_velocity * Time.deltaTime);
        Vector3 delta_rotation = new Vector3(0, Mathf.Deg2Rad * current_rudder_angle * current_velocity * Time.deltaTime, 0);
        
        return (delta_position, delta_rotation);
    }

    public float GetBreakDistance(float current_velocity, float acceleration) {
        float t = current_velocity / acceleration;
        float s = current_velocity * t / 2f;
        return s;
    }

    public float CalculateTargetRudderLevel(Transform transform, float current_velocity, float current_rudder_angle, float max_rudder_angle, float rudder_angle_acceleration, Vector3 destination, float snatch_value) {
        float target_rudder_angle_level = 0f;
        if (destination_reached) {
            return target_rudder_angle_level;
        }
        float angle_between_forward_dest = Vector3.Angle(transform.forward, destination - transform.position);
        float angle_dir = AngleDir(transform.forward, destination - transform.position, transform.up, snatch_value);

        float rudder_centering_time = current_rudder_angle / rudder_angle_acceleration;
        float rotation_while_centering = Mathf.Deg2Rad * current_rudder_angle * current_velocity * rudder_centering_time / 2;
        if (rotation_while_centering > angle_between_forward_dest) {
            target_rudder_angle_level = 0f;
        }
        else {
            target_rudder_angle_level = angle_dir;
        }
        return target_rudder_angle_level;
    }

    public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up, float snatch_value) {
        Vector3 right = Vector3.Cross(up, fwd);        // right vector
        float dir = Vector3.Dot(right, targetDir);
        if (dir > snatch_value) {
            return 1f;
        }
        else if (dir < snatch_value) {
            return -1f;
        }
        else {
            return 0f;
        }
    }

    public void Move(Transform transform, Vector3 delta_position, Vector3 delta_rotation) {
        if (delta_position.sqrMagnitude != 0) {
            transform.Translate(delta_position);
        }
        if (delta_rotation.sqrMagnitude != 0) {
            transform.Rotate(delta_rotation);
        }
    }

}
