using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;
using nav_msgs = RosSharp.RosBridgeClient.Messages.Navigation;
using geo_msgs = RosSharp.RosBridgeClient.Messages.Geometry;

public class CarController : MonoBehaviour
{
    [Header("Car Axles")]
    public Transform backAxle;
    public Transform frontAxle;
    public Transform frontWheel_right;
    public Transform frontWheel_left;

    
    public float speed {
        set { speed_topic = value; speed_mps = speed_interp.Evaluate(value); }
        get { return speed_topic; }
    }
    public float steering {
        set { steering_topic = value; Phi = Mathf.Deg2Rad*steering_interp.Evaluate(value); }
        get { return steering_topic; }
    }

    //Private variables
    private RosSocket rosSocket;
    private string speed_sub;
    private string steering_sub;
    private string odom_pub;
    private float L;    // Distance between axles
    private float Phi;  // Steering angle of virtual front wheel (IN RADIAN!)
    private float steering_topic;
    private float R;    // Radius of current turning circle
    private Vector3 C;  // Center of current turning circle
    private float speed_mps; // Speed of the car in m/s (not /speed topic values or rpm)
    private float speed_topic; // Speed of car in rpm
    private float stime;
    private float odom_frequency = 30.0f;

    void Start()
    {
        //rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol("ws://192.168.178.23:9090"));
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketSharpProtocol("ws://192.168.178.23:9090"));

        speed_sub = rosSocket.Subscribe<std_msgs.Int16>("/manual_control/speed", speed_callback);
        steering_sub = rosSocket.Subscribe<std_msgs.UInt8>("/steering", steering_callback);
        odom_pub = rosSocket.Advertise<nav_msgs.Odometry>("/localization/odom/5");
        stime = Time.time;
    }

    void Update()
    {
        //steering = 180; //For testing
        //speed = 250;
        float Tan_Phi = (Mathf.Tan(Phi)!=0.0f) ? Mathf.Tan(Phi) : 0.0000001f; //Avoid Tan(Phi)=0, because it causes trouble

        L = Vector3.Distance(backAxle.position, frontAxle.position); // L is the distance between the front and back axle
        R = Mathf.Abs(L / Tan_Phi);

        float T = Vector3.Distance(frontWheel_left.position, frontWheel_right.position); //Interwheel distance

        float amount_of_rotation;
        if(Phi>0) {
            C = backAxle.position + transform.right * R;
            amount_of_rotation = (speed_mps*Time.deltaTime)/R;

            //Individual wheel rotation
            frontWheel_left.localRotation = Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan(L/(R-T/2)), 0);
            frontWheel_right.localRotation = Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan(L/(R+T/2)), 0);
        }
        else {
            C = backAxle.position - transform.right * R;
            amount_of_rotation = -(speed_mps*Time.deltaTime)/R;

            //Individual wheel rotation
            frontWheel_left.localRotation = Quaternion.Euler(0, -1f * Mathf.Rad2Deg * Mathf.Atan(L/(R-T/2)), 0);
            frontWheel_right.localRotation = Quaternion.Euler(0, -1f * Mathf.Rad2Deg * Mathf.Atan(L/(R+T/2)), 0);
        }

        // GUI and HUD stuff
        if(Globals.Instance.c_marker.gameObject.activeSelf) {
            Globals.Instance.c_marker.position = C;
        }
        if(Globals.Instance.CircleDraw.gameObject.activeSelf) {
            Globals.Instance.CircleDraw.center = C;
            Globals.Instance.CircleDraw.xradius = R;
            Globals.Instance.CircleDraw.yradius = R;
        }

        transform.RotateAround(C, Vector3.up, Mathf.Rad2Deg*amount_of_rotation);
    }

    void FixedUpdate() {
        if(Time.time < stime + 1.0f/odom_frequency) return;

        nav_msgs.Odometry co = new nav_msgs.Odometry();
        co.pose.pose.position = new geo_msgs.Point();
        Vector3 pos = RosSharp.TransformExtensions.Unity2Ros(transform.position);
        co.pose.pose.position.x = pos.x;
        co.pose.pose.position.y = pos.y;
        co.pose.pose.position.z = pos.z;

        co.pose.pose.orientation = new geo_msgs.Quaternion();
        Quaternion rot = RosSharp.TransformExtensions.Unity2Ros(transform.rotation);
        co.pose.pose.orientation.x = rot.x;
        co.pose.pose.orientation.y = rot.y;
        co.pose.pose.orientation.z = rot.z;
        co.pose.pose.orientation.w = rot.w;

        rosSocket.Publish(odom_pub, co);        
        stime = Time.time;
    }

    private AnimationCurve speed_interp = new AnimationCurve(new Keyframe(0, 0), new Keyframe(150, 0), new Keyframe(600, 3), new Keyframe(2500, 10)); //converting from [0, 150, 600] to [0, 0, 3] (m/s)
    private void speed_callback(std_msgs.Int16 data) {
        speed = (float)data.data;
        //Globals.Instance.DevConsole.print("New speed: " + data.data);
    }

    private AnimationCurve steering_interp = new AnimationCurve(new Keyframe(0, -25), new Keyframe(180, 25)); //converting coordinates from [0,180] to [-25,25]
    private void steering_callback(std_msgs.UInt8 data) {
        steering = (float)data.data;        
        //Globals.Instance.DevConsole.print("New steering: " + data.data);
    }
}
