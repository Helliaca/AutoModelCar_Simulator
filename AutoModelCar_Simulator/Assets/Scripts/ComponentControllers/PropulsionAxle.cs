using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;

public class PropulsionAxle : MonoBehaviour
{
    public RosConnector Connection;

    //converting from [0, 150, 600] to [0, 0, 3] (m/s), Use this for value interpoaltion
    public AnimationCurve speed_interp = new AnimationCurve(new Keyframe(0, 0), new Keyframe(150, 0), new Keyframe(600, 3), new Keyframe(2500, 10)); 

    public string topic = "/manual_control/speed";
    public Transform left_wheel, right_wheel;
    
    // Inter-wheel distance:
    public float T {
        get {return Vector3.Distance(left_wheel.position, right_wheel.position);}
        set {Globals.Instance.DevConsole.error("Setting interwheel distance not implemented!");}
    }

    public float speed_topic {
        set { 
            _speed_topic = value;
            _speed_goal = speed_interp.Evaluate(value); 
            if(instant_response) _speed_real = speed_interp.Evaluate(value); 
         }
        get { return _speed_topic; }
    }
    public float speed_real {
        get { return _speed_real; }
    }

    private float _speed_real=0, _speed_topic;
    private string speed_sub;
    private float accel_stime = 0.0f;
    private float _speed_goal = 0.0f;
    private float last_frame_real_speed = 0.0f;

    private float delta_speed = 0.0f, delta_accel = 0.0f;
    public float delta_accl_mul = 0.4f;
    public float delta_speed_damp = 0.8f;
    public float delta_speed_mul = 0.055f;
    public bool instant_response = false;

    
    void Start()
    {
        if(!Connection) Connection = Globals.Instance.Connection;
        speed_sub = Connection.RosSocket.Subscribe<std_msgs.Int16>(topic, speed_callback);
    }

    void FixedUpdate()
    {
        if(instant_response) return;
        delta_accel = (_speed_goal - _speed_real) * delta_accl_mul;
        delta_speed += delta_accel;
        delta_speed *= delta_speed_damp;
        _speed_real += delta_speed * delta_speed_mul;
    }

    private void speed_callback(std_msgs.Int16 data) {
        //Globals.Instance.DevConsole.print("New speed: " + data.data);
        speed_topic = (float)data.data;
    }

    public void stop() {
        _speed_goal = 0.0f;
        _speed_real = 0.0f;
    }
}
