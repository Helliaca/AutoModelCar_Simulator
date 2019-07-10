using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using autominy_msgs = RosSharp.RosBridgeClient.Messages.Autominy;

public class SteeringAxle : MonoBehaviour
{
    public RosConnector Connection;

    public AnimationCurve steering_pwm_interp = new AnimationCurve(new Keyframe(950, -25), new Keyframe(2150, 25)); 
    public AnimationCurve steering_real_interp = new AnimationCurve(new Keyframe(-25, -25), new Keyframe(0, 0), new Keyframe(25, 25)); 
    public AnimationCurve steering_normalized_interp = new AnimationCurve(new Keyframe(-1, 25), new Keyframe(1, -25)); 

    public string steering_pwm_topic = "/manual_control/steering_pwm";
    public string steering_real_topic = "/manual_control/steering_real";
    public string steering_normalized_topic = "/manual_control/steering_nrm";
    public Transform left_wheel, right_wheel;


    public float T {
        get {return Vector3.Distance(left_wheel.position, right_wheel.position);}
        set {Globals.Instance.DevConsole.error("Setting interwheel distance not implemented!");}
    }

    // public float steering_topic {
    //     set { 
    //         _steering_topic = value;
    //         _steering_goal = steering_interp.Evaluate(value); 
    //         if(instant_response) _steering_real = steering_interp.Evaluate(value); 
    //     }
    //     get { return _steering_topic; }
    // }
    public float steering_real_deg {
        get { 
            return _steering_real;
        }
    }

    public float steering_real_rad {
        get { return steering_real_deg*Mathf.Deg2Rad; }
    }

    private float _steering_topic;

    private string steering_pwm_sub, steering_real_sub, steering_normalized_sub;
    private float _steering_goal = 0.01f;
    private float _steering_real= 0.01f;
    private float ang_speed = 0.0f, ang_accel = 0.0f;
    public float accl_mul = 0.4f;
    public float speed_damp = 0.8f;
    public float speed_mul = 0.055f;
    public bool instant_response = false;



    public void set_wheel_rotations(float left_phi, float right_phi) {
        left_wheel.localRotation = Quaternion.Euler(0, left_phi, 0);
        right_wheel.localRotation = Quaternion.Euler(0, right_phi, 0);
    }

    void Start()
    {
        if(!Connection) Connection = Globals.Instance.Connection;
        steering_pwm_sub = Connection.RosSocket.Subscribe<autominy_msgs.Autominy_SteeringPWMCommand>(steering_pwm_topic, steering_pwm_callback);
        steering_real_sub = Connection.RosSocket.Subscribe<autominy_msgs.Autominy_SteeringCommand>(steering_real_topic, steering_real_callback);
        steering_normalized_sub = Connection.RosSocket.Subscribe<autominy_msgs.Autominy_NormalizedSteeringCommand>(steering_normalized_topic, steering_normalized_callback);
    }

    void FixedUpdate()
    {
        if(instant_response) return;
        //Different acceleration model (constant acceleration)
        // float sign = (_steering_goal - _steering_real) / Mathf.Abs((_steering_goal - _steering_real)+0.0001f);
        // ang_accel = sign * 15.0f * accl_mul;
        ang_accel = (_steering_goal - _steering_real) * accl_mul;
        ang_speed += ang_accel;
        ang_speed *= speed_damp;
        _steering_real += ang_speed * speed_mul;
    }

    private void steering_pwm_callback(autominy_msgs.Autominy_SteeringPWMCommand data) {
        _steering_goal = steering_pwm_interp.Evaluate((float)data.value);
        if(instant_response) _steering_real = _steering_goal;
    }

    private void steering_real_callback(autominy_msgs.Autominy_SteeringCommand data) {
        _steering_goal = steering_real_interp.Evaluate((float)data.value);
        if(instant_response) _steering_real = _steering_goal;
    }

    AnimationCurve interp = new AnimationCurve(new Keyframe(0, -25), new Keyframe(180, 25));
    private void steering_normalized_callback(autominy_msgs.Autominy_NormalizedSteeringCommand data) {
        _steering_goal = steering_normalized_interp.Evaluate((float)data.value);
        if(instant_response) _steering_real = _steering_goal;
    }

    public void set_steering(float v) {
        _steering_goal = v;
        if(instant_response) _steering_real = _steering_goal;
    }
}
