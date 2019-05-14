
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;
using nav_msgs = RosSharp.RosBridgeClient.Messages.Navigation;
using geo_msgs = RosSharp.RosBridgeClient.Messages.Geometry;

public class CarController : MonoBehaviour
{

    private RosSocket rosSocket;

    private string speed_sub;
    private string steering_sub;
    private string odom_pub;

    private double speed;
    private double steering;


     
    // these should be updated with the car model
    private Vector3 bit;  //  position of back ideal tire
    private  Vector3 fit;  // position of front ideal tire
    
    private Vector3 old_bit;  // old position of back tire, to calculate the traveled distance.
    private Vector3 cen;  // center of rotation
    private double rad; // radius of rotation
    private double amount_of_rotation;

    private const double L = 0.257; // distance between bit and fit, this depends on car model
    private const double M_PI_2 = Math.PI/2;
    private const double m_per_ticks = 0.01369863f;
    private static double traveled_dist = 0.0;
    private static double accumulate_dist = 0.0;
    private static int ticks = 0;


    // Start is called before the first frame update
    void Start()
    {
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol("ws://192.168.178.23:9090"));

        speed_sub = rosSocket.Subscribe<std_msgs.Int16>("/manual_control/speed", speed_callback);
        steering_sub = rosSocket.Subscribe<std_msgs.UInt8>("/steering", steering_callback);
        odom_pub = rosSocket.Advertise<nav_msgs.Odometry>("/localization/odom/5");


    
        

        /*
        
        old_bit = bit;
        
        */
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = new Vector3(0,0,0);
        offset += transform.forward * (float)(speed * Math.Cos(Mathf.Deg2Rad * steering));
        offset += transform.right * (float)(speed * Math.Sin(Mathf.Deg2Rad * steering));
        transform.position += offset * Time.deltaTime * 0.005f;
        transform.Rotate(Vector3.up, (float)(speed * steering * Time.deltaTime * 0.01f));

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





        Vector3 p1, p2;

        if(steering > 0){
            p1 = rotate(fit, bit, -M_PI_2, true);
            p2 = rotate(bit, fit, -(M_PI_2-steering), false);
        }else{
            p1 = rotate(fit, bit, M_PI_2, false);
            p2 = rotate(bit, fit, M_PI_2-steering, true);
        }

        // the radius of rotation
        rad = Math.Abs( L/Math.Tan(steering));

        // center of rotaion
        cen = intersect(bit, p1, fit, p2);

        // amount of rotaion
        amount_of_rotation = (Math.Tan(steering) * speed * Time.deltaTime)/L;



        /*
            TODO...

            - now rotate the model car about the point "cen" in xz-plane by angel of "amount_of_rotation"
            - "bit" and "fit" should also be transformed as they supposed to be sticked with the car model
        */


        // traveled dist from last frame is given by the length of arch between old position and new position
        double d =  Math.Sqrt( Math.Pow( old_bit.x-bit.x,2)+ Math.Pow(old_bit.z-bit.z ,2) );
        traveled_dist =  rad * Math.Acos(1.0 - (d*d)/(2*rad*rad) );

        old_bit = bit;


        accumulate_dist += traveled_dist;

        ticks = Convert.ToInt32(Math.Floor(accumulate_dist/m_per_ticks));
        accumulate_dist = accumulate_dist%m_per_ticks;  // this should be a floating point modulo
        // or   accumulate_dist -= ticks*ticks_per_m;

        /*
            TODO...

            - publish "bit" as a GPS publisher
            - publish "ticks" 
        */
        
    }

    private Vector3 rotate(Vector3 p, Vector3 c, double angle, bool ccw) // rotate p about c
    {
        double _sin = Math.Sin(angle);
        double _cos = Math.Cos(angle);
        p -= c;

        if (ccw){                      // counter clockwise rotation
            p = new Vector3((float)(p.x * _cos + p.z * _sin), 0.0f,
                             (float)(p.z * _cos - p.x * _sin));
        }else{                         // clockwise rotation
            p = new Vector3((float)(p.x * _cos - p.z * _sin), 0.0f,
                             (float)(p.z * _cos + p.x * _sin));
        }

        return p + c;
    }

    private Vector3 intersect(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) // point intersection between p1p2 and p3p4
    {
        double temp = ((p4.x - p3.x) * (p1.z - p3.z) - (p4.z - p3.z) * (p1.x - p3.x)) /
                ((p4.z - p3.z) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.z - p1.z));

        return new Vector3((float)(p1.x + temp * (p2.x - p1.x)), 0.0f, (float)(p1.z + temp * (p2.z - p1.z)));
    }

    public static double remap (double value, double a1, double b1, double a2, double b2) {
        if(value > b1)
            value = b1;
        else if(value < a1)
            value = a1;

        return Math.Round((value - a1) / (b1 - a1) * (b2 - a2) + a2);
    }




    private void speed_callback(std_msgs.Int16 data) {
        if(data.data >= 0)
            speed =  -remap(data.data, 120, 800, 0, 600); 
        else speed = -remap(data.data, -800, -120, -600, 0); 


        // speed =  (float)data.data;
        //Globals.Instance.DevConsole.print("New speed: " + data.data);
    }

    private void steering_callback(std_msgs.UInt8 data) {
        steering =  remap(data.data, 0, 180, -25, 25) ; //  (double)data.data;
        if(steering == 0.0)
            steering = 0.0001;
        else
            steering = (steering*Math.PI)/180.0;



        // steering = (steering/180.0f) * 50.0f - 25.0f; //converting coordinates from [0,180] to [-25,25]
        //Globals.Instance.DevConsole.print("New steering: " + data.data);
    }
}
