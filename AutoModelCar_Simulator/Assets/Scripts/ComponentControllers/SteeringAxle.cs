using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;

public class SteeringAxle : MonoBehaviour
{
    public string topic = "/steering";
    public RosConnector Connection;
    public Transform left_wheel, right_wheel;
    public AnimationCurve steering_interp = new AnimationCurve(new Keyframe(0, -25), new Keyframe(180, 25)); //converting coordinates from [0,180] to [-25,25]


    public float T {
        get {return Vector3.Distance(left_wheel.position, right_wheel.position);}
        set {Globals.Instance.DevConsole.error("Setting interwheel distance not implemented!");}
    }

    public float steering_topic {
        set { 
            _steering_topic = value;
            _steering_goal = steering_interp.Evaluate(value); 
            if(instant_response) _steering_real = steering_interp.Evaluate(value); 
        }
        get { return _steering_topic; }
    }
    public float steering_real_deg {
        get { 
            return _steering_real;
        }
    }

    public float steering_real_rad {
        get { return steering_real_deg*Mathf.Deg2Rad; }
    }

    private float _steering_topic;

    private string steering_sub;
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

    // Start is called before the first frame update
    void Start()
    {
        if(!Connection) Connection = Globals.Instance.Connection;
        steering_sub = Connection.RosSocket.Subscribe<std_msgs.UInt8>(topic, steering_callback);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(instant_response) return;
        ang_accel = (_steering_goal - _steering_real) * accl_mul;
        ang_speed += ang_accel;
        ang_speed *= speed_damp;
        _steering_real += ang_speed * speed_mul;
    }

    private void steering_callback(std_msgs.UInt8 data) {
        steering_topic = (float)data.data;        
        //Globals.Instance.DevConsole.print("New steering: " + data.data);
    }
}
