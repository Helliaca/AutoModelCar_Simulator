using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;

public class PropulsionAxle : MonoBehaviour
{
    public RosConnector Connection;
    public AnimationCurve acceleration_curve;
    public AnimationCurve speed_interp = new AnimationCurve(new Keyframe(0, 0), new Keyframe(150, 0), new Keyframe(600, 3), new Keyframe(2500, 10)); //converting from [0, 150, 600] to [0, 0, 3] (m/s)

    public string topic = "/manual_control/speed";
    public Transform left_wheel, right_wheel;
    
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

    private float ang_speed = 0.0f, ang_accel = 0.0f;
    public float accl_mul = 0.0f;
    public float speed_damp = 0.9f;
    public float speed_mul = 0.01f;
    public bool instant_response = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!Connection) Connection = Globals.Instance.Connection;
        speed_sub = Connection.RosSocket.Subscribe<std_msgs.Int16>(topic, speed_callback);
    }

    void FixedUpdate()
    {
        if(instant_response) return;
        ang_accel = (_speed_goal - _speed_real) * accl_mul;
        ang_speed += ang_accel;
        ang_speed *= speed_damp;
        _speed_real += ang_speed * speed_mul;
    }

    private void speed_callback(std_msgs.Int16 data) {
        //Globals.Instance.DevConsole.print("New speed: " + data.data);
        speed_topic = (float)data.data;
    }

    public void stop() {
        _speed_real = 0.0f;
    }
}
